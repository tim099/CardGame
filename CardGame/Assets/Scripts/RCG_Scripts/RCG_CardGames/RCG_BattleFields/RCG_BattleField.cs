using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCL.Core.UI;
using UnityEngine.UI;
namespace RCG {
    [UCL.Core.ATTR.EnableUCLEditor]
    public class RCG_BattleField : MonoBehaviour
    {
        static public RCG_BattleField ins = null;

        /// <summary>
        /// 目前的行動單位
        /// </summary>
        public RCG_Unit ActiveUnit
        {
            get; protected set;
        } = null;

        public Button m_SelectNoneButton = null;
        public UCL.Core.FileLib.UCL_FileInspector m_MonsterFiles = null;
        public List<RCG_Unit> m_SelectedUnits = new List<RCG_Unit>();
        public List<RCG_Unit> m_Characters = new List<RCG_Unit>();
        public List<RCG_Unit> m_Monsters = new List<RCG_Unit>();
        /// <summary>
        /// 已經啟動過的玩家腳色
        /// </summary>
        public HashSet<RCG_Unit> m_ActivatedUnit = new HashSet<RCG_Unit>();
        public RCG_BattlePositionSetting m_PlayerBattlePositionSetting = null;
        public RCG_BattlePositionSetting m_EnemyBattlePositionSetting = null;
        protected TargetType m_TargetType = TargetType.Close;
        protected bool m_Entered = false;

        virtual public void Init()
        {
            ins = this;
            m_SelectNoneButton.onClick.AddListener(SelectNone);
            m_SelectNoneButton.gameObject.SetActive(false);
            m_PlayerBattlePositionSetting.Init();
            m_EnemyBattlePositionSetting.Init();
        }
        private void Awake() {
            m_MonsterFiles = Resources.Load<UCL.Core.FileLib.UCL_FileInspector>(PathConst.MonsterResource + "/MonsterFileInspector");
            //Init();
        }

        public List<RCG_Unit> GetAllPlayerUnits()
        {
            return m_PlayerBattlePositionSetting.GetAllUnits();
        }
        public List<RCG_Unit> GetPlayerFrontUnits()
        {
            return m_PlayerBattlePositionSetting.GetFrontUntis();
        }
        public List<RCG_Unit> GetPlayerBackUnits()
        {
            return m_PlayerBattlePositionSetting.GetBackUntis();
        }
        public List<RCG_Unit> GetAllEnemyUnits()
        {
            return m_EnemyBattlePositionSetting.GetAllUnits();
        }
        public List<RCG_Unit> GetEnemyFrontUnits()
        {
            return m_EnemyBattlePositionSetting.GetFrontUntis();
        }
        public List<RCG_Unit> GetEnemyBackUnits()
        {
            return m_EnemyBattlePositionSetting.GetBackUntis();
        }
        public List<RCG_Unit> GetAllUnits()
        {
            var aUnits = m_PlayerBattlePositionSetting.GetAllUnits();
            aUnits.AddRange(m_EnemyBattlePositionSetting.GetAllUnits());
            return aUnits;
        }
        /// <summary>
        /// 在指定位置生成單位
        /// </summary>
        /// <param name="iUnitPos">前排或後排</param>
        /// <param name="iPosition">位置</param>
        /// <param name="iUnitName">名稱</param>
        /// <param name="iIsMonster">是否是怪物</param>
        /// <returns></returns>
        public RCG_Unit CreateMonsterAt(UnitPos iUnitPos, int iPosition, string iUnitName, bool iIsMonster)
        {
            RCG_Unit aUnit = CreateUnit(iUnitName, iIsMonster);
            aUnit.m_UnitPos = iUnitPos;
            aUnit.m_UnitPosId = iPosition;
            if (iIsMonster) { 
                m_Monsters.Add(aUnit);
            }
            else { 
                m_Characters.Add(aUnit);
            }
            if (aUnit == null)
            {
                return null;
            }
            aUnit.Init(iIsMonster);
            if (iIsMonster)
            {
                m_EnemyBattlePositionSetting.SetUnit(iUnitPos, iPosition, aUnit);
            }
            else
            {
                m_PlayerBattlePositionSetting.SetUnit(iUnitPos, iPosition, aUnit);
            }
            

            return aUnit;
        }
        public RCG_Unit CreateUnit(string aUnitName, bool iIsMonster = true)
        {
            RCG_Unit aUnit = Resources.Load<RCG_Unit>((iIsMonster? PathConst.MonsterResource: PathConst.CharacterResource) + "/" + aUnitName);
            if (aUnit == null)
            {
                Debug.LogError("CreateMonster Fail:" + aUnitName + ", Not Exist!!");
                return null;
            }
            var aNewUnit = Instantiate(aUnit, transform);
            return aNewUnit;
        }
        /// <summary>
        /// 進入戰鬥 進行初始化
        /// </summary>
        virtual public void EnterBattle() {
            if(m_Entered) {
                Debug.LogError("EnterBattle() Fail!! Already Entered!!");
                return;
            }
            gameObject.SetActive(true);
            m_Entered = true;
            ActiveUnit = null;
            m_ActivatedUnit.Clear();

            CreateMonsterAt(UnitPos.Front, 1, "Abigail", true);
            CreateMonsterAt(UnitPos.Back, 1, "Archer", true);
            CreateMonsterAt(UnitPos.Back, 2, "Knight", true);

            CreateCharacters();
            
            //SetSelectMode(TargetType.Close);
        }
        /// <summary>
        /// 戰鬥結束
        /// </summary>
        public void ExitBattle() {
            if(!m_Entered) {
                Debug.LogError("ExitBattle() Fail!! Not Entered!!");
                return;
            }
            m_Entered = false;
            gameObject.SetActive(false);
        }
        public void SetSelectMode(TargetType iTargetType)
        {
            m_TargetType = iTargetType;
            Debug.LogWarning("m_TargetType:" + m_TargetType.ToString());
            m_SelectedUnits.Clear();
            if (m_TargetType == TargetType.Close)
            {
                m_SelectNoneButton.gameObject.SetActive(false);
                m_PlayerBattlePositionSetting.SelectPlayer(m_ActivatedUnit);
                m_EnemyBattlePositionSetting.SetAllSelection(false);
                return;
            }
            switch (m_TargetType)
            {
                case TargetType.Off:
                    {
                        m_SelectNoneButton.gameObject.SetActive(false);
                        m_PlayerBattlePositionSetting.SetAllSelection(false);
                        m_EnemyBattlePositionSetting.SetAllSelection(false);
                        break;
                    }
                case TargetType.None:
                    {
                        m_SelectNoneButton.gameObject.SetActive(true);
                        m_PlayerBattlePositionSetting.SetAllSelection(false);
                        m_EnemyBattlePositionSetting.SetAllSelection(false);
                        break;
                    }
                case TargetType.All:
                    {
                        m_SelectNoneButton.gameObject.SetActive(false);
                        m_PlayerBattlePositionSetting.SetAllSelection(true);
                        m_EnemyBattlePositionSetting.SetAllSelection(true);
                        break;
                    }
                case TargetType.Allied:
                    {
                        m_SelectNoneButton.gameObject.SetActive(false);
                        m_PlayerBattlePositionSetting.SetAllSelection(true);
                        m_EnemyBattlePositionSetting.SetAllSelection(false);
                        break;
                    }
                case TargetType.AlliedFront:
                    {
                        m_SelectNoneButton.gameObject.SetActive(false);
                        m_PlayerBattlePositionSetting.SelectFront();
                        m_EnemyBattlePositionSetting.SetAllSelection(false);
                        break;
                    }
                case TargetType.AlliedBack:
                    {
                        m_SelectNoneButton.gameObject.SetActive(false);
                        m_PlayerBattlePositionSetting.SelectBack();
                        m_EnemyBattlePositionSetting.SetAllSelection(false);
                        break;
                    }
                case TargetType.Enemy:
                    {
                        m_SelectNoneButton.gameObject.SetActive(false);
                        m_PlayerBattlePositionSetting.SetAllSelection(false);
                        m_EnemyBattlePositionSetting.SetAllSelection(true);
                        break;
                    }
                case TargetType.EnemyFront:
                    {
                        m_SelectNoneButton.gameObject.SetActive(false);
                        m_PlayerBattlePositionSetting.SetAllSelection(false);
                        m_EnemyBattlePositionSetting.SelectFront();
                        break;
                    }
                case TargetType.EnemyBack:
                    {
                        m_SelectNoneButton.gameObject.SetActive(false);
                        m_PlayerBattlePositionSetting.SetAllSelection(false);
                        m_EnemyBattlePositionSetting.SelectBack();
                        break;
                    }
            }
        }
        /// <summary>
        /// 回合初始化
        /// </summary>
        public void TurnInit()
        {
            Debug.LogWarning("Battle TurnInit()!!");
            ClearSelectedUnits();
            ClearActivatedUnits();
            UpdateMonsterActions();
            SetSelectMode(TargetType.Close);
        }
        /// <summary>
        /// 玩家行動結束 敵人開始行動
        /// </summary>
        public void TurnStart()
        {
            TurnEnd();
            foreach (RCG_Unit u in m_Monsters)
            {
                if (u == null)
                {
                    continue;
                }
                var m = u.gameObject.GetComponent<RCG_Monster>();
                if (m)
                {
                    m.Act();
                }
                u.EndTurn();
            }
        }
        /// <summary>
        /// 敵方戰鬥行動結束
        /// </summary>
        public void TurnEnd()
        {
            foreach (RCG_Unit u in m_Characters)
            {
                if (u == null)
                {
                    continue;
                }
                u.EndTurn();
            }
            RCG_BattleManager.ins.EnemyTurnEnd();
        }
        public void ClearSelectedUnits()
        {
            m_SelectedUnits.Clear();
        }
        public void ClearActivatedUnits()
        {
            m_ActivatedUnit.Clear();
            SetActiveUnit(null);
        }
        public void SelectUnit(RCG_Unit iUnit)
        {
            if(m_TargetType == TargetType.Close)
            {
                SetActiveUnit(iUnit);
                SetSelectMode(TargetType.Close);
                return;
            }
            m_SelectedUnits.Add(iUnit);
            RCG_Player.ins.SelectTargets(m_SelectedUnits);
        }
        /// <summary>
        /// 設定目前行動的腳色
        /// </summary>
        /// <param name="iUnit"></param>
        public void SetActiveUnit(RCG_Unit iUnit)
        {
            if (iUnit != null && m_ActivatedUnit.Contains(iUnit))
            {
                Debug.LogError("SetCurrentUnit Fail!!Unit" + iUnit.name + " Already Selected!!");
                return;
            }
            if (ActiveUnit != null)
            {
                ActiveUnit.DeselectUnit();
            }
            m_ActivatedUnit.Add(iUnit);
            ActiveUnit = iUnit;
            if (ActiveUnit != null)
            {
                ActiveUnit.SelectUnit();
            }
            RCG_Player.ins.UpdateCardStatus();
        }
        /// <summary>
        /// 選擇無目標按鈕
        /// </summary>
        public void SelectNone()
        {
            RCG_Player.ins.SelectTargets(m_SelectedUnits);
            SetSelectMode(TargetType.Close);
        }


        public void CreateUnits() {

        }

        public void CreateCharacters()
        {
            int count_front = 0;
            int count_back = 0;
            RCG_Unit unit;
            foreach (var character_data in RCG_DataService.ins.m_CharacterDatas)
            {
                if(character_data.m_battle_position == UnitPos.Front)
                {
                    unit = CreateMonsterAt(UnitPos.Front, count_front++, character_data.m_character_name, false);
                }
                else
                {
                    unit = CreateMonsterAt(UnitPos.Back, count_back++, character_data.m_character_name, false);
                }
                unit.MaxHp = character_data.m_max_hp;
                unit.SetHp(character_data.m_hp);
            }
        }


        public void TriggerCardEffect(int target, RCG_CardData card_data){
            Debug.Log("TriggerCardEffect");
            //RCG_CardEffectHandler.TriggerCardEffectOnUnits(m_units ,target, card_data);
        }

        /// <summary>
        /// 更新敵人的動作
        /// </summary>
        public void UpdateMonsterActions()
        {
            foreach (RCG_Unit u in m_Monsters)
            {
                if (u == null)
                {
                    continue;
                }
                var m = u.gameObject.GetComponent<RCG_Monster>();
                if (m)
                {
                    m.PrepareToAct();
                }
            }
        }
    }
}

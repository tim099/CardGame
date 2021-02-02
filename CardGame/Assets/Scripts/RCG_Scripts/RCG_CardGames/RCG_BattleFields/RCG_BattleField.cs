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
        public List<RCG_Unit> m_Units = new List<RCG_Unit>();
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
            RCG_Unit aUnit = CreateUnit(iUnitName);
            if(aUnit == null)
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
        public RCG_Unit CreateUnit(string aUnitName)
        {
            RCG_Unit aUnit = Resources.Load<RCG_Unit>(PathConst.MonsterResource + "/" + aUnitName);
            if (aUnit == null)
            {
                Debug.LogError("CreateMonster Fail:" + aUnitName + ", Not Exist!!");
                return null;
            }
            var aNewUnit = Instantiate(aUnit, transform);
            m_Units.Add(aNewUnit);
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

            CreateMonsterAt(UnitPos.Front, 1, "Knight", true);
            CreateMonsterAt(UnitPos.Back, 1, "Archer", true);
            CreateMonsterAt(UnitPos.Back, 2, "Knight", true);

            CreateMonsterAt(UnitPos.Front, 0, "Knight", false);
            CreateMonsterAt(UnitPos.Front, 2, "Knight", false);
            CreateMonsterAt(UnitPos.Back, 1, "Archer", false);
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
                        m_PlayerBattlePositionSetting.SetFrontSelection(true);
                        m_PlayerBattlePositionSetting.SetBackSelection(false);
                        m_EnemyBattlePositionSetting.SetAllSelection(false);
                        break;
                    }
                case TargetType.AlliedBack:
                    {
                        m_SelectNoneButton.gameObject.SetActive(false);
                        m_PlayerBattlePositionSetting.SetFrontSelection(false);
                        m_PlayerBattlePositionSetting.SetBackSelection(true);
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
                        m_EnemyBattlePositionSetting.SetFrontSelection(true);
                        m_EnemyBattlePositionSetting.SetBackSelection(false);
                        break;
                    }
                case TargetType.EnemyBack:
                    {
                        m_SelectNoneButton.gameObject.SetActive(false);
                        m_PlayerBattlePositionSetting.SetAllSelection(false);
                        m_EnemyBattlePositionSetting.SetFrontSelection(false);
                        m_EnemyBattlePositionSetting.SetBackSelection(true);
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
            SetSelectMode(TargetType.Close);
        }
        /// <summary>
        /// 玩家行動結束 敵人開始行動
        /// </summary>
        public void TurnStart()
        {
            TurnEnd();
        }
        /// <summary>
        /// 敵方戰鬥行動結束
        /// </summary>
        public void TurnEnd()
        {
            foreach (RCG_Unit u in m_Units)
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


        public void TriggerCardEffect(int target, RCG_CardData card_data){
            Debug.Log("TriggerCardEffect");
            //RCG_CardEffectHandler.TriggerCardEffectOnUnits(m_units ,target, card_data);
        }
    }
}

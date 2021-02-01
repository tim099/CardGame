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
        public Button m_SelectNoneButton = null;
        public UCL.Core.FileLib.UCL_FileInspector m_MonsterFiles = null;
        public List<RCG_Unit> m_SelectedUnits = new List<RCG_Unit>();
        public List<RCG_Unit> m_Units = new List<RCG_Unit>();
        public RCG_BattlePositionSetting m_PlayerBattlePositionSetting = null;
        public RCG_BattlePositionSetting m_EnemyBattlePositionSetting = null;
        bool m_Entered = false;

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
        /// 在指定位置生成怪物
        /// </summary>
        /// <param name="iMonsterPos"></param>
        /// <param name="aPosition"></param>
        /// <param name="aMonsterName"></param>
        /// <returns></returns>
        public RCG_Unit CreateMonsterAt(MonsterPos iMonsterPos, int aPosition, string aMonsterName)
        {
            RCG_Unit aUnit = CreateMonster(aMonsterName);
            if(aUnit == null)
            {
                return null;
            }
            m_EnemyBattlePositionSetting.SetUnit(iMonsterPos, aPosition, aUnit);

            return aUnit;
        }
        public RCG_Unit CreateMonster(string aMonsterName)
        {
            RCG_Unit aUnit = Resources.Load<RCG_Unit>(PathConst.MonsterResource + "/" + aMonsterName);
            if (aUnit == null)
            {
                Debug.LogError("CreateMonster Fail:" + aMonsterName + ", Not Exist!!");
                return null;
            }
            var aNewUnit = Instantiate(aUnit, transform);
            aNewUnit.Init();
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
            m_Entered = true;
            gameObject.SetActive(true);
            CreateMonsterAt(MonsterPos.Front, 1, "Knight");
            CreateMonsterAt(MonsterPos.Back, 0, "Knight");
            CreateMonsterAt(MonsterPos.Back, 2, "Knight");
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
            m_SelectedUnits.Clear();
            if (iTargetType == TargetType.Close)
            {
                m_SelectNoneButton.gameObject.SetActive(false);
                m_PlayerBattlePositionSetting.SetAllSelection(false);
                m_EnemyBattlePositionSetting.SetAllSelection(false);
                return;
            }
            Debug.LogError("iTargetType:" + iTargetType.ToString());
            switch (iTargetType)
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
        public void SelectUnit(RCG_Unit iUnit)
        {
            m_SelectedUnits.Add(iUnit);
            RCG_Player.ins.SelectTargets(m_SelectedUnits);
        }
        /// <summary>
        /// 選擇無目標按鈕
        /// </summary>
        public void SelectNone()
        {
            RCG_Player.ins.SelectTargets(m_SelectedUnits);
            SetSelectMode(TargetType.Close);
        }

        public void Test() {
            
        }

        public void CreateUnits() {

        }
        /// <summary>
        /// 敵方戰鬥行動結束
        /// </summary>
        public void TurnEnd() {
            foreach(RCG_Unit u in m_Units){
                if(u == null){
                    continue;
                }
                u.EndTurn();
            }
        }

        public void TriggerCardEffect(int target, RCG_CardData card_data){
            Debug.Log("TriggerCardEffect");
            //RCG_CardEffectHandler.TriggerCardEffectOnUnits(m_units ,target, card_data);
        }
    }
}

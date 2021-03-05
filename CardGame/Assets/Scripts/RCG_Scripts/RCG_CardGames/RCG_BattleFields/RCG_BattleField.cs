using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCL.Core.UI;
using UnityEngine.UI;
using UCL.TweenLib;

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
        protected System.Action<List<RCG_Unit>> m_SelectEndAct = null;
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
        public List<RCG_Unit> GetRowUnits(UnitPos iPos, bool iIsEnemy)
        {
            if (iIsEnemy) return GetEnemyUnits(iPos);
            return GetPlayerUnits(iPos);
        }
        public List<RCG_Unit> GetPlayerUnits(UnitPos iPos)
        {
            switch (iPos)
            {
                case UnitPos.Front: return m_PlayerBattlePositionSetting.GetFrontUntis();
                case UnitPos.Back: return m_PlayerBattlePositionSetting.GetBackUntis();
            }
            return m_PlayerBattlePositionSetting.GetFrontUntis();
        }
        public List<RCG_Unit> GetAllEnemyUnits()
        {
            return m_EnemyBattlePositionSetting.GetAllUnits();
        }
        public List<RCG_Unit> GetEnemyUnits(UnitPos iPos)
        {
            switch (iPos) {
                case UnitPos.Front: return m_EnemyBattlePositionSetting.GetFrontUntis();
                case UnitPos.Back: return m_EnemyBattlePositionSetting.GetBackUntis();
            }
            return m_EnemyBattlePositionSetting.GetFrontUntis();
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
            aUnit.Init(iIsMonster, iUnitPos);
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
            RCG_Unit aUnit = Resources.Load<RCG_Unit>((iIsMonster ? PathConst.MonsterResource : PathConst.CharacterResource) + "/" + aUnitName);
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
            if (m_Entered) {
                Debug.LogError("EnterBattle() Fail!! Already Entered!!");
                return;
            }
            gameObject.SetActive(true);
            m_Entered = true;
            ActiveUnit = null;
            m_ActivatedUnit.Clear();

            CreateCharacters();

            //SetSelectMode(TargetType.Close);
        }
        /// <summary>
        /// 地圖上的怪物資訊
        /// </summary>
        virtual public void SetMonsters(RCG_MonsterSet iMonsterSet)
        {
            int frontCount = 1;
            int backCount = 1;
            foreach (var monster in iMonsterSet.m_Monsters)
            {
                frontCount = frontCount % 3;
                backCount = backCount % 3;
                if (monster.m_MonsterPos == UnitPos.Front)
                {
                    CreateMonsterAt(monster.m_MonsterPos, frontCount++, monster.m_MonsterName, true);
                }
                else
                {
                    CreateMonsterAt(monster.m_MonsterPos, backCount++, monster.m_MonsterName, true);
                }

            }
        }
        /// <summary>
        /// 戰鬥結束
        /// </summary>
        public void ExitBattle() {
            if (!m_Entered) {
                Debug.LogError("ExitBattle() Fail!! Not Entered!!");
                return;
            }
            m_Entered = false;
            gameObject.SetActive(false);
        }
        /// <summary>
        /// 設定到選擇腳色模式
        /// </summary>
        public void SetActiveUnitSelectMode()
        {
            SetSelectMode(TargetType.Close, SelectActiveUnit);
        }
        public void SetSelectMode(TargetType iTargetType, System.Action<List<RCG_Unit>> iSelectEndAct = null)
        {
            m_TargetType = iTargetType;
            m_SelectEndAct = iSelectEndAct;
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
            SetActiveUnitSelectMode();
        }
        /// <summary>
        /// 玩家行動結束 敵人開始行動
        /// </summary>
        public void TurnStart()
        {
            SetActiveUnit(null);
            MonsterAction();
        }
        /// <summary>
        /// 敵人行動
        /// </summary>
        public void MonsterAction()
        {
            foreach (RCG_Unit u in m_Monsters)
            {
                if (u == null)
                {
                    continue;
                }
                var m = u.gameObject.GetComponent<RCG_Monster>();
                if (!m.m_acted)
                {
                    try//目前有Exception導致流程失效
                    {
                        m.Act(delegate () {
                            MonsterAction();
                        });
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError(e);
                    }
                    return;
                }
            }
            TurnEnd();
        }
        /// <summary>
        /// 敵方戰鬥行動結束
        /// </summary>
        public void TurnEnd()
        {
            Debug.LogWarning("RCG_BattleField TurnEnd()");
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
        /// <summary>
        /// 單位被選中後呼叫
        /// </summary>
        /// <param name="iUnit"></param>
        public void SelectUnit(RCG_Unit iUnit)
        {
            //if (m_TargetType == TargetType.Close)
            //{
            //    SetActiveUnit(iUnit);
            //    SetSelectMode(TargetType.Close);
            //    return;
            //}
            m_SelectedUnits.Add(iUnit);
            if (m_SelectEndAct != null)
            {
                var aAct = m_SelectEndAct;
                m_SelectEndAct = null;
                aAct.Invoke(m_SelectedUnits.Clone());
            }
        }
        public void SelectActiveUnit(List<RCG_Unit> iUnits)
        {
            if (iUnits == null || iUnits.Count == 0)
            {
                return;
            }
            SetActiveUnit(iUnits[0]);
            SetActiveUnitSelectMode();
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
            RCG_Player.ins.SetActiveUnit(ActiveUnit);
        }
        /// <summary>
        /// 演出多段攻擊動畫 攻擊群體敵人
        /// </summary>
        /// <param name="iAttaker"></param>
        /// <param name="iTarget"></param>
        /// <param name="iAtk"></param>
        /// <param name="iEndAction"></param>
        public void AttackUnit(RCG_Unit iAttaker, RCG_Unit iTarget, int iAtk, int iAtkTimes, System.Action iEndAction)
        {
            if(iTarget == null)
            {
                iEndAction.Invoke();
            }
            AttackUnits(iAttaker, new List<RCG_Unit>() { iTarget }, iAtk, iAtkTimes, iEndAction);
        }
        /// <summary>
        /// 演出多段攻擊動畫 攻擊群體敵人
        /// </summary>
        /// <param name="iAttaker"></param>
        /// <param name="iTargets"></param>
        /// <param name="iAtk"></param>
        /// <param name="iAtkTimes"></param>
        /// <param name="iEndAction"></param>
        public void AttackUnits(RCG_Unit iAttaker, List<RCG_Unit> iTargets, int iAtk, int iAtkTimes, System.Action iEndAction)
        {
            if (iAtkTimes <= 0)
            {
                iEndAction.Invoke();
            }
            System.Action<int> aAttackAct = null;
            aAttackAct = delegate (int iAt)
            {
                AttackUnits(iAttaker, iTargets, iAtk, delegate () {
                    if (iAt < iAtkTimes)
                    {
                        aAttackAct(iAt + 1);
                    }
                    else
                    {
                        iEndAction.Invoke();
                    }
                });
            };
            aAttackAct.Invoke(1);
        }
        /// <summary>
        /// 演出攻擊動畫 攻擊群體敵人(單次)
        /// </summary>
        /// <param name="iAttaker"></param>
        /// <param name="iTargets"></param>
        /// <param name="iAtk"></param>
        protected void AttackUnits(RCG_Unit iAttaker, List<RCG_Unit> iTargets, int iAtk, System.Action iEndAction)
        {
            if (iTargets == null || iTargets.Count == 0)
            {
                iEndAction.Invoke();
                return;
            }
            for (int i = iTargets.Count - 1; i >= 0; i--)
            {
                if (iTargets[i].IsDead)
                {
                    iTargets.RemoveAt(i);
                }
            }
            if (iTargets.Count == 0)
            {
                iEndAction.Invoke();
                return;
            }
            int aAtk = iAtk;
            if (iAttaker != null)
            {
                aAtk = iAttaker.GetAtk(iAtk);
            }

            var aSeq = LibTween.Sequence();
            {
                aSeq.Append(delegate ()
                {
                    foreach (var aTarget in iTargets)
                    {
                        if (!aTarget.IsDead)
                        {
                            aTarget.UnitHit(aAtk);
                        }
                    }
                });
                var aTweener = LibTween.Tweener(0.2f);//aSeq.AppendInterval(0.8f);
                aSeq.Append(aTweener);
                foreach (var aTarget in iTargets)
                {
                    if (!aTarget.IsDead)
                    {
                        aTweener.AddComponent(aTarget.transform.TC_LocalShake(35, 20, true));
                    }
                }
                aSeq.AppendInterval(0.4f);
            }
            aSeq.OnComplete(iEndAction);
            aSeq.Start();
        }

        /// <summary>
        /// 演出恢復動畫
        /// </summary>
        /// <param name="iHealer"></param>
        /// <param name="iTargets"></param>
        /// <param name="iHealAmount"></param>
        /// <param name="iEndAction"></param>
        public void HealUnits(RCG_Unit iHealer, List<RCG_Unit> iTargets, int iHealAmount, System.Action iEndAction)
        {
            if (iTargets == null || iTargets.Count == 0)
            {
                iEndAction.Invoke();
                return;
            }
            for (int i = iTargets.Count - 1; i >= 0; i--)
            {
                if (iTargets[i].IsDead)
                {
                    iTargets.RemoveAt(i);
                }
            }
            if (iTargets.Count == 0)
            {
                iEndAction.Invoke();
                return;
            }
            int aHealAmount = iHealAmount;
            if (iHealer != null)
            {
                aHealAmount = iHealer.GetHealAmount(iHealAmount);
            }

            var aSeq = LibTween.Sequence();
            {
                aSeq.Append(delegate ()
                {
                    foreach (var aTarget in iTargets)
                    {
                        if (!aTarget.IsDead)
                        {
                            aTarget.UnitHeal(aHealAmount);
                        }
                    }
                });
                var aTweener = LibTween.Tweener(0.2f);//aSeq.AppendInterval(0.8f);
                aTweener.SetBackfolding(true);
                aSeq.Append(aTweener);
                foreach (var aTarget in iTargets)
                {
                    if (!aTarget.IsDead)
                    {
                        aTweener.AddComponent(aTarget.transform.TC_LocalMove(0, 20, 0));
                    }
                }
                aSeq.AppendInterval(0.4f);
            }
            aSeq.OnComplete(iEndAction);
            aSeq.Start();
        }
        /// <summary>
        /// 單位死亡時觸發
        /// </summary>
        /// <param name="iUnit"></param>
        public void OnUnitDead(RCG_Unit iUnit)
        {
            Debug.LogWarning("OnUnitDead:" + iUnit.name+ ",m_TargetType:"+ m_TargetType.ToString());
            if (!iUnit.IsEnemy)
            {
                ///腳色死亡 清除ActiveUnit狀態
                if(iUnit == ActiveUnit)
                {
                    SetActiveUnit(null);
                }
                ///腳色死亡 更新選擇腳色狀態
                if(m_TargetType == TargetType.Close)
                {
                    SetActiveUnitSelectMode();
                }
            }
            RCG_Player.ins.OnUnitDead(iUnit);
        }
        /// <summary>
        /// 選擇無目標按鈕
        /// </summary>
        public void SelectNone()
        {
            RCG_Player.ins.SelectTargets(m_SelectedUnits);
            SetActiveUnitSelectMode();
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

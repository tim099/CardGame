using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCL.TweenLib;
namespace RCG {
    public enum AttackRange
    {
        FrontEnemy = 0,//前排敵人
        BackEnemy,//後排敵人
        AllEnemy,//全體敵人

        FrontEnemyAOE,//前排敵人群攻
        BackEnemyAOE,//後排敵人群攻
        AllEnemyAOE,//全體敵人敵人群攻

        FrontAllied,//前排我方
        BackAllied,//後排我方
        AllAllied,//我方全體

        FrontAlliedAOE,//前排我方群攻
        BackAlliedAOE,//後排我方群攻
        AllAlliedAOE,//我方全體群攻

        AllAOE,//敵我全體群攻
        RowAOE,//全排群攻
        Player,//使用者
        Target,//選中的目標
    }
    public class RCG_CardAttackEffect : RCG_BattleEffect {
        public static string GetAttackRangeDes(AttackRange iAttackRange)
        {
            return UCL.Core.LocalizeLib.UCL_LocalizeManager.Get("AttackRange_" + iAttackRange.ToString());
        }
        public static List<RCG_Unit> GetAttackRangeTarget(TriggerEffectData iTriggerEffectData, AttackRange iAttackRange)
        {
            List<RCG_Unit> aTargets = null;
            switch (iAttackRange)
            {
                case AttackRange.FrontEnemyAOE:
                    {
                        aTargets = RCG_BattleField.ins.GetEnemyFrontUnits();
                        break;
                    }
                case AttackRange.BackEnemyAOE:
                    {
                        aTargets = RCG_BattleField.ins.GetEnemyBackUnits();
                        break;
                    }
                case AttackRange.AllEnemyAOE:
                    {
                        aTargets = RCG_BattleField.ins.GetAllEnemyUnits();
                        break;
                    }
                case AttackRange.FrontAlliedAOE:
                    {
                        aTargets = RCG_BattleField.ins.GetPlayerFrontUnits();
                        break;
                    }
                case AttackRange.BackAlliedAOE:
                    {
                        aTargets = RCG_BattleField.ins.GetPlayerBackUnits();
                        break;
                    }
                case AttackRange.AllAlliedAOE:
                    {
                        aTargets = RCG_BattleField.ins.GetAllPlayerUnits();
                        break;
                    }
                case AttackRange.RowAOE:
                    {
                        if (iTriggerEffectData.m_Targets.Count > 0)
                        {
                            var aTarget = iTriggerEffectData.m_Targets[0];
                            aTargets = RCG_BattleField.ins.GetRowUnits(aTarget.m_UnitPos, aTarget.IsEnemy);
                        }
                        else
                        {
                            aTargets = new List<RCG_Unit>();
                        }
                        break;
                    }
                case AttackRange.AllAOE:
                    {
                        aTargets = RCG_BattleField.ins.GetAllUnits();
                        break;
                    }
                case AttackRange.Player:
                    {
                        aTargets = new List<RCG_Unit>() { iTriggerEffectData.m_PlayerUnit };
                        break;
                    }
            }
            if (aTargets == null) aTargets = iTriggerEffectData.m_Targets.Clone();
            return aTargets;
        }
        [System.Serializable]
        public struct AttackData {
            public int m_Int;
            public float m_Float;
        }
        public enum AttackType {
            Normal = 0,
            Magic,
        }
        public int Atk
        {
            get
            {
                if (RCG_BattleField.ins != null && RCG_BattleField.ins.ActiveUnit != null)
                {

                    return RCG_BattleField.ins.ActiveUnit.GetAtk(m_Atk);
                }
                return m_Atk;
            }
        }
        public int m_Atk = 0;
        public AttackRange m_AttackRange = AttackRange.AllEnemy;
        public int m_AtkTimes = 1;
        public AttackType m_AttackType = AttackType.Normal;
        public string m_VFX = string.Empty;
        //public AttackData m_AttackData;
        override public void OnGUI(int iID) {
            base.OnGUI(iID);
        }
        override public string Description {
            get {
                string aAttackRangeDes = GetAttackRangeDes(m_AttackRange);
                string aAtkStr = m_Atk.ToString();
                int aAtk = Atk;
                if (aAtk > m_Atk)//Buff
                {
                    aAtkStr = aAtk.ToString().RichTextColor("00FF00");
                }
                else if (aAtk < m_Atk)//Debuff
                {
                    aAtkStr = aAtk.ToString().RichTextColor("FF0000");
                }
                if (m_AtkTimes > 1) {
                    return UCL.Core.LocalizeLib.UCL_LocalizeManager.Get("Attack_Des", aAtkStr, m_AtkTimes, aAttackRangeDes) + "\n";
                } else {
                    return UCL.Core.LocalizeLib.UCL_LocalizeManager.Get("Attack_DesSingle", aAtkStr, aAttackRangeDes) + "\n";
                }
            }
        }
        public override void TriggerEffect(TriggerEffectData iTriggerEffectData, Action iEndAction)
        {
            if (!string.IsNullOrEmpty(m_VFX))
            {
                RCG_VFXManager.ins.CreateVFX(m_VFX);
            }
            var aTargets = GetAttackRangeTarget(iTriggerEffectData, m_AttackRange);
            //Debug.LogError("aTargets:" + aTargets.Count);
            if(aTargets != null && aTargets.Count > 0)
            {
                for (int i = 0; i < m_AtkTimes; i++)
                {
                    iTriggerEffectData.p_Player.AddPlayerAction(new RCG_PlayerAttackAction(iTriggerEffectData.m_PlayerUnit, aTargets, m_Atk));
                }
            }

            iEndAction.Invoke();
        }


        //override public void LoadJson(UCL.Core.JsonLib.JsonData data) {
        //    UCL.Core.JsonLib.JsonConvert.LoadDataFromJson(this, data);
        //}
        //override public UCL.Core.JsonLib.JsonData ToJson() {
        //    UCL.Core.JsonLib.JsonData data = base.ToJson();
        //    UCL.Core.JsonLib.JsonConvert.SaveDataToJson(this, data);
        //    return data;
        //}
    }
}
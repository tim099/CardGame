using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCL.TweenLib;
namespace RCG {
    public class RCG_CardAttackEffect : RCG_BattleEffect {
        [System.Serializable]
        public struct AttackData {
            public int m_Int;
            public float m_Float;
        }
        public enum AttackType {
            Normal = 0,
            Magic,
        }
        public enum AttackRange
        {
            Front = 0,//前排敵人
            Back,//後排敵人
            All,//全體敵人

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
            Player,//使用者
        }
        public int m_Atk = 0;
        public AttackRange m_AttackRange = AttackRange.All;
        public int m_AtkTimes = 1;
        public AttackType m_AttackType = AttackType.Normal;
        public string m_VFX = string.Empty;
        //public AttackData m_AttackData;
        override public void OnGUI(int iID) {
            base.OnGUI(iID);
        }
        override public string Description {
            get {
                string aAttackRangeDes = UCL.Core.LocalizeLib.UCL_LocalizeManager.Get("AttackRange_" + m_AttackRange.ToString());
                if (m_AtkTimes > 1) {
                    return UCL.Core.LocalizeLib.UCL_LocalizeManager.Get("Attack_Des", m_Atk, m_AtkTimes, aAttackRangeDes) + "\n";
                } else {
                    return UCL.Core.LocalizeLib.UCL_LocalizeManager.Get("Attack_DesSingle", m_Atk, aAttackRangeDes) + "\n";
                }
            }
        }
        public override void TriggerEffect(TriggerEffectData iTriggerEffectData, Action iEndAction)
        {
            if (!string.IsNullOrEmpty(m_VFX))
            {
                RCG_VFXManager.ins.CreateVFX(m_VFX);
            }
            var aTargets = iTriggerEffectData.m_Targets.Clone();
            switch (m_AttackRange)
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
            //Debug.LogError("aTargets:" + aTargets.Count);
            if(aTargets != null && aTargets.Count > 0)
            {
                for (int i = 0; i < m_AtkTimes; i++)
                {
                    iTriggerEffectData.p_Player.AddPlayerAction(new RCG_PlayerAttackAction(aTargets, m_Atk));
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
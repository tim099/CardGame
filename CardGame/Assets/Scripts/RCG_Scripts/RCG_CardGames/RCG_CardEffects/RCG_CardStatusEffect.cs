using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG
{
    public class RCG_CardStatusEffect : RCG_CardEffect
    {
        public StatusType m_StatusType = StatusType.None;
        public AttackRange m_BuffRange = AttackRange.Target;
        public bool m_ShowBuffRangeDes = true;
        public int m_Amount = 0;
        override public string Description
        {
            get
            {
                if (m_ShowBuffRangeDes)
                {
                    return UCL.Core.LocalizeLib.UCL_LocalizeManager.Get("AppliedStatus_Des", m_Amount,
                        RCG_Status.GetStatusDes(m_StatusType), RCG_CardAttackEffect.GetAttackRangeDes(m_BuffRange))
                        + System.Environment.NewLine;
                }
                return UCL.Core.LocalizeLib.UCL_LocalizeManager.Get("AppliedStatus_DesNoTarget", m_Amount, 
                    RCG_Status.GetStatusDes(m_StatusType))
                    + System.Environment.NewLine;
            }
        }
        public override void TriggerEffect(TriggerEffectData iTriggerEffectData, System.Action iEndAction)
        {
            var aUnits = RCG_CardAttackEffect.GetAttackRangeTarget(iTriggerEffectData, m_BuffRange);
            foreach(var aUnit in aUnits)
            {
                RCG_Player.ins.AddPlayerAction(CreateAction.StatusAction(aUnit, m_StatusType, m_Amount));
            }
            //iTriggerEffectData.p_Player.AlterCost(m_CostAlter);
            iEndAction.Invoke();
        }
    }
}
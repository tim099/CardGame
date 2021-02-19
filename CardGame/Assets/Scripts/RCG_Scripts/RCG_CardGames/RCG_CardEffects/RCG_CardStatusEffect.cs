using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG
{
    public class RCG_CardStatusEffect : RCG_CardEffect
    {
        public StatusType m_StatusType = StatusType.None;
        public AttackRange m_BuffRange = AttackRange.Target;
        public int m_Amount = 0;
        override public string Description
        {
            get
            {
                return UCL.Core.LocalizeLib.UCL_LocalizeManager.Get("AppliedStatus_Des", m_Amount, 
                    RCG_Status.GetStatusDes(m_StatusType), RCG_CardAttackEffect.GetAttackRangeDes(m_BuffRange))
                    + System.Environment.NewLine;
            }
        }
        public override void TriggerEffect(TriggerEffectData iTriggerEffectData, System.Action iEndAction)
        {
            //iTriggerEffectData.p_Player.AlterCost(m_CostAlter);
            iEndAction.Invoke();
        }
    }
}
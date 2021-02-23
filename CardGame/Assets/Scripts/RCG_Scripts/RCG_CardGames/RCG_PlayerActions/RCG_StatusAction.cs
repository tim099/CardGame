using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG {
    public class RCG_StatusAction : RCG_PlayerAction
    {
        public RCG_Unit m_Target = null;
        public StatusType m_StatusType = StatusType.None;
        public int m_Amount = 0;
        public RCG_StatusAction(RCG_Unit iTarget, StatusType iStatusType,int iAmount)
        {
            m_Target = iTarget;
            m_StatusType = iStatusType;
            m_Amount = iAmount;
        }
        public override void Trigger(Action iEndAction)
        {
            m_Target.AddStatusEffect(m_StatusType, m_Amount);
            iEndAction.Invoke();
        }
    }
}



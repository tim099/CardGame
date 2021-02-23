using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace RCG
{
    public class RCG_PlayerActionTrigger : RCG_PlayerAction
    {
        System.Action m_Act = null;
        public RCG_PlayerActionTrigger(System.Action iAct)
        {
            m_Act = iAct;
        }
        public override void Trigger(Action iEndAct)
        {
            m_Act?.Invoke();
            iEndAct.Invoke();
        }
    }
}


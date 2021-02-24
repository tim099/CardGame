using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace RCG
{
    public class RCG_PlayerActionTrigger : RCG_PlayerAction
    {
        System.Action m_Act = null;
        System.Action<System.Action> m_Act2 = null;
        public RCG_PlayerActionTrigger(System.Action iAct)
        {
            m_Act = iAct;
        }
        public RCG_PlayerActionTrigger(System.Action<System.Action> iAct2)
        {
            m_Act2 = iAct2;
        }
        public override void Trigger(Action iEndAct)
        {
            if (m_Act != null)
            {
                m_Act.Invoke();
                iEndAct.Invoke();
                return;
            }
            if(m_Act2 != null)
            {
                m_Act2.Invoke(iEndAct);
            }
        }
    }
}


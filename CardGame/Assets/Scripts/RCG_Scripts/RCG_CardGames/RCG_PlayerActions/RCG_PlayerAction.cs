using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG
{
    public static class CreateAction
    {
        public static RCG_StatusAction StatusAction(RCG_Unit iTarget, StatusType iStatusType, int iAmount)
        {
            return new RCG_StatusAction(iTarget, iStatusType, iAmount);
        }
        public static RCG_PlayerActionTrigger ActionTrigger(System.Action iAct)
        {
            return new RCG_PlayerActionTrigger(iAct);
        }
    }
    public class RCG_PlayerAction
    {
        virtual public void Trigger(System.Action iEndAct) { }
    }
}
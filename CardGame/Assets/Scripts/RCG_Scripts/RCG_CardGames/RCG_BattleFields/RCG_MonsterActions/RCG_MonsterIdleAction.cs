using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UCL.TweenLib;

namespace RCG
{
    public class RCG_MonsterIdleAction : RCG_MonsterAction
    {
        string m_ActionName;
        string m_Something;

        override public bool ActionAvailable()
        {
            return true;
        }

        public override void TriggerAction(Action iEndAction)
        {
            var aSeq = LibTween.Sequence();
            aSeq.AppendInterval(1.0f);
            aSeq.OnComplete(iEndAction);
            aSeq.Start();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG
{
    public class RCG_DrawCardAction : RCG_PlayerAction
    {
        public override void Trigger(Action iEndAct)
        {
            RCG_Player.ins.DrawCardAnim(iEndAct);
        }
    }
}


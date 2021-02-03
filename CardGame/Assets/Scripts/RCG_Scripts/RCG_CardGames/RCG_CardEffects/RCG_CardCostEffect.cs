using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG {
    public class RCG_CardCostEffect : RCG_CardEffect {
        public int m_CostAlter = 0;
        override public string Description { get {
                return UCL.Core.LocalizeLib.UCL_LocalizeManager.Get("AddCost_Des", m_CostAlter) + System.Environment.NewLine;
            }
        }
        public override void TriggerEffect(TriggerEffectData iTriggerEffectData, Action iEndAction)
        {
            iTriggerEffectData.p_Player.AlterCost(m_CostAlter);
            iEndAction.Invoke();
        }
    }
}
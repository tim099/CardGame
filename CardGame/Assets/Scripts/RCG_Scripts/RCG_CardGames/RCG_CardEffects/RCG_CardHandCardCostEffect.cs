using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace RCG
{
    /// <summary>
    /// 增減手牌Cost的效果
    /// </summary>
    public class RCG_CardHandCardCostEffect : RCG_CardEffect
    {
        public int m_CostAlter = 0;
        override public string Description
        {
            get
            {
                if (m_CostAlter > 0)
                {
                    return UCL.Core.LocalizeLib.UCL_LocalizeManager.Get("AddHandCardCost_Des", m_CostAlter) + System.Environment.NewLine;
                }
                return UCL.Core.LocalizeLib.UCL_LocalizeManager.Get("SubHandCardCost_Des", -m_CostAlter) + System.Environment.NewLine;
            }
        }
        public override void TriggerEffect(TriggerEffectData iTriggerEffectData, Action iEndAction)
        {
            iTriggerEffectData.p_Player.AlterHandCardCost(m_CostAlter);
            iEndAction.Invoke();
        }
    }
}
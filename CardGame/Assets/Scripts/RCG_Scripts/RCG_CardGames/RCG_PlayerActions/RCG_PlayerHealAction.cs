using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG
{
    public class RCG_PlayerHealAction : RCG_PlayerAction
    {
        List<RCG_Unit> m_Targets = null;
        RCG_Unit m_Healer = null;
        int m_HealAmount = 0;
        /// <summary>
        /// iHealer帶入null則不會受Buff與Debuff影響
        /// </summary>
        /// <param name="iHealer">恢復者</param>
        /// <param name="iTargets">恢復目標</param>
        /// <param name="iHealAmount">原始恢復量</param>
        public RCG_PlayerHealAction(RCG_Unit iHealer, List<RCG_Unit> iTargets, int iHealAmount)
        {
            m_Healer = iHealer;
            m_Targets = iTargets;
            m_HealAmount = iHealAmount;
        }
        public override void Trigger(Action iEndAction)
        {
            RCG_BattleField.ins.HealUnits(m_Healer, m_Targets, m_HealAmount, iEndAction);
        }
    }
}


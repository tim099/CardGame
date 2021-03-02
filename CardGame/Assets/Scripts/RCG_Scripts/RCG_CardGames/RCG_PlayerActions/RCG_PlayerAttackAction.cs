using System;
using System.Collections;
using System.Collections.Generic;
using UCL.TweenLib;
using UnityEngine;

namespace RCG
{
    public class RCG_PlayerAttackAction : RCG_PlayerAction
    {
        List<RCG_Unit> m_Targets = null;
        RCG_Unit m_Attaker = null;
        int m_Atk = 0;
        int m_AtkTimes = 1;
        /// <summary>
        /// iAttaker帶入null則不會受Buff與Debuff影響
        /// </summary>
        /// <param name="iAttaker">攻擊者</param>
        /// <param name="iTargets">攻擊目標</param>
        /// <param name="iAtk">原始傷害</param>
        public RCG_PlayerAttackAction(RCG_Unit iAttaker, List<RCG_Unit> iTargets, int iAtk, int iAtkTimes)
        {
            m_Attaker = iAttaker;
            m_Targets = iTargets;
            m_Atk = iAtk;
            m_AtkTimes = iAtkTimes;
        }
        public override void Trigger(Action iEndAction)
        {
            RCG_BattleField.ins.AttackUnits(m_Attaker, m_Targets, m_Atk, m_AtkTimes, iEndAction);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCL.TweenLib;

namespace RCG
{
    public class RCG_CardHealEffect : RCG_BattleEffect
    {
        public int m_HealAmount = 0;
        public AttackRange m_HealRange = AttackRange.Target;

        override public string Description
        {
            get
            {
                string aHealRangeDes = RCG_CardAttackEffect.GetAttackRangeDes(m_HealRange);
                string aHealStr = m_HealAmount.ToString();
                return UCL.Core.LocalizeLib.UCL_LocalizeManager.Get("Heal_Des", aHealStr, aHealRangeDes) + "\n";
            }
        }
        public override void TriggerEffect(TriggerEffectData iTriggerEffectData, Action iEndAction)
        {
            var aTargets = RCG_CardAttackEffect.GetAttackRangeTarget(iTriggerEffectData, m_HealRange);
            //Debug.LogError("aTargets:" + aTargets.Count);
            if (aTargets != null && aTargets.Count > 0)
            {
                iTriggerEffectData.p_Player.AddPlayerAction(new RCG_PlayerHealAction(iTriggerEffectData.m_PlayerUnit, aTargets, m_HealAmount));
            }

            iEndAction.Invoke();
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RCG {
    public class RCG_CardDefenseEffect : RCG_CardEffect {
        public int m_Defense = 0;
        override public string Description {
            get {
                return UCL.Core.LocalizeLib.UCL_LocalizeManager.Get("Def_Des", m_Defense) + System.Environment.NewLine;
            }
        }
        public override void TriggerEffect(TriggerEffectData iTriggerEffectData, Action iEndAction)
        {
            var aTargets = iTriggerEffectData.m_Targets.Clone();
            RCG_Player.Ins.AddPlayerAction(CreateAction.ActionTrigger(delegate ()
            {
                foreach(var aTarget in aTargets)
                {
                    aTarget.AlterArmor(m_Defense);
                }
            }));
            iEndAction.Invoke();
        }
    }
}
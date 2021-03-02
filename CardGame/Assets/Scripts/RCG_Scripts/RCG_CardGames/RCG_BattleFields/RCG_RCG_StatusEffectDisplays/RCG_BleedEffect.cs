using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RCG
{
    public class RCG_BleedEffect : RCG_StatusEffectDisplay
    {
        public override StatusType StatusType { get { return StatusType.Bleed; } }
        public override void TurnEndAction()
        {
            RCG_Player.ins.AddPlayerAction(new RCG_PlayerAttackAction(null, new List<RCG_Unit>() { p_Unit }, m_StatusLayer, 1));
        }
    }
}
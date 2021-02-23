using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RCG
{
    public class RCG_WeakEffect : RCG_StatusEffectDisplay
    {
        public override StatusType StatusType { get { return StatusType.Weak; } }
        public override float GetAtkBuff()
        {
            return -0.25f * m_StatusLayer;
        }
        public override void AlterLayer(int iAmount)
        {
            base.AlterLayer(iAmount);
            p_Unit.UpdateAtkBuff();
        }
    }
}
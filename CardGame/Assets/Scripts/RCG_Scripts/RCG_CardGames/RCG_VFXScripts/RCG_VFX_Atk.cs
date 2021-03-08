using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG
{
    public class RCG_VFX_Atk : RCG_VFX
    {
        protected RCG_Unit m_Attacker = null;
        protected List<RCG_Unit> m_Targets = null;
        virtual public void InitAtkVFX(RCG_Unit iAttacker, List<RCG_Unit> iTargets)
        {
            m_Attacker = iAttacker;
            m_Targets = iTargets;
        }
    }
}
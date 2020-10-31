using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG{
    public class RCG_Status_Bleed : RCG_Status
    {
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        override public void StatusTurnEnd(){
            m_target.DamageHP(1);
            m_round -= 1;
        }
    }
}

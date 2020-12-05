using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RCG{

    class StatusEffect{
        public StatusType m_status_type;
        public int m_count;
        public int m_multiplier;

        public StatusEffect(StatusType t=StatusType.None, int c=1, int m=0) {
            this.m_status_type = t;
            this.m_count = c;
            this.m_multiplier = m;
        }
    }
}
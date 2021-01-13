using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG {
    public class RCG_BattleManager : MonoBehaviour {
        public RCG_BattleField m_BattleField;
        virtual public void Init() {
            m_BattleField.Init();
        }
    }
}


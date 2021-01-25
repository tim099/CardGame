using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RCG {
    public class RCG_CardGameTest : MonoBehaviour {
        public RCG_CardGame m_Game;
        public bool m_AutoInit = true;
        bool m_Inited = false;
        private void Start() {
            if(m_AutoInit) Init();
        }
        public void TriggerCard() {

        }
        public void Init() {
            if(m_Inited) return;
            m_Inited = true;
            m_Game.Init();
        }

    }
}
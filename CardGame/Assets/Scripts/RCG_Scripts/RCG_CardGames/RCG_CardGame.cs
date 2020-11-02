using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG {
    public class RCG_CardGame : MonoBehaviour {
        static public RCG_CardGame ins = null; 
        public RCG_Player m_Player;
        private void Awake() {
            ins = this;
            m_Player.Init();
        }
        public void EnemyEndTurn() {

        }
        public void PlayerEndTurn() {
            m_Player.EndTurn();
        }
    }
}
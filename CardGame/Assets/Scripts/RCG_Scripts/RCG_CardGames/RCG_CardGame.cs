using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG {
    public class RCG_CardGame : MonoBehaviour {
        static public RCG_CardGame ins = null; 
        public RCG_Player m_Player = null;
        bool m_Entered = false;
        public void Init() {
            ins = this;

            m_Player.Init();
            Debug.LogWarning("Application.systemLanguage:" + Application.systemLanguage.ToString());
        }
        virtual public void EnterBattle() {
            if(m_Entered) {
                Debug.LogError("EnterBattle() Fail!! Already Entered!!");
                return;
            }
            m_Entered = true;
            gameObject.SetActive(true);
        }
        public void ExitBattle() {
            if(!m_Entered) {
                Debug.LogError("ExitBattle() Fail!! Not Entered!!");
                return;
            }
            m_Entered = false;
            gameObject.SetActive(false);
        }
        public void TriggerCard() {
            m_Player.TriggerCard();
        }
        public void EnemyEndTurn() {

        }
        public void PlayerEndTurn() {
            m_Player.EndTurn();
        }
        private void Update() {
            //Debug.LogWarning("Update()");
            if(Input.GetKeyDown(KeyCode.Escape)) {
                //Debug.LogWarning("Input.GetKeyDown(KeyCode.Escape)");
                RCG_GameManager.Instance.ExitGame();
            }
        }
    }
}
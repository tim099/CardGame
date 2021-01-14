using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG {
    public class RCG_CardGame : MonoBehaviour {
        static public RCG_CardGame ins = null; 
        public RCG_Player m_Player;
        public RCG_BattleField m_battlefield;
        public void Init() {
            ins = this;
            //UCL.Core.LocalizeLib.UCL_LocalizeManager.Instance.ResourceLoadLanguage("Language", "Chinese");
            m_Player.Init();
            m_battlefield.Init();
            Debug.LogWarning("Application.systemLanguage:" + Application.systemLanguage.ToString());
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
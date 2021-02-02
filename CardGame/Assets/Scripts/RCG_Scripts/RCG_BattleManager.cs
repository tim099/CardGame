using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG {
    public class RCG_BattleManager : MonoBehaviour {
        static public RCG_BattleManager ins = null;
        public RCG_BattleField m_BattleField = null;
        public RCG_CardGame m_CardGame = null;
        bool m_Entered = false;
        virtual public void Init() {
            ins = this;
            m_BattleField.Init();
            m_CardGame.Init();
            gameObject.SetActive(false);
        }
        public void EnterBattle() {
            if(m_Entered) {
                Debug.LogError("EnterBattle() Fail!! Already Entered!!");
                return;
            }
            m_Entered = true;
            gameObject.SetActive(true);
            m_BattleField.EnterBattle();
            m_CardGame.EnterBattle();
            TurnInit();
        }
        public void TurnInit()
        {
            m_BattleField.TurnInit();
            m_CardGame.TurnInit();
        }
        /// <summary>
        /// 玩家行動結束
        /// </summary>
        public void PlayerTurnEnd()
        {
            m_BattleField.TurnStart();
        }
        /// <summary>
        /// 敵人行動結束
        /// </summary>
        public void EnemyTurnEnd()
        {
            TurnInit();
        }
        private void OnGUI() {
            if(m_Entered) {
                if(GUILayout.Button("ExitBattle()")) {
                    ExitBattle();
                }
            }

        }
        public void ExitBattle() {
            if(!m_Entered) {
                Debug.LogError("ExitBattle() Fail!! Not Entered!!");
                return;
            }
            m_Entered = false;
            m_BattleField.ExitBattle();
            m_CardGame.ExitBattle();

            gameObject.SetActive(false);
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RCG {
    public class RCG_BattleManager : MonoBehaviour {
        public enum BattleState
        {
            None = 0,
            TrunInit,//回合開始 初始化階段
            PlayerAction,//玩家行動
            EnemyAction,//敵方行動
            TurnEnd,//回合結束
        }
        static public RCG_BattleManager ins = null;
        public RCG_BattleField m_BattleField = null;
        public RCG_CardGame m_CardGame = null;
        public RCG_VFXManager m_VFXManager = null;
        public Button m_BackgroundButton;
        bool m_Entered = false;
        BattleState m_State = BattleState.None;
        virtual public void Init() {
            ins = this;
            m_BattleField.Init();
            m_CardGame.Init();
            m_VFXManager.Init();
            m_BackgroundButton.onClick.AddListener(BackgroundClick);
            gameObject.SetActive(false);
        }
        /// <summary>
        /// 背景按鈕被按下
        /// </summary>
        virtual public void BackgroundClick()
        {
            m_CardGame.BackgroundClick();
        }
        public void SetState(BattleState iState)
        {
            m_State = iState;
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
            SetState(BattleState.TrunInit);
            Debug.LogWarning("RCG_BattleManager TurnInit()");
            m_BattleField.TurnInit();
            m_CardGame.TurnInit();
        }
        /// <summary>
        /// 玩家行動結束
        /// </summary>
        public void PlayerTurnEnd()
        {
            SetState(BattleState.EnemyAction);
            m_BattleField.TurnStart();
        }
        /// <summary>
        /// 敵人行動結束
        /// </summary>
        public void EnemyTurnEnd()
        {
            TurnEnd();
        }
        public void TurnEnd()
        {
            SetState(BattleState.TurnEnd);
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


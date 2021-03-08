using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG {
    public class RCG_CardGame : MonoBehaviour {
        static public RCG_CardGame ins = null; 
        public RCG_Player m_Player = null;
        [SerializeField] protected RCG_ItemUI m_ItemUI = null;
        bool m_Entered = false;
        public void Init() {
            ins = this;

            m_Player.Init();
            m_ItemUI.Init();
            Debug.LogWarning("Application.systemLanguage:" + Application.systemLanguage.ToString());
        }
        /// <summary>
        /// 回合開始初始化
        /// </summary>
        public void TurnInit()
        {
            m_Player.TurnInit();
        }
        virtual public void EnterBattle() {
            if(m_Entered) {
                Debug.LogError("EnterBattle() Fail!! Already Entered!!");
                return;
            }
            m_Entered = true;
            gameObject.SetActive(true);
            var aCards = RCG_DataService.ins.m_DeckData.GetCardDatas();
            //UCL.Core.MathLib.UCL_Random.Instance.Shuffle(aCards);
            m_Player.InitDeckDatas(aCards);
        }
        public void ExitBattle() {
            if(!m_Entered) {
                Debug.LogError("ExitBattle() Fail!! Not Entered!!");
                return;
            }
            m_Entered = false;
            gameObject.SetActive(false);
        }
        /// <summary>
        /// 背景按鈕被按下
        /// </summary>
        virtual public void BackgroundClick()
        {
            m_Player.BackgroundClick();
        }
        public void EnemyEndTurn() {

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
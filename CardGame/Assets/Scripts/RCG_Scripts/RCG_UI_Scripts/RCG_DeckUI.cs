using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RCG
{
    public class RCG_DeckUI : MonoBehaviour
    {
        
        public Text m_CardNumText;
        public RCG_ShowCardUI m_ShowCardUI;

        System.Func<List<RCG_CardData>> m_GetCardsFunc = null;
        List<RCG_CardData> m_Cards;
        RCG_Player p_Player;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="iPlayer"></param>
        /// <param name="iGetCardsFunc">用來抓取要顯示的牌</param>
        public void Init(RCG_Player iPlayer, System.Func<List<RCG_CardData>> iGetCardsFunc) {
            p_Player = iPlayer;
            m_GetCardsFunc = iGetCardsFunc;

            m_ShowCardUI.Init();
            m_ShowCardUI.gameObject.SetActive(false);
        }

        public void SetCardNum(int num) {
            m_CardNumText.SetText("" + num);
        }
        public void Toggle() {
            if(m_ShowCardUI.gameObject.activeSelf) {
                Hide();
            } else {
                Show();
            }
        }
        public void Show() {
            List<RCG_CardData> cards = null;
            cards = m_GetCardsFunc.Invoke();
             
            m_Cards = cards;
            m_ShowCardUI.Show(m_Cards);
        }
        public void Hide() {
            m_ShowCardUI.Hide();
            //gameObject.SetActive(false);
        }
    }
}
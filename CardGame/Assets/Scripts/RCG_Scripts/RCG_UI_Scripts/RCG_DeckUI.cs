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
        /// <summary>
        /// if m_ShowCards == false then show UsedCard
        /// </summary>
        public bool m_ShowCards = true;
        List<RCG_CardData> m_Cards;
        RCG_Player p_Player;
        public void Init(RCG_Player _Player) {
            p_Player = _Player;
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
            if(m_ShowCards) {
                cards = p_Player.m_Deck.ShowCards();
            } else {
                cards = p_Player.m_Deck.ShowUsedCards();
            }
             
            m_Cards = cards;
            m_ShowCardUI.Show(m_Cards);
        }
        public void Hide() {
            m_ShowCardUI.Hide();
            //gameObject.SetActive(false);
        }
    }
}
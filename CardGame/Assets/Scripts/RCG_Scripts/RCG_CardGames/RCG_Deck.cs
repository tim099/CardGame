using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RCG {
    public class RCG_Deck {
        public List<RCG_CardData> m_Cards = new List<RCG_CardData>();
        public List<RCG_CardData> m_UsedCards = new List<RCG_CardData>();
        public void Add(RCG_CardData card) {
            m_Cards.Add(card);
        }
        public void Used(RCG_CardData card) {
            m_UsedCards.Add(card);
        }
        public void Shuffle() {
            UCL.Core.MathLib.UCL_Random.Instance.Shuffle(ref m_Cards);
        }
        public RCG_CardData Draw() {
            if(m_Cards.Count == 0) {
                if(m_UsedCards.Count == 0) return null;
                UCL.Core.GameObjectLib.Swap(ref m_Cards, ref m_UsedCards);
                Shuffle();
            }
            var card = m_Cards[0];
            m_Cards.RemoveAt(0);
            return card;
        }
    }
}
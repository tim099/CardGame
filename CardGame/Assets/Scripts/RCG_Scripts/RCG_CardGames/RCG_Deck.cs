using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RCG {
    public class RCG_Deck : MonoBehaviour {
        public int HandCardCount { get { return m_HandCardCount; } }
        public List<RCG_CardData> Cards { get { return m_Cards; } }
        public List<RCG_CardData> UsedCards { get { return m_UsedCards; } }
        public UCL.Core.UCL_Event m_OnDeckEmptyEvent = new UCL.Core.UCL_Event();


        public RCG_DeckUI m_CardUI;
        public RCG_DeckUI m_UsedCardUI;

        protected List<RCG_CardData> m_Cards = new List<RCG_CardData>();
        protected List<RCG_CardData> m_UsedCards = new List<RCG_CardData>();
        protected int m_HandCardCount = 0;
        RCG_Player p_Player;
        virtual public void Init(RCG_Player player) {
            p_Player = player;
            m_CardUI.Init(p_Player, ShowCards);
            m_UsedCardUI.Init(p_Player, ShowUsedCards);
        }
        virtual public void PlayerUpdate() {
            m_CardUI.SetCardNum(Cards.Count);
            m_UsedCardUI.SetCardNum(UsedCards.Count);
        }
        public void LogDatas() {
            Debug.LogWarning("m_Cards:" + m_Cards.Count);
            Debug.LogWarning("m_UsedCards:" + m_UsedCards.Count);
        }
        public List<RCG_CardData> ShowCards() {
            var list = m_Cards.Clone();
            UCL.Core.MathLib.UCL_Random.Instance.Shuffle(ref list);
            return list;
        }
        public List<RCG_CardData> ShowUsedCards() {
            var list = m_UsedCards.Clone();
            return list;
        }
        public void Add(RCG_CardData card) {
            m_Cards.Add(card);
        }
        /// <summary>
        /// 回收使用完的卡牌
        /// </summary>
        /// <param name="card"></param>
        public void Used(RCG_CardData card) {
            if(card == null) {
                Debug.LogError("Used card fail card == null");
                return;
            }
            --m_HandCardCount;
            Debug.LogWarning("m_HandCardCount:" + m_HandCardCount);
            m_UsedCards.Add(card);
        }
        public void Shuffle() {
            UCL.Core.MathLib.UCL_Random.Instance.Shuffle(ref m_Cards);
        }
        public void OnDeckEmpty() {
            UCL.Core.GameObjectLib.Swap(ref m_Cards, ref m_UsedCards);
            Shuffle();
            m_OnDeckEmptyEvent.UCL_Invoke();
        }
        public RCG_CardData Draw() {
            if(m_Cards.Count == 0) {
                if(m_UsedCards.Count == 0) return null;
                OnDeckEmpty();
            }
            var card = m_Cards[0];
            m_Cards.RemoveAt(0);
            ++m_HandCardCount;
            //Debug.LogWarning("m_HandCardCount:" + m_HandCardCount);
            return card;
        }
    }
}
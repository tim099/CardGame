using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RCG {
    public class RCG_Deck : MonoBehaviour {
        /// <summary>
        /// 牌庫
        /// </summary>
        public List<RCG_CardData> Cards { get { return m_Cards; } }
        /// <summary>
        /// 棄牌堆
        /// </summary>
        public List<RCG_CardData> DiscardPile { get { return m_DiscardPile; } }
        public UCL.Core.UCL_Event m_OnDeckEmptyEvent = new UCL.Core.UCL_Event();


        public RCG_DeckUI m_CardUI;
        public RCG_DeckUI m_UsedCardUI;

        protected List<RCG_CardData> m_Cards = new List<RCG_CardData>();
        protected List<RCG_CardData> m_DiscardPile = new List<RCG_CardData>();
        RCG_Player p_Player;
        virtual public void Init(RCG_Player player) {
            p_Player = player;
            m_CardUI.Init(p_Player, ShowCards);
            m_UsedCardUI.Init(p_Player, ShowUsedCards);
        }
        virtual public void PlayerUpdate() {
            m_CardUI.SetCardNum(Cards.Count);
            m_UsedCardUI.SetCardNum(DiscardPile.Count);
        }
        public void LogDatas() {
            Debug.LogWarning("m_Cards:" + m_Cards.Count);
            Debug.LogWarning("m_UsedCards:" + m_DiscardPile.Count);
        }
        /// <summary>
        /// 清空牌庫
        /// </summary>
        public void ClearCards()
        {
            m_Cards.Clear();
        }
        public List<RCG_CardData> ShowCards() {
            var list = m_Cards.Clone();
            UCL.Core.MathLib.UCL_Random.Instance.Shuffle(list);
            return list;
        }
        public List<RCG_CardData> ShowUsedCards() {
            var list = m_DiscardPile.Clone();
            return list;
        }
        public void Add(RCG_CardData card) {
            m_Cards.Add(card);
        }
        /// <summary>
        /// 回收使用完的卡牌
        /// </summary>
        /// <param name="iCard"></param>
        public void AddToDiscardPile(RCG_CardData iCard) {
            if(iCard == null) {
                Debug.LogError("AddToDiscardPile fail card == null");
                return;
            }
            //Debug.LogWarning("m_HandCardCount:" + m_HandCardCount);
            m_DiscardPile.Add(iCard);
        }
        /// <summary>
        /// 將卡牌放置到牌堆頂端
        /// </summary>
        /// <param name="iCard"></param>
        public void AddToDeckTop(RCG_CardData iCard)
        {
            if (iCard == null)
            {
                Debug.LogError("AddToDeckTop fail card == null");
                return;
            }
            m_Cards.Insert(0, iCard);
        }
        public void Shuffle() {
            UCL.Core.MathLib.UCL_Random.Instance.Shuffle(m_Cards);
        }
        public void OnDeckEmpty() {
            UCL.Core.GameObjectLib.Swap(ref m_Cards, ref m_DiscardPile);
            Shuffle();
            m_OnDeckEmptyEvent.UCL_Invoke();
        }
        /// <summary>
        /// 從牌堆頂抽一張牌
        /// </summary>
        /// <returns></returns>
        public RCG_CardData Draw() {
            if(m_Cards.Count == 0) {
                if(m_DiscardPile.Count == 0) return null;
                //牌庫清空 進行洗牌補牌
                OnDeckEmpty();
            }
            var card = m_Cards[0];
            m_Cards.RemoveAt(0);
            return card;
        }
    }
}
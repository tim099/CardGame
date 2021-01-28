using System.Collections;
using System.Collections.Generic;
using UCL.Core.UI;
using UCL.TweenLib;
using UnityEngine;
using UnityEngine.UI;
namespace RCG {
    [UCL.Core.ATTR.EnableUCLEditor]
    public class RCG_Player : MonoBehaviour {
        /// <summary>
        /// 手牌剩餘空間
        /// </summary>
        public int CardSpace {
            get {
                if(IsUsingCard) return m_CardPosController.m_MaxCardCount - m_Deck.HandCardCount + 1;
                return m_CardPosController.m_MaxCardCount - m_Deck.HandCardCount;
            }
        }
        public bool IsUsingCard { get; set; } = false;
        public int Cost { get { return m_Cost; } }
        public const int TurnInitCost = 3;//每回合初始費用
        public const int TurnInitCardNum = 5;//每回合初始手牌
        public Text m_CostText = null;
        public RCG_Deck m_Deck = null;
        public RCG_Card m_CardTemplate = null;
        public List<RCG_Card> m_Cards = null;
        public List<RCG_CardBeginSet> m_BeginSets = null;
        public List<UCL_RectTransformCollider> m_Targets = new List<UCL_RectTransformCollider>();
        
        public int m_DrawCardCount = 0;
        public Transform m_DrawCardPos = null;
        public Transform m_CardsRoot = null;
        public RCG_CardPosController m_CardPosController = null;
        protected RCG_Card m_SelectedCard = null;
        protected UCL_RectTransformCollider m_Target = null;
        protected bool m_Blocking = false;
        protected bool m_Inited = false;
        protected int m_Cost = 0;
        [UCL.Core.ATTR.UCL_FunctionButton]
        public void LogDeck() {
            m_Deck.LogDatas();
        }

        [UCL.Core.ATTR.UCL_FunctionButton]
        public void Init() {
            if(m_Inited) return;
            m_Inited = true;
            m_CardPosController.Init();
            if(m_Cards == null) {
                m_Cards = new List<RCG_Card>();
            }
            //UCL.Core.GameObjectLib.SearchChildNotIncludeParent(m_CardsRoot, m_CardPositions);
            for(int i = 0; i < m_CardPosController.m_MaxCardCount; i++) {
                var aCard = Instantiate(m_CardTemplate, m_CardsRoot);
                aCard.Init(this);
                m_CardPosController.AddCard(aCard);
                m_Cards.Add(aCard);
            }
            
            if(m_BeginSets.Count == 0) {
                return;
            }

            var set = m_BeginSets[UCL.Core.MathLib.UCL_Random.Instance.Next(m_BeginSets.Count)];
            m_Deck.Init(this);
            for(int i = 0, len = set.m_Settings.Count; i < len; i++) {
                m_Deck.Add(set.m_Settings[i].CreateCard());
            }
            m_Deck.Shuffle();

            TurnInit();
        }
        /// <summary>
        /// 測試用 強制使用卡牌
        /// </summary>
        public void TriggerCard() {
            if(m_Blocking) return;
            if(m_SelectedCard == null) return;
            m_SelectedCard.Deselect();
            m_SelectedCard.TriggerCardEffect(0);
            m_Deck.Used(m_SelectedCard.Data);
            m_SelectedCard.CardUsed();
        }
        /// <summary>
        /// 設定選中的手牌
        /// </summary>
        /// <param name="iCard"></param>
        public void SetSelectedCard(RCG_Card iCard) {
            if(m_SelectedCard == iCard) {
                Debug.LogWarning("m_SelectedCard == iCard");
                return;
            }
            if(m_SelectedCard != null) {
                m_SelectedCard.Deselect();
            }
            m_SelectedCard = iCard;
            if(m_SelectedCard == null) return;
            m_SelectedCard.Select();
        }
        public void DrawCard(int count) {
            m_DrawCardCount += count;
            int space = CardSpace;
            if(m_DrawCardCount > space) m_DrawCardCount = space;
        }
        public void EndTurn() {
            SetSelectedCard(null);
        }
        public bool AlterCost(int iAlter) {
            if(m_Cost + iAlter < 0) return false;
            SetCost(m_Cost + iAlter);
            return true;
        }
        public void SetCost(int iCost) {
            m_Cost = iCost;
            m_CardPosController.UpdateCardStatus();
        }
        /// <summary>
        /// 清除所有手牌(移除到棄牌堆)
        /// </summary>
        public void ClearAllHandCard() {
            foreach(var aCard in m_Cards) {
                if(!aCard.m_Used && aCard.Data != null) {
                    m_Deck.Used(aCard.Data);
                    aCard.SetCardData(null);
                }
            }
        }
        public void TurnInit() {
            if(m_Blocking) return;
            SetSelectedCard(null);
            m_DrawCardCount = 0;
            ClearAllHandCard();

            for(int i = 0; i < m_Cards.Count; i++) {
                var card = m_Cards[i];
                RCG_CardData data = null;
                if(i < TurnInitCardNum) data = m_Deck.Draw();
                card.TurnInit(data);
                if(data != null) card.DrawCardAnime(m_DrawCardPos.position, null);
            }
            SetCost(TurnInitCost);
        }
        /// <summary>
        /// 點擊卡牌後放開觸發
        /// </summary>
        public void CardRelease() {
            if(m_Blocking) return;
        }
        private void Update() {
            if(!m_Inited) return;
            m_Target = null;
            m_CostText.SetText("" + Cost);

            m_Deck.PlayerUpdate();
            if(!m_Blocking) {
                if(m_DrawCardCount > 0) {
                    var card = m_CardPosController.GetAvaliableCard();
                    if(card != null && card.IsEmpty) {
                        m_Blocking = true;
                        card.SetCardData(m_Deck.Draw());
                        card.DrawCardAnime(m_DrawCardPos.position, () => { m_Blocking = false; });
                        m_DrawCardCount--;
                    }
                }
            }
        }
    }
}
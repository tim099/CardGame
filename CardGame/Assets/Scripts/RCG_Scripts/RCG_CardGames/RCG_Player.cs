using System.Collections;
using System.Collections.Generic;
using UCL.Core.UI;
using UCL.TweenLib;
using UnityEngine;
using UnityEngine.UI;
namespace RCG {
    [UCL.Core.ATTR.EnableUCLEditor]
    public class RCG_Player : MonoBehaviour {
        public const int MaxCost = 3;
        public Text m_CostText = null;
        public RCG_Deck m_Deck = null;
        public RCG_Card m_CardTemplate = null;
        public List<RCG_Card> m_Cards = null;
        public List<RCG_CardBeginSet> m_BeginSets = null;
        public List<UCL_RectTransformCollider> m_Targets = new List<UCL_RectTransformCollider>();
        public int m_Cost = 0;
        public int m_DrawCardCount = 0;
        public Transform m_DrawCardPos = null;
        public Transform m_CardsRoot = null;
        protected List<Transform> m_CardPositions = new List<Transform>();
        protected RCG_Card m_DraggingCard = null;
        protected UCL_RectTransformCollider m_Target = null;
        protected bool m_Blocking = false;
        protected bool m_Inited = false;
        public int CardSpace {
            get {
                int count = 0;
                for(int i = 0; i < m_Cards.Count; i++) {
                    var card = m_Cards[i];
                    if(card.IsEmpty) count++;
                }
                return count;
            }
        }

        [UCL.Core.ATTR.UCL_FunctionButton]
        public void LogDeck() {
            m_Deck.LogDatas();
        }

        [UCL.Core.ATTR.UCL_FunctionButton]
        public void Init() {
            if(m_Inited) return;
            m_Inited = true;
            if(m_Cards == null) {
                m_Cards = new List<RCG_Card>();
            }
            UCL.Core.GameObjectLib.SearchChildNotIncludeParent(m_CardsRoot, m_CardPositions);
            for(int i = 0; i < 7; i++) {
                var card = Instantiate(m_CardTemplate, m_CardsRoot);
                card.transform.position = m_CardPositions[i].position;
                m_Cards.Add(card);
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
            for(int i = 0; i < m_Cards.Count; i++) {
                var card = m_Cards[i];
                card.Init(this);
            }
            TurnInit();
        }
        public void DrawCard(int count) {
            m_DrawCardCount += count;
            int space = CardSpace;
            if(m_DrawCardCount > space) m_DrawCardCount = space;
        }
        public void EndTurn() {

        }
        public void TurnInit() {
            if(m_Blocking) return;
            m_DrawCardCount = 0;
            foreach(var card in m_Cards) {
                if(!card.m_Used && card.Data != null) {
                    //Debug.LogWarning("Use:" + card.name);
                    //card.Data.LogSetting();
                    m_Deck.Used(card.Data);
                }
            }

            for(int i = 0; i < m_Cards.Count; i++) {
                var card = m_Cards[i];
                RCG_CardData data = null;
                if(i > 0 && i < m_Cards.Count - 1) data = m_Deck.Draw();
                card.TurnInit(data);
                if(data != null) card.DrawCardAnime(m_DrawCardPos.position, null);
            }
            m_Cost = MaxCost;
        }
        public void CardRelease() {
            if(m_Blocking) return;
            if(m_DraggingCard != null && m_Target != null) {
                if(m_DraggingCard.TriggerCardEffect(m_Target.m_ID)) {
                    Debug.LogWarning("D Use:" + m_DraggingCard.name);
                    var card = m_DraggingCard;
                    m_Deck.Used(m_DraggingCard.Data);
                    UCL.TweenLib.UCL_TweenManager.Instance.KillAllOnTransform(m_DraggingCard.m_Button.transform);
                    //Debug.LogError("Hit!!:" + m_DraggingCard.name + ",m_Target:" + m_Target.name);
                    m_Blocking = true;
                    var seq = m_DraggingCard.m_Button.transform.UCL_Move(0.15f, m_Target.transform.position).SetEase(EaseType.OutQuad);
                    seq.OnComplete(() => {
                        m_Blocking = false;
                        card.CardUsed();
                    });
                    seq.Start();
                }
            }
        }
        private void Update() {
            if(!m_Inited) return;
            m_DraggingCard = null;
            m_Target = null;
            foreach(var card in m_Cards) {
                if(card.IsDragging) {
                    m_DraggingCard = card;
                }
            }
            m_CostText.SetText("" + m_Cost);

            m_Deck.PlayerUpdate();
            if(!m_Blocking) {
                if(m_DrawCardCount > 0) {
                    
                    for(int i = 0; i < m_Cards.Count; i++) {
                        var card = m_Cards[i];
                        if(card.IsEmpty) {
                            m_Blocking = true;
                            card.SetCardData(m_Deck.Draw());
                            card.DrawCardAnime(m_DrawCardPos.position, () => { m_Blocking = false; });
                            m_DrawCardCount--;
                            break;
                        }
                    }
                }
            }
        }
    }
}
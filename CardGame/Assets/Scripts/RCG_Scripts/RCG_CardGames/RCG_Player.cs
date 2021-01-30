using System.Collections;
using System.Collections.Generic;
using UCL.Core.UI;
using UCL.TweenLib;
using UnityEngine;
using UnityEngine.UI;
namespace RCG {
    [UCL.Core.ATTR.EnableUCLEditor]
    public class RCG_Player : MonoBehaviour {
        public enum PlayerState {
            Idle = 0,
            DrawCard,//開始抽牌
            DrawingCard,//抽牌中
            TriggerCard,//出牌中
        }
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
        
        public int m_DrawCardCount = 0;
        public Transform m_DrawCardPos = null;//抽牌位置
        public Transform m_TriggerCardPos = null;//出牌目標位置
        public Transform m_CardsRoot = null;
        public RCG_CardPosController m_CardPosController = null;
        protected PlayerState m_PlayerState = PlayerState.Idle;
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
            m_TriggerCardPos.gameObject.SetActive(false);
            m_CardPosController.Init();
            if(m_Cards == null) {
                m_Cards = new List<RCG_Card>();
            }
            //UCL.Core.GameObjectLib.SearchChildNotIncludeParent(m_CardsRoot, m_CardPositions);
            for(int i = 0; i < m_CardPosController.m_MaxCardCount; i++) {
                var aCard = Instantiate(m_CardTemplate, m_CardsRoot);
                aCard.name = m_CardTemplate.name + "_" + i;
                aCard.Init(this);
                m_CardPosController.AddCard(aCard);
                m_Cards.Add(aCard);
            }
            
            if(m_BeginSets.Count == 0) {
                return;
            }
            m_Deck.Init(this);
            int aCardCount = RCG_CardDataService.ins.CardCount;
            for(int i = 0; i < 16; i++) {
                m_Deck.Add(RCG_CardDataService.ins.GetCardData(UCL.Core.MathLib.UCL_Random.Instance.Next(aCardCount)));
            }
            //var set = m_BeginSets[UCL.Core.MathLib.UCL_Random.Instance.Next(m_BeginSets.Count)];            
            //for(int i = 0, len = set.m_Settings.Count; i < len; i++) {
            //    m_Deck.Add(set.m_Settings[i].CreateCard());
            //}
            m_Deck.Shuffle();

            TurnInit();
        }
        /// <summary>
        /// 測試用 強制使用卡牌
        /// </summary>
        public void TriggerCard() {
            if(m_Blocking) return;
            if(m_SelectedCard == null) return;
            //Debug.LogError("TriggerCard()");
            SetState(PlayerState.TriggerCard);
            m_TriggerCardPos.gameObject.SetActive(true);
            m_SelectedCard.TriggerCardAnime(m_TriggerCardPos, delegate () {
                if(m_SelectedCard == null) {
                    Debug.LogError("TriggerCard()m_SelectedCard == null");
                    return;
                }
                m_SelectedCard.Deselect();
                m_SelectedCard.TriggerCardEffect(new TriggerEffectData(this));
                m_Deck.Used(m_SelectedCard.Data);
                m_SelectedCard.CardUsed();
                SetState(PlayerState.Idle);
            });

        }
        /// <summary>
        /// 設定選中的手牌
        /// </summary>
        /// <param name="iCard"></param>
        public void SetSelectedCard(RCG_Card iCard) {
            if(m_Blocking || m_PlayerState!= PlayerState.Idle) {
                Debug.LogWarning("SetSelectedCard Fail, Blocking!!");
                return;
            }
            if(m_SelectedCard == iCard) {
                Debug.LogWarning("m_SelectedCard == iCard");
                if(iCard != null && !iCard.IsSelected) {
                    iCard.Select();
                }
                return;
            }
            if(m_SelectedCard != null) {
                m_SelectedCard.Deselect();
            }
            m_SelectedCard = iCard;
            if(m_SelectedCard == null) return;
            m_SelectedCard.Select();
        }
        public void SetState(PlayerState iPlayerState) {
            m_PlayerState = iPlayerState;
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
        public void StartBlocking() {
            if(m_Blocking) {
                Debug.LogError("StartBlocking() Fail!!Already Blocking!!");
                return;
            }
            m_Blocking = true;
            foreach(var aCard in m_Cards) {
                aCard.BlockSelection(RCG_Card.BlockingStatus.Player);
            }
        }
        public void EndBlocking() {
            if(!m_Blocking) {
                Debug.LogError("EndBlocking() Fail!!Not yet Blocking!!");
                return;
            }
            m_Blocking = false;
            foreach(var aCard in m_Cards) {
                aCard.UnBlockSelection(RCG_Card.BlockingStatus.Player);
            }
        }
        private void DrawCardUpdate() {
            if(m_DrawCardCount > 0) {
                var card = m_CardPosController.GetAvaliableCard();
                if(card != null && card.IsEmpty) {
                    StartBlocking();
                    SetState(PlayerState.DrawingCard);
                    card.SetCardData(m_Deck.Draw());
                    --m_DrawCardCount;
                    card.DrawCardAnime(m_DrawCardPos.position, delegate () {
                        if(m_DrawCardCount == 0) {
                            SetState(PlayerState.Idle);
                            EndBlocking();
                        } else {
                            SetState(PlayerState.DrawCard);
                        }
                    });
                }
            } else {
                SetState(PlayerState.Idle);
            }
        }
        private void Update() {
            if(!m_Inited) return;
            m_Target = null;
            m_CostText.SetText("" + Cost);
            switch(m_PlayerState) {
                case PlayerState.Idle: {
                        if(m_DrawCardCount > 0) {
                            SetState(PlayerState.DrawCard);
                        }
                        break;
                    }
                case PlayerState.DrawCard: {
                        DrawCardUpdate();
                        break;
                    }
                case PlayerState.DrawingCard: {
                        break;
                    }
            }
            m_Deck.PlayerUpdate();

        }
    }
}
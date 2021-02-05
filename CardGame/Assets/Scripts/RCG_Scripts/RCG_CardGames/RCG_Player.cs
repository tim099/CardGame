using System.Collections;
using System.Collections.Generic;
using UCL.Core.UI;
using UCL.TweenLib;
using UnityEngine;
using UnityEngine.UI;
namespace RCG {
    [UCL.Core.ATTR.EnableUCLEditor]
    public class RCG_Player : MonoBehaviour {
        public static RCG_Player ins = null;
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
        public int Cost { get { return m_CostUI.Cost; } }
        public const int TurnInitCost = 3;//每回合初始費用
        public const int TurnInitCardNum = 5;//每回合初始手牌
        public RCG_Deck m_Deck = null;
        public RCG_Card m_CardTemplate = null;
        public List<RCG_Card> m_Cards = null;
        public List<RCG_CardBeginSet> m_BeginSets = null;
        
        public int m_DrawCardCount = 0;
        public Transform m_DrawCardPos = null;//抽牌位置
        public Transform m_TriggerCardPos = null;//出牌目標位置
        public Transform m_CardsRoot = null;
        public RCG_CardPosController m_CardPosController = null;
        [SerializeField] protected RCG_CostUI m_CostUI = null;
        protected PlayerState m_PlayerState = PlayerState.Idle;
        protected RCG_Card m_SelectedCard = null;
        protected UCL_RectTransformCollider m_Target = null;
        protected bool m_Blocking = false;
        protected bool m_Inited = false;
        [UCL.Core.ATTR.UCL_FunctionButton]
        public void LogDeck() {
            m_Deck.LogDatas();
        }

        [UCL.Core.ATTR.UCL_FunctionButton]
        public void Init() {
            if(m_Inited) return;
            m_Inited = true;
            ins = this;
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
            for(int i = 0; i < 22; i++) {
                m_Deck.Add(RCG_CardDataService.ins.GetCardData(UCL.Core.MathLib.UCL_Random.Instance.Next(aCardCount)));
            }
            //var set = m_BeginSets[UCL.Core.MathLib.UCL_Random.Instance.Next(m_BeginSets.Count)];            
            //for(int i = 0, len = set.m_Settings.Count; i < len; i++) {
            //    m_Deck.Add(set.m_Settings[i].CreateCard());
            //}
            m_Deck.Shuffle();
        }
        /// <summary>
        /// 使用手牌
        /// </summary>
        /// <param name="iSelectUnits"></param>
        public void TriggerCard(List<RCG_Unit> iSelectUnits)
        {
            if (m_Blocking) return;
            if (m_SelectedCard == null) return;
            //Debug.LogError("TriggerCard()");
            RCG_BattleField.ins.SetSelectMode(TargetType.Off);
            SetState(PlayerState.TriggerCard);
            m_TriggerCardPos.gameObject.SetActive(true);
            m_SelectedCard.TriggerCardAnime(m_TriggerCardPos, //打出卡牌演出
            delegate () {
                if (m_SelectedCard == null)
                {
                    Debug.LogError("TriggerCard()m_SelectedCard == null");
                    return;
                }
                m_SelectedCard.Deselect();
                var aData = new TriggerEffectData(this);
                aData.m_Targets = iSelectUnits;
                aData.m_PlayerUnit = RCG_BattleField.ins.ActiveUnit;
                IsUsingCard = true;
                UpdateCardStatus();
                m_SelectedCard.TriggerCardEffect(aData,
                delegate (bool iTriggerSuccess) {//卡牌效果觸發完畢
                    if (!iTriggerSuccess)
                    {
                        Debug.LogError("!iTriggerSuccess");
                    }
                    m_Deck.Used(m_SelectedCard.Data);
                    IsUsingCard = false;
                    m_SelectedCard.CardUsed();

                    UpdateCardStatus();
                    SetState(PlayerState.Idle);
                });
            });
        }

        /// <summary>
        /// 選中的卡牌觸發目標
        /// </summary>
        /// <param name="iTargets"></param>
        public void SelectTargets(List<RCG_Unit> iTargets)
        {
            TriggerCard(iTargets.Clone());
        }
        /// <summary>
        /// 背景按鈕被按下
        /// </summary>
        virtual public void BackgroundClick()
        {
            if (m_SelectedCard != null) ClearSelectedCard();
        }
        /// <summary>
        /// 設定選中的手牌
        /// </summary>
        /// <param name="iCard"></param>
        public void SetSelectedCard(RCG_Card iCard) {
            if(m_Blocking || m_PlayerState != PlayerState.Idle) {
                Debug.LogWarning("SetSelectedCard Fail, Blocking!!");
                return;
            }
            if(m_SelectedCard == iCard) {
                Debug.LogWarning("m_SelectedCard == iCard");
                if(iCard != null) {
                    ClearSelectedCard();
                }
                return;
            }
            if(m_SelectedCard != null) {
                ClearSelectedCard();
            }
            m_SelectedCard = iCard;
            if (m_SelectedCard == null) {
                RCG_BattleField.ins.SetSelectMode(TargetType.Close);
                return;
            }
            m_SelectedCard.Select();
            if (m_SelectedCard.Data != null)
            {
                RCG_BattleField.ins.SetSelectMode(m_SelectedCard.Data.TargetType);
            }
            else
            {
                Debug.LogError("m_SelectedCard.Data == null!!");
                RCG_BattleField.ins.SetSelectMode(TargetType.Close);
            }
            
        }
        public void ClearSelectedCard()
        {
            if (IsUsingCard) return;
            //Debug.LogError("ClearSelectedCard()");
            if (m_SelectedCard != null)
            {
                m_SelectedCard.Deselect();
            }
            m_SelectedCard = null;
            RCG_BattleField.ins.SetSelectMode(TargetType.Close);
        }
        public void SetState(PlayerState iPlayerState) {
            m_PlayerState = iPlayerState;
        }
        public void DrawCard(int count) {
            m_DrawCardCount += count;
            int space = CardSpace;
            if(m_DrawCardCount > space) m_DrawCardCount = space;
        }
        public bool AlterCost(int iAlter) {
            if(!m_CostUI.AlterCost(iAlter))return false;
            m_CardPosController.UpdateCardStatus();
            return true;
        }
        public void SetCost(int iCost) {
            m_CostUI.SetCost(iCost);
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
        /// <summary>
        /// 玩家行動結束
        /// </summary>
        public void TurnEnd()
        {
            ClearSelectedCard();
            RCG_BattleManager.ins.PlayerTurnEnd();
        }
        public void TurnInit() {
            if(m_Blocking) return;
            ClearSelectedCard();
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
        /// 更新卡牌資訊 包含判斷玩家費用是否足夠 技能是否符合等...
        /// </summary>
        virtual public void UpdateCardStatus()
        {
            m_CardPosController.UpdateCardStatus();
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
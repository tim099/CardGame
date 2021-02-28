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
        /// <summary>
        /// 選牌模式
        /// </summary>
        public enum SelectCardMode
        {
            /// <summary>
            /// 鎖定出牌狀態 不能選擇卡牌
            /// </summary>
            Block = 0,
            /// <summary>
            /// 玩家行動中 會檢查選定腳色技能 Cost
            /// </summary>
            Normal,
            /// <summary>
            /// 不檢查技能 Cost, 例如棄牌時
            /// </summary>
            DontCheck,
        }
        public enum PlayerState {
            Idle = 0,
            TriggerCardInit,//出牌初始狀態
            TriggerCardPostTriggerAction,//出牌前置檢測
            TriggeringCardPostTriggerAction,//出牌前置檢測中
            TriggerCardAction,//觸發卡牌行動
            TriggeringCardAction,//觸發卡牌行動中
            TriggerCardActionEnd,//觸發卡牌行動結束
            PlayerActionStart,//玩家行動開始(卡牌觸發後的效果)
            PlayerAction,//玩家行動(受卡牌效果觸發 包含抽牌等)
        }
        /// <summary>
        /// 手牌剩餘空間
        /// </summary>
        public int CardSpace {
            get {
                return m_CardPosController.m_MaxCardCount - HandCardCount;
            }
        }
        public SelectCardMode CurSelectCardMode
        {
            get;
            protected set;
        }
        public int HandCardCount { get { return m_HandCards.Count; } }
        public bool IsUsingCard { get; set; } = false;
        public int Cost { get { return m_CostUI.Cost; } }
        public const int TurnInitCost = 3;//每回合初始費用
        public const int TurnInitCardNum = 5;//每回合初始手牌
        public RCG_Deck m_Deck = null;
        public RCG_Card m_CardTemplate = null;
        public List<RCG_Card> m_Cards = null;
        public List<RCG_Card> m_HandCards = new List<RCG_Card>();
        public int m_DrawCardCount = 0;
        [SerializeField] protected Transform m_DrawCardPos = null;//抽牌位置
        [SerializeField] protected Transform m_DiscardCardPos = null;//棄牌位置
        [SerializeField] protected Transform m_TriggerCardPos = null;//出牌目標位置
        [SerializeField] protected Transform m_CardsRoot = null;
        [SerializeField] protected RCG_CardPosController m_CardPosController = null;

        [SerializeField] protected RCG_CostUI m_CostUI = null;
        /// <summary>
        /// 選中的卡
        /// </summary>
        //[SerializeField] 
        protected RCG_Card m_SelectedCard = null;
        /// <summary>
        /// 正在觸發效果的卡
        /// </summary>
        //[SerializeField] 
        protected RCG_Card m_TriggerCard = null;

        [SerializeField] protected RCG_SelectCardUI m_SelectCardUI = null;
        protected TriggerEffectData m_TriggerEffectData = null;
        protected System.Action<RCG_Card> m_SelectCardEndAct = null;
        protected List<RCG_PlayerAction> m_PlayerActions = new List<RCG_PlayerAction>();
        protected PlayerState m_PlayerState = PlayerState.Idle;
        /// <summary>
        /// 正在使用的牌 包含被選中要棄掉的牌
        /// </summary>
        protected List<RCG_Card> m_UsingCards = new List<RCG_Card>();
        /// <summary>
        /// 演出用卡牌
        /// </summary>
        protected Queue<RCG_Card> m_AnimCards = new Queue<RCG_Card>();
        protected UCL_RectTransformCollider m_Target = null;
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
            m_SelectCardUI.Init();
            m_CardPosController.Init();
            m_Cards = new List<RCG_Card>();
            //UCL.Core.GameObjectLib.SearchChildNotIncludeParent(m_CardsRoot, m_CardPositions);
            for (int i = 0; i < m_CardPosController.m_MaxCardCount; i++) {
                var aCard = CreateCard(m_CardTemplate.name + "_" + i);
                m_CardPosController.AddCard(aCard);
                m_Cards.Add(aCard);
            }
            m_Deck.Init(this);
            var aDeckData = RCG_DataService.ins.m_DeckData;
            //Debug.LogWarning("aDeckData:" + aDeckData.UCL_ToString());
            var aCards = aDeckData.GetCardDatas();
            //Debug.LogWarning("aCards:" + aCards.UCL_ToString());
            foreach (var aCard in aCards)
            {
                m_Deck.Add(new RCG_CardBattleData(aCard));
            }

            m_Deck.Shuffle();
        }
        /// <summary>
        /// 單位死亡時觸發
        /// </summary>
        /// <param name="iUnit"></param>
        public void OnUnitDead(RCG_Unit iUnit)
        {

        }
        protected RCG_Card CreateCard(string iName)
        {
            var aCard = Instantiate(m_CardTemplate, m_CardsRoot);
            aCard.name = iName;
            aCard.Init(this);
            return aCard;
        }
        /// <summary>
        /// 捨棄手牌 必須先設定為使用中!!
        /// </summary>
        /// <param name="iCard"></param>
        public void DiscardCard(RCG_Card iCard, System.Action iEndAct)
        {
            RemoveFromHandCard(iCard);
            var aAnimCard = GetAnimCard(iCard);
            aAnimCard.transform.SetPosition(m_TriggerCardPos);
            aAnimCard.CardMoveAnime(m_DiscardCardPos, delegate () {
                iCard.CardDiscarded();
                iEndAct.Invoke();
                ReturnAnimCard(aAnimCard);
            }); //卡牌飛往棄牌堆
        }
        /// <summary>
        /// 把卡片設定為使用中
        /// </summary>
        /// <param name="iCard"></param>
        protected void AddUsingCard(RCG_Card iCard)
        {
            iCard.m_Using = true;
            m_UsingCards.Add(iCard);
            m_CardPosController.UpdateActiveCardList();
        }
        /// <summary>
        /// 演出卡牌使用動畫
        /// </summary>
        /// <param name="iCard"></param>
        /// <param name="iEndAct"></param>
        public void AddUsingCardAnim(RCG_Card iCard, System.Action iEndAct)
        {
            AddUsingCard(iCard);
            iCard.gameObject.SetActive(false);//暫時隱藏

            var aAnimCard = GetAnimCard(iCard);
            aAnimCard.TriggerCardAnime(m_TriggerCardPos, delegate() {
                iEndAct.Invoke();
                ReturnAnimCard(aAnimCard);
            }); //打出卡牌 往上飛演出
        }
        /// <summary>
        /// 清除使用中的手牌
        /// </summary>
        public void ClearUsingCards()
        {
            Debug.LogWarning("ClearUsingCards()");
            for(int i = 0; i < m_UsingCards.Count; i++)
            {
                var aCard = m_UsingCards[i];
                aCard.m_Using = false;
                aCard.gameObject.SetActive(true);
            }
            m_UsingCards.Clear();
            m_CardPosController.UpdateActiveCardList();
        }
        /// <summary>
        /// 取消使用卡牌 歸還至手牌
        /// </summary>
        public void ReturnUsingCards()
        {
            for (int i = 0; i < m_UsingCards.Count; i++)
            {
                m_UsingCards[i].m_Using = false;
            }
            m_UsingCards.Clear();
            m_CardPosController.UpdateActiveCardList();
        }
        /// <summary>
        /// 生成演出專用卡牌
        /// </summary>
        /// <param name="iTargetCard"></param>
        /// <returns></returns>
        protected RCG_Card GetAnimCard(RCG_Card iTargetCard)
        {
            RCG_Card aAnimCard = null;
            if (m_AnimCards.Count > 0)
            {
                aAnimCard = m_AnimCards.Dequeue();
            }
            if(aAnimCard == null)
            {
                aAnimCard = CreateCard("AnimCard");
            }
            aAnimCard.transform.SetPositionAndRotation(iTargetCard.transform);
            aAnimCard.gameObject.SetActive(true);
            aAnimCard.SetCardData(iTargetCard.Data);
            return aAnimCard;
        }
        /// <summary>
        /// 使用DontCheck模式選擇卡牌
        /// </summary>
        /// <param name="SelectDescription"></param>
        /// <param name="iSelectEndAction"></param>
        public void StartSelectCardDontCheck(string SelectDescription, System.Action<RCG_Card> iSelectEndAction)
        {
            if (m_SelectedCard != null)
            {
                m_SelectedCard.Deselect();
                m_SelectedCard = null;
            }
            CurSelectCardMode = SelectCardMode.DontCheck;
            m_SelectCardEndAct = iSelectEndAction;
            foreach (var aCard in m_Cards)
            {
                aCard.UnBlockSelection(RCG_Card.BlockingStatus.Player);
            }
            
            m_CardPosController.UpdateCardStatus();
            int aSelectableCount = m_CardPosController.SelectableCardCount;
            //Debug.LogError("aSelectableCount:" + aSelectableCount);
            if (aSelectableCount <= 0)
            {
                Debug.LogError("StartSelectCardDontCheck Fail!! aSelectableCount:" + aSelectableCount);
                m_SelectCardEndAct.Invoke(null);
                return;
            }
            m_SelectCardUI.Show(SelectDescription);
        }
        public void StartSelectCard(SelectCardMode iSelectMode, System.Action<RCG_Card> iSelectEndAction = null)
        {
            if (m_SelectedCard != null)
            {
                m_SelectedCard.Deselect();
                m_SelectedCard = null;
            }
            CurSelectCardMode = iSelectMode;
            m_SelectCardEndAct = iSelectEndAction;
            switch (iSelectMode) {
                case SelectCardMode.Block:
                    {
                        foreach (var aCard in m_Cards)
                        {
                            aCard.BlockSelection(RCG_Card.BlockingStatus.Player);
                        }
                        break;
                    }
                case SelectCardMode.Normal:
                    {
                        foreach (var aCard in m_Cards)
                        {
                            aCard.UnBlockSelection(RCG_Card.BlockingStatus.Player);
                        }
                        break;
                    }
                case SelectCardMode.DontCheck:
                    {
                        foreach (var aCard in m_Cards)
                        {
                            aCard.UnBlockSelection(RCG_Card.BlockingStatus.Player);
                        }
                        m_SelectCardUI.Show("");
                        break;
                    }
            }
            m_CardPosController.UpdateCardStatus();
        }
        /// <summary>
        /// 歸還演出專用卡牌
        /// </summary>
        /// <param name="iAnimCard"></param>
        protected void ReturnAnimCard(RCG_Card iAnimCard)
        {
            iAnimCard.gameObject.SetActive(false);
            m_AnimCards.Enqueue(iAnimCard);
        }
        /// <summary>
        /// 使用手牌
        /// </summary>
        /// <param name="iSelectUnits"></param>
        public void TriggerCard(List<RCG_Unit> iSelectUnits)
        {
            if (m_SelectedCard == null) return;
            m_TriggerCard = m_SelectedCard;
            RemoveFromHandCard(m_TriggerCard);//打出視為移除手牌
            //Debug.LogError("TriggerCard()");
            RCG_BattleField.ins.SetSelectMode(TargetType.Off);
            SetState(PlayerState.TriggerCardInit);

            m_TriggerEffectData = new TriggerEffectData(this);
            m_TriggerEffectData.m_Targets = iSelectUnits;
            m_TriggerEffectData.m_PlayerUnit = RCG_BattleField.ins.ActiveUnit;
            
            m_TriggerCardPos.gameObject.SetActive(true);//打牌特效
            AddUsingCardAnim(m_TriggerCard, //打出卡牌 往上飛演出
            delegate () {
                SetState(PlayerState.TriggerCardPostTriggerAction);
            });
        }
        protected void TriggerPlayerActions(System.Action iEndAction)
        {
            if (m_PlayerActions.Count == 0)
            {
                iEndAction.Invoke();
                return;
            }
            var aPrevState = m_PlayerState;
            SetState(PlayerState.PlayerAction);


            System.Action<int> aTriggerAct = null;
            aTriggerAct = delegate (int iTriggerAt)
            {
                //Debug.LogWarning("iTriggerAt:" + iTriggerAt+",Total:" + m_PlayerActions.Count);
                var aPlayerAct = m_PlayerActions[iTriggerAt];
                try
                {
                    aPlayerAct.Trigger(delegate () {
                        if (iTriggerAt + 1 < m_PlayerActions.Count)
                        {
                            aTriggerAct.Invoke(iTriggerAt + 1);
                        }
                        else
                        {
                            m_PlayerActions.Clear();
                            SetState(aPrevState);
                            iEndAction.Invoke();
                        }
                    });
                }
                catch (System.Exception e)
                {
                    m_PlayerActions.Clear();
                    Debug.LogError("TriggerPlayerActions Exception:" + e);
                    SetState(aPrevState);
                    iEndAction.Invoke();
                }
            };
            aTriggerAct.Invoke(0);
        }
        /// <summary>
        /// 將卡牌置入棄牌堆
        /// </summary>
        /// <param name="iCardData"></param>
        public void AddToDiscardPile(RCG_CardData iCardData)
        {
            m_Deck.AddToDiscardPile(iCardData);
        }
        public void AddToDeckTop(RCG_CardData iCardData)
        {
            m_Deck.AddToDeckTop(iCardData);
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
            switch (CurSelectCardMode)
            {
                case SelectCardMode.Block:return;
                case SelectCardMode.Normal:
                    {
                        SetSelectedCardNormal(iCard);
                        break;
                    }
                case SelectCardMode.DontCheck:
                    {
                        SetSelectedCardDontCheck(iCard);
                        break;
                    }
            }

        }
        /// <summary>
        /// 一般模式下設定選中的手牌 選中後會觸發選擇使用目標
        /// </summary>
        /// <param name="iCard"></param>
        protected void SetSelectedCardNormal(RCG_Card iCard)
        {
            if (m_PlayerState != PlayerState.Idle)
            {
                Debug.LogWarning("SetSelectedCard Fail, m_PlayerState:" + m_PlayerState.ToString());
                return;
            }
            if (m_SelectedCard == iCard)
            {
                Debug.LogWarning("m_SelectedCard == iCard");
                if (iCard != null)
                {
                    ClearSelectedCard();
                }
                return;
            }
            if (m_SelectedCard != null)
            {
                ClearSelectedCard();
            }
            m_SelectedCard = iCard;
            if (m_SelectedCard == null)
            {
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
        /// <summary>
        /// 不檢查技能 Cost, 例如棄牌時 設定選中的手牌
        /// </summary>
        /// <param name="iCard"></param>
        protected void SetSelectedCardDontCheck(RCG_Card iCard)
        {
            if (m_SelectedCard == iCard)
            {
                if (iCard != null)
                {
                    if (m_SelectedCard != null)
                    {
                        m_SelectedCard.Deselect();
                    }
                    m_SelectedCard = null;
                }
                return;
            }
            if (m_SelectedCard != null)
            {
                m_SelectedCard.Deselect();
            }
            m_SelectedCard = iCard;

            m_SelectedCard.Select();
        }
        /// <summary>
        /// 確認選取的卡牌
        /// </summary>
        public bool ConfirmSelectedCard()
        {
            if (m_SelectedCard == null) return false;

            if (m_SelectCardEndAct != null)
            {
                m_SelectCardEndAct.Invoke(m_SelectedCard);
            }
            return true;
        }
        public void ClearSelectedCard()
        {
            if (IsUsingCard) return;
            if (m_SelectedCard != null)
            {
                m_SelectedCard.Deselect();
            }
            m_SelectedCard = null;
            RCG_BattleField.ins.SetSelectMode(TargetType.Close);
        }
        /// <summary>
        /// 新增玩家行動
        /// </summary>
        /// <param name="iPlayerAct"></param>
        public void AddPlayerAction(RCG_PlayerAction iPlayerAct)
        {
            m_PlayerActions.Add(iPlayerAct);
        }
        #region DrawCard
        /// <summary>
        /// 當卡牌加入手牌時呼叫
        /// </summary>
        /// <param name="iCard"></param>
        public void AddToHandCard(RCG_Card iCard)
        {
            m_HandCards.Add(iCard);
        }
        public void RemoveFromHandCard(RCG_Card iCard)
        {
            m_HandCards.Remove(iCard);
        }

        /// <summary>
        /// 實際抽牌演出
        /// </summary>
        /// <param name="iEndAct"></param>
        public void DrawCardAnim(System.Action iEndAct)
        {
            var aCard = m_CardPosController.GetAvaliableCard();
            if (aCard == null)
            {
                Debug.LogError("DrawCard Fail!!aCard == null");
                iEndAct.Invoke();
                return;
            }
            if (!aCard.IsEmpty)
            {
                Debug.LogError("DrawCard Fail!! !aCard.IsEmpty");
                iEndAct.Invoke();
                return;
            }
            m_CardPosController.UpdateCardPos();
            aCard.SetCardData(DrawCardFromDeck());
            AddToHandCard(aCard);
            --m_DrawCardCount;
            m_CardPosController.UpdateActiveCardList();

            aCard.DrawCardAnime(m_DrawCardPos.position, delegate ()
            {
                iEndAct();
            });
        }
        /// <summary>
        /// 抽牌(不包含演出)
        /// </summary>
        /// <param name="iCount">抽牌張數</param>
        public void DrawCard(int iCount) {
            int aSpace = CardSpace;
            for (int i = 0; i < iCount; i++)
            {
                if (m_DrawCardCount >= aSpace)
                {
                    Debug.LogError("m_DrawCardCount:" + m_DrawCardCount + ">=aSpace:" + aSpace);
                    break;
                }
                AddPlayerAction(new RCG_DrawCardAction());
                m_DrawCardCount++;
            }
        }
        /// <summary>
        /// 從牌堆中抽一張牌
        /// </summary>
        /// <returns></returns>
        public RCG_CardData DrawCardFromDeck()
        {
            return m_Deck.Draw();
        }
        #endregion
        #region Cost
        /// <summary>
        /// 調整Cost
        /// </summary>
        /// <param name="iAlter"></param>
        /// <returns></returns>
        public bool AlterCost(int iAlter) {
            if (!m_CostUI.AlterCost(iAlter))
            {
                Debug.LogError("!m_CostUI.AlterCost, Cost:" + Cost + ",iAlter:" + iAlter);
                return false;
            }
            m_CardPosController.UpdateCardStatus();
            return true;
        }
        /// <summary>
        /// 設定Cost
        /// </summary>
        /// <param name="iCost"></param>
        public void SetCost(int iCost) {
            m_CostUI.SetCost(iCost);
            m_CardPosController.UpdateCardStatus();
        }
        #endregion
        /// <summary>
        /// 清除所有手牌(移除到棄牌堆)
        /// </summary>
        public void ClearAllHandCard() {
            foreach(var aCard in m_Cards) {
                if(!aCard.m_Used && aCard.Data != null) {
                    AddToDiscardPile(aCard.Data);
                    aCard.SetCardData(null);
                }
            }
            m_HandCards.Clear();
        }
        #region PlayerState
        /// <summary>
        /// 設定玩家狀態
        /// </summary>
        /// <param name="iPlayerState"></param>
        public void SetState(PlayerState iPlayerState)
        {
            m_PlayerState = iPlayerState;
        }
        /// <summary>
        /// 玩家行動結束
        /// </summary>
        public void TurnEnd()
        {
            ClearSelectedCard();
            ClearAllHandCard();
            RCG_BattleManager.ins.PlayerTurnEnd();
        }
        public void TurnEndPlayerAction(System.Action iEndAct)
        {
            TriggerPlayerActions(iEndAct);
        }
        /// <summary>
        /// 回合初始化
        /// </summary>
        public void TurnInit() {
            ClearSelectedCard();
            m_DrawCardCount = 0;
            m_CardPosController.UpdateCardPos();
            for (int i = 0; i < m_Cards.Count; i++) {
                var aCard = m_Cards[i];
                RCG_CardData data = null;
                if(i < TurnInitCardNum) data = DrawCardFromDeck();
                aCard.TurnInit(data);
                if (data != null)
                {
                    AddToHandCard(aCard);
                    aCard.DrawCardAnime(m_DrawCardPos.position, null);
                }
            }
            SetCost(TurnInitCost);
            StartSelectCard(SelectCardMode.Block);
        }
        #endregion
        /// <summary>
        /// 當選中行動腳色時呼叫
        /// </summary>
        /// <param name="iUnit"></param>
        virtual public void SetActiveUnit(RCG_Unit iUnit)
        {
            StartSelectCard(SelectCardMode.Normal);
            UpdateCardStatus();
            UpdateCardDiscription();
        }
        /// <summary>
        /// 更新卡牌資訊 包含判斷玩家費用是否足夠 技能是否符合等...
        /// </summary>
        virtual public void UpdateCardStatus()
        {
            m_CardPosController.UpdateCardStatus();
        }
        /// <summary>
        /// 更新卡牌描述(當攻擊力Buff等導致數值變化時
        /// </summary>
        virtual public void UpdateCardDiscription()
        {
            m_CardPosController.UpdateCardDiscription();
        }
        /// <summary>
        /// 卡片效果觸發結束
        /// </summary>
        virtual protected void PlayerActionEnd()
        {
            IsUsingCard = false;
            UpdateCardStatus();
            
            ClearSelectedCard();
            SetState(PlayerState.Idle);
            //回到腳色行動階段
            SetActiveUnit(RCG_BattleField.ins.ActiveUnit);
        }
        private void Update() {
            if(!m_Inited) return;
            m_Target = null;
            switch(m_PlayerState) {
                case PlayerState.Idle: {
                        break;
                    }
                case PlayerState.TriggerCardPostTriggerAction:
                    {
                        SetState(PlayerState.TriggeringCardPostTriggerAction);
                        m_TriggerCard.PostTriggerAction();
                        TriggerPlayerActions(delegate () {
                            SetState(PlayerState.TriggerCardAction);
                        });
                        break;
                    }
                case PlayerState.TriggerCardAction:
                    {
                        //m_TriggerCard.gameObject.SetActive(true);
                        m_TriggerCard.Deselect();
                        IsUsingCard = true;
                        UpdateCardStatus();
                        SetState(PlayerState.TriggeringCardAction);
                        m_TriggerCard.TriggerCardEffect(m_TriggerEffectData,
                        delegate (bool iTriggerSuccess) {//卡牌效果觸發完畢
                            if (!iTriggerSuccess)
                            {
                                Debug.LogError("Trigger Fail!!");
                            }
                            SetState(PlayerState.TriggerCardActionEnd);
                        });
                        break;
                    }
                case PlayerState.TriggerCardActionEnd:
                    {
                        m_TriggerCard.CardUsed();
                        m_TriggerCard = null;
                        ClearUsingCards();
                        m_CardPosController.UpdateActiveCardList();
                        SetState(PlayerState.PlayerActionStart);
                        break;
                    }
                case PlayerState.PlayerActionStart:
                    {
                        TriggerPlayerActions(PlayerActionEnd);
                        break;
                    }
            }
            m_Deck.PlayerUpdate();
        }
    }
}
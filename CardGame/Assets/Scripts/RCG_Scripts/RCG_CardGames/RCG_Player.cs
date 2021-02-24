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
            TriggerCard,//出牌中
            PlayerActionStart,//玩家行動開始
            PlayerAction,//玩家行動(受卡牌效果觸發 包含抽牌等)
        }
        /// <summary>
        /// 手牌剩餘空間
        /// </summary>
        public int CardSpace {
            get {
                //if(IsUsingCard) return m_CardPosController.m_MaxCardCount - HandCardCount + 1;
                return m_CardPosController.m_MaxCardCount - HandCardCount;
            }
        }
        public int HandCardCount { get { return m_HandCardCount; } }
        public bool IsUsingCard { get; set; } = false;
        public int Cost { get { return m_CostUI.Cost; } }
        public const int TurnInitCost = 3;//每回合初始費用
        public const int TurnInitCardNum = 5;//每回合初始手牌
        public RCG_Deck m_Deck = null;
        public RCG_Card m_CardTemplate = null;
        public List<RCG_Card> m_Cards = null;
        
        public int m_DrawCardCount = 0;
        public Transform m_DrawCardPos = null;//抽牌位置
        public Transform m_TriggerCardPos = null;//出牌目標位置
        public Transform m_CardsRoot = null;
        public RCG_CardPosController m_CardPosController = null;

        [SerializeField] protected RCG_CostUI m_CostUI = null;
        [SerializeField] protected RCG_Card m_SelectedCard = null;

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
        protected int m_HandCardCount = 0;
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
        /// 把卡片設定為使用中
        /// </summary>
        /// <param name="iCard"></param>
        public void AddUsingCard(RCG_Card iCard)
        {
            iCard.m_Using = true;
            m_UsingCards.Add(iCard);
            m_CardPosController.UpdateActiveCardList();
        }
        /// <summary>
        /// 清除使用中的手牌
        /// </summary>
        public void ClearUsingCards()
        {
            for(int i = 0; i < m_UsingCards.Count; i++)
            {
                m_UsingCards[i].m_Using = false;
            }
            m_UsingCards.Clear();
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
            if (m_Blocking) return;
            if (m_SelectedCard == null) return;
            var aTriggerCard = m_SelectedCard;
            //Debug.LogError("TriggerCard()");
            RCG_BattleField.ins.SetSelectMode(TargetType.Off);
            SetState(PlayerState.TriggerCard);

            m_TriggerCardPos.gameObject.SetActive(true);//打牌特效
            AddUsingCard(aTriggerCard);
            aTriggerCard.gameObject.SetActive(false);//暫時隱藏

            var aAnimCard = GetAnimCard(aTriggerCard);
            aAnimCard.TriggerCardAnime(m_TriggerCardPos, //打出卡牌 往上飛演出
            delegate () {
                ReturnAnimCard(aAnimCard);
                aTriggerCard.PostTriggerAction();
                TriggerPlayerActions(delegate () {
                    aTriggerCard.gameObject.SetActive(true);
                    aTriggerCard.Deselect();
                    var aData = new TriggerEffectData(this);
                    aData.m_Targets = iSelectUnits;
                    aData.m_PlayerUnit = RCG_BattleField.ins.ActiveUnit;
                    IsUsingCard = true;
                    --m_HandCardCount;//卡牌已觸發 手牌-1
                    UpdateCardStatus();
                    aTriggerCard.TriggerCardEffect(aData,
                    delegate (bool iTriggerSuccess) {//卡牌效果觸發完畢
                        if (!iTriggerSuccess)
                        {
                            Debug.LogError("Trigger Fail!!");
                        }
                        aTriggerCard.CardUsed();
                        m_CardPosController.UpdateActiveCardList();
                        ClearUsingCards();
                        SetState(PlayerState.PlayerActionStart);
                    });
                });
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
            if(m_Blocking) {
                Debug.LogWarning("SetSelectedCard Fail, Blocking!!");
                return;
            }
            if(m_PlayerState != PlayerState.Idle)
            {
                Debug.LogWarning("SetSelectedCard Fail, m_PlayerState:"+ m_PlayerState.ToString());
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
        /// <summary>
        /// 抽牌(不包含演出)
        /// </summary>
        /// <param name="iCount">抽牌張數</param>
        public void DrawCard(int iCount) {
            int aSpace = CardSpace;
            for (int i = 0; i < iCount; i++)
            {
                if (m_DrawCardCount >= CardSpace)
                {
                    break;
                }
                AddPlayerAction(new RCG_DrawCardAction());
                m_DrawCardCount++;
            }
        }
        public void AddPlayerAction(RCG_PlayerAction iPlayerAct)
        {
            m_PlayerActions.Add(iPlayerAct);
        }
        public RCG_CardData DrawCardFromDeck()
        {
            ++m_HandCardCount;
            return m_Deck.Draw();
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
                    AddToDiscardPile(aCard.Data);
                    aCard.SetCardData(null);
                }
            }
            m_HandCardCount = 0;
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
            if(m_Blocking) return;
            ClearSelectedCard();
            m_DrawCardCount = 0;

            for (int i = 0; i < m_Cards.Count; i++) {
                var card = m_Cards[i];
                RCG_CardData data = null;
                if(i < TurnInitCardNum) data = DrawCardFromDeck();
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
        /// <summary>
        /// 更新卡牌描述(當攻擊力Buff等導致數值變化時
        /// </summary>
        virtual public void UpdateCardDiscription()
        {
            m_CardPosController.UpdateCardDiscription();
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
            //m_CardPosController.UpdateActiveCardList();
            //aCard.BlockSelection(RCG_Card.BlockingStatus.Player);
            aCard.SetCardData(DrawCardFromDeck());
            --m_DrawCardCount;
            m_CardPosController.UpdateActiveCardList();
            
            aCard.DrawCardAnime(m_DrawCardPos.position, delegate ()
            {
                iEndAct();
            });
        }
        private void Update() {
            if(!m_Inited) return;
            m_Target = null;
            switch(m_PlayerState) {
                case PlayerState.Idle: {
                        break;
                    }
                case PlayerState.PlayerActionStart:
                    {
                        TriggerPlayerActions(delegate ()
                        {
                            IsUsingCard = false;
                            UpdateCardStatus();
                            ClearSelectedCard();
                            SetState(PlayerState.Idle);
                        });
                        break;
                    }
            }
            m_Deck.PlayerUpdate();
        }
    }
}
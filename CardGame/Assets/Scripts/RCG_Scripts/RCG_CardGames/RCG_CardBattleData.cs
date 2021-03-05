using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG
{
    public class RCG_CardBattleData : RCG_CardData
    {
        public RCG_CardData m_CardData = null;
        override public Sprite Icon { get { return m_CardData.Icon; } }
        override public string CardName { get { return m_CardData.CardName; } }
        override public int Cost { get { 
                int aCost = m_CardData.Cost + m_CostAlter;
                if (aCost < 0) return 0;
                return aCost;
            } }
        override public string IconName { get { return m_CardData.IconName; } }
        override public string Description
        {
            get {
                return m_CardData.Description;
            }
        }
        override public TargetType Target { get { return m_CardData.Target; } }

        override public List<UnitSkill> RequireSkills { get { return m_CardData.RequireSkills; } }

        override public CardType CardType { get { return m_CardData.CardType; } }
        override public TargetType TargetType { get { return m_CardData.TargetType; } }

        protected int m_CostAlter = 0;
        /// <summary>
        /// 初始化資料 卡牌進入牌堆後執行清除掉Buff等效果
        /// </summary>
        override public void InitData() {
            m_CostAlter = 0;
        }
        public override void AlterCost(int iAmount)
        {
            m_CostAlter += iAmount;
        }
        public RCG_CardBattleData(RCG_CardData iData)
        {
            m_CardData = iData;
            m_Data = iData.Data;
        }
        public override void PostTriggerAction()
        {
            m_CardData.PostTriggerAction();
        }
        override public void TriggerEffect(TriggerEffectData iTriggerEffectData, Action iEndAction)
        {
            m_CardData.TriggerEffect(iTriggerEffectData, iEndAction);
        }
        public override void CardDiscarded(RCG_Player iPlayer)
        {
            InitData();
            iPlayer.AddToDiscardPile(this);
        }
        override public void CardUsed(RCG_Player iPlayer)
        {
            InitData();
            switch (UsedType) {
                case UsedType.ToDiscardPile:
                    {
                        iPlayer.AddToDiscardPile(this);
                        break;
                    }
                case UsedType.ToDeckTop:
                    {
                        iPlayer.AddToDeckTop(this);
                        break;
                    }
                case UsedType.Banish:
                    {
                        break;
                    }
            }
        }
    }
}
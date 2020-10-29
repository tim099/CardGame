﻿using System.Collections;
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
        public List<RCG_Card> m_Cards = null;
        public List<RCG_CardBeginSet> m_BeginSets = null;
        public List<UCL_RectTransformCollider> m_Targets = new List<UCL_RectTransformCollider>();
        public int m_Cost = 0;
        protected RCG_Card m_DraggingCard = null;
        protected UCL_RectTransformCollider m_Target = null;
        private void Awake() {
            Init();
        }

        [UCL.Core.ATTR.UCL_FunctionButton]
        public void Init() {
            if(m_Cards == null) {
                m_Cards = new List<RCG_Card>();
            }
            if(m_Cards.Count == 0) {
                UCL.Core.GameObjectLib.SearchChild(transform, m_Cards);
            }
            if(m_BeginSets.Count == 0) {
                return;
            }
            for(int i = 0; i < m_Targets.Count; i++) {
                m_Targets[i].m_ID = i;
            }
            m_Deck = new RCG_Deck();
            var set = m_BeginSets[UCL.Core.MathLib.UCL_Random.Instance.Next(m_BeginSets.Count)];
            for(int i = 0,len = set.m_Settings.Count; i < len; i++) {
                m_Deck.Add(set.m_Settings[i].CreateCard());
            }
            m_Deck.Shuffle();
            for(int i = 0; i < m_Cards.Count; i++) {
                var card = m_Cards[i];
                card.Init(this);
            }
            TurnInit();
        }
        public void TurnInit() {
            foreach(var card in m_Cards) {
                if(!card.m_Used && card.Data != null) {
                    m_Deck.Used(card.Data);
                }
            }

            for(int i = 0; i < m_Cards.Count; i++) {
                var card = m_Cards[i];
                card.TurnInit(m_Deck.Draw());
            }
            m_Cost = MaxCost;
        }
        public void CardRelease() {
            if(m_DraggingCard != null && m_Target != null) {
                if(m_DraggingCard.TriggerCardEffect(m_Target.m_ID)) {
                    var card = m_DraggingCard;
                    m_Deck.Used(m_DraggingCard.Data);
                    UCL.TweenLib.UCL_TweenManager.Instance.KillAllOnTransform(m_DraggingCard.m_Button.transform);
                    //Debug.LogError("Hit!!:" + m_DraggingCard.name + ",m_Target:" + m_Target.name);
                    var seq = m_DraggingCard.m_Button.transform.UCL_Move(0.4f, m_Target.transform.position).SetEase(EaseType.OutElastic);
                    seq.OnComplete(() => {
                        card.CardUsed();
                    });
                    seq.Start();
                }
            }
        }
        private void Update() {
            m_DraggingCard = null;
            m_Target = null;
            foreach(var card in m_Cards) {
                if(card.IsDragging) {
                    m_DraggingCard = card;
                }
            }
            foreach(var target in m_Targets) {
                if(target.IsMouseEntered) {
                    m_Target = target;
                }
            }
            m_CostText.SetText("" + m_Cost);
        }
    }
}
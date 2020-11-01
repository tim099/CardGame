using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RCG {

    public class RCG_CardData {
        public RCG_CardData(RCG_CardSettings setting) {
            m_Setting = setting;
            m_Cost = m_Setting.m_Cost;
            m_CardType = m_Setting.m_CardType;
        }
        virtual public bool TargetCheck(int target) {
            switch(m_Setting.m_Target) {
                case Target.Null: {
                        return false;
                    }
                case Target.Player: {
                        return target == 0;
                    }
                case Target.Friend: {
                        return target == 1;
                    }
                case Target.Allied: {
                        return target <= 1;
                    }
                case Target.Enemy: {
                        if(target <= 1) return false;
                        return target <= m_Setting.m_AtkRange + 2;
                    }
                case Target.All: {
                        return true;
                    }
            }
            return true;
        }
        virtual public void TriggerEffect(RCG_Player player) {
            if(m_Setting.m_DrawCard > 0) {
                player.DrawCard(m_Setting.m_DrawCard);
            }
        }

        virtual public int GetCost() {
            return m_Cost;
        }
        protected RCG_CardSettings m_Setting;
        public int m_Cost = 1;
        public CardType m_CardType = CardType.Attack;
        public Sprite Icon { get { return m_Setting.m_Icon; } }
        virtual public string CardName { get { return m_Setting.m_CardName; } }
        virtual public string Description { get { return m_Setting.m_Description; } }
    }
}
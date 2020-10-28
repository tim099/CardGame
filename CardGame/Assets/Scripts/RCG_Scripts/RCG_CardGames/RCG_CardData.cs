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
        protected RCG_CardSettings m_Setting;
        public int m_Cost = 1;
        public CardType m_CardType = CardType.Attack;
        public Sprite Icon { get { return m_Setting.m_Icon; } }
        virtual public string CardName { get { return m_Setting.m_CardName; } }
        virtual public string Description { get { return m_Setting.m_Description; } }
    }
}
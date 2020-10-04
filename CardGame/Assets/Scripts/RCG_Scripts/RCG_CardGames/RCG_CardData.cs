using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RCG {
    public enum CardType {
        Attack = 0,
        Defense,
        Buff,
        DeBuff,
        Unknow,
    }
    public class RCG_CardData : MonoBehaviour {
        public int m_Cost = 1;
        public CardType m_CardType = CardType.Attack;
        public Sprite m_Icon;
        public string m_CardName;
        public string m_Description;

        virtual public string GetName() {
            return m_CardName;
        }
        virtual public string GetDescription() {
            return m_Description;
        }
    }
}
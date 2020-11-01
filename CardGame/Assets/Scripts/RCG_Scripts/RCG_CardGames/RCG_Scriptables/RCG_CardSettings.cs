using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RCG
{
    public enum CardType
    {
        Attack = 0,
        Defense,
        Buff,
        DeBuff,
        Unknow,
    }
    public enum Target 
    {
        Null = 0,
        Player,
        Friend,
        Allied,//both Player and Friend
        Enemy,
        All,
    }
    [CreateAssetMenu(fileName = "New CardSettings", menuName = "RCG/CardSettings")]
    public class RCG_CardSettings : ScriptableObject
    {
        public RCG_CardData CreateCard() {
            return new RCG_CardData(this);
        }


        public int m_Cost = 0;
        public string m_CardName;
        public string m_Description;

        public Target m_Target = Target.Enemy;
        public CardType m_CardType = CardType.Attack;
        public Sprite m_Icon = null;
        public int m_Atk = 0;
        public int m_AtkRange = 1;//if MeleeAttack m_AtkRange == 1
        public int m_AtkTimes = 1;

        public int m_Defense = 0;
        public int m_DrawCard = 0;
    }
}
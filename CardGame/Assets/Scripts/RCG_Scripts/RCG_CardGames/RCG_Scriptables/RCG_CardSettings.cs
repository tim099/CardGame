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

    public enum TargetType : int
    {
        /// <summary>
        /// 不需要指定目標 如抽牌
        /// </summary>
        None = 0,
        /// <summary>
        /// 使用腳色
        /// </summary>
        Player,
        /// <summary>
        /// 除了使用腳色外的我方
        /// </summary>
        Friend,
        /// <summary>
        /// 我方全體
        /// </summary>
        Allied,
        /// <summary>
        /// 我方前排
        /// </summary>
        AlliedFront,
        /// <summary>
        /// 我方後排
        /// </summary>
        AlliedBack,

        /// <summary>
        /// 敵方全體
        /// </summary>
        Enemy,

        /// <summary>
        /// 敵方前排
        /// </summary>
        EnemyFront,

        /// <summary>
        /// 敵方後排
        /// </summary>
        EnemyBack,

        /// <summary>
        /// 全體目標皆為對象
        /// </summary>
        All,
    }
    public enum Entry
    {
        Discard = 0,//Removed after used in this battle
        Return,//Return to Deck istead of DiscardPile after used
    }
    [CreateAssetMenu(fileName = "New CardSettings", menuName = "RCG/CardSettings")]
    public class RCG_CardSettings : ScriptableObject
    {
        public RCG_CardData CreateCard() {
            return new RCG_CardData(this);
        }

        public RCG_CardSettings m_FortifyCard = null;//Fortify version of this card
        public RCG_CardSettings m_Insane = null;//Insane version of this card
        public int m_Cost = 0;
        public string m_CardName;
        public string m_Description;

        public TargetType m_Target = TargetType.Enemy;
        public CardType m_CardType = CardType.Attack;
        public Sprite m_Icon = null;
        public int m_Atk = 0;
        public int m_AtkRange = 1;//if MeleeAttack m_AtkRange == 1
        public int m_AtkTimes = 1;

        public int m_Defense = 0;
        public int m_DrawCard = 0;
        public List<Entry> m_Entries = new List<Entry>();
        public StatusType m_status_type = StatusType.None;
        public int m_status_count = 0;
    }
}
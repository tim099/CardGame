using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG {
    public enum UnitActionType
    {
        None,
        Damage,
        Heal,
        Defend,
        Attack,
        Buff,
        Debuff
    }

    [UCL.Core.ATTR.EnableUCLEditor]
    public class RCG_UnitAction {
        public UnitActionType m_type = UnitActionType.None;
        public int m_amount = 0;
        public RCG_CardData m_card_data;
        public double m_duration;

        public RCG_UnitAction(UnitActionType type, int amount, RCG_CardData card_data, double duration = 0.3){
            m_type = type;
            m_amount = amount;
            m_card_data = card_data;
            m_duration = duration;
        }

        public void TakeAction(RCG_Unit unit){
            switch(m_type){
                case UnitActionType.Damage:
                    unit.DamageHP(m_amount);
                    break;
                default:
                    break;
            }
        }
    }
}
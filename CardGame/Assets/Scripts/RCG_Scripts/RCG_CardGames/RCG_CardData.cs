using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RCG {

    public class RCG_CardData {
        public Sprite Icon { get { return m_Setting.m_Icon; } }
        virtual public string CardName { get { return m_Setting.m_CardName; } }
        virtual public string Description {
            get {
                string des = "";
                if(Atk > 0) {
                    if(AtkTimes > 1) {
                        des += UCL.Core.LocalizeLib.UCL_LocalizeManager.Get("Attack_Des", Atk, AtkTimes, AtkRange) + "\n";
                    } else {
                        des += UCL.Core.LocalizeLib.UCL_LocalizeManager.Get("Attack_DesSingle", Atk, AtkRange) + "\n";
                    }
                }
                if(Defense > 0) {
                    des += UCL.Core.LocalizeLib.UCL_LocalizeManager.Get("Def_Des", Defense) + "\n";
                }
                if(Cost < 0) {
                    des += UCL.Core.LocalizeLib.UCL_LocalizeManager.Get("AddCost_Des", -Cost) + "\n";
                }
                if(m_Setting.m_DrawCard > 0) {
                    des += UCL.Core.LocalizeLib.UCL_LocalizeManager.Get("DrawCard_Des", m_Setting.m_DrawCard) + "\n";
                }
                des += m_Setting.m_Description;
                return des;
            }
        }
        virtual public int Atk { get { return m_Setting.m_Atk; } }
        virtual public int AtkTimes { get { return m_Setting.m_AtkTimes; } }
        virtual public int AtkRange { get { return m_Setting.m_AtkRange; } }
        virtual public int Defense { get { return m_Setting.m_Defense; } }
        virtual public TargetType Target { get { return m_Setting.m_Target; } }
        virtual public int Cost { get { return m_Cost; } }
        public RCG_CardData(UCL.Core.JsonLib.JsonData setting) {

        }
        public RCG_CardData(RCG_CardSettings setting) {
            m_Setting = setting;
            m_Cost = m_Setting.m_Cost;
            m_CardType = m_Setting.m_CardType;
        }
        public void LogSetting() {
            Debug.LogWarning("m_Setting:" + m_Setting.UCL_ToString());
        }
        virtual public bool TargetCheck(int target) {
            switch(m_Setting.m_Target) {
                case TargetType.Null: {
                        return false;
                    }
                case TargetType.Player: {
                        return target == 0;
                    }
                case TargetType.Friend: {
                        return target == 1;
                    }
                case TargetType.Allied: {
                        return target <= 1;
                    }
                case TargetType.Enemy: {
                        if(target <= 1) return false;
                        return target < m_Setting.m_AtkRange + 2;
                    }
                case TargetType.All: {
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


        protected RCG_CardSettings m_Setting;
        public int m_Cost = 1;
        public CardType m_CardType = CardType.Attack;

    }
}
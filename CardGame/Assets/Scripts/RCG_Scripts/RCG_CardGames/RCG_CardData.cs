using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RCG {

    public class RCG_CardData {
        public Sprite Icon { get { return m_Setting.m_Icon; } }
        virtual public string CardName { get; set; }
        virtual public int Cost { get; set; }
        virtual public string IconName { get; set; }
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
        public List<RCG_CardEffect> m_CardEffects = null;
        public RCG_CardData(UCL.Core.JsonLib.JsonData setting) {
            CardName = setting.GetString("CardName", "card_name");
            IconName = setting.GetString("IconName", CardName);
            Cost = setting.GetInt("Cost", 0);
            m_CardEffects = new List<RCG_CardEffect>();
            if(setting.Contains("CardEffect")) {
                LoadCardEffect(setting["CardEffect"]);
            }

        }
        virtual protected void LoadCardEffect(UCL.Core.JsonLib.JsonData card_effect) {
            Debug.LogWarning("CardEffect:" + card_effect.ToJson());
            for(int i = 0; i < card_effect.Count; i++) {
                var effect_data = card_effect[i];
                Debug.LogWarning("effect_data:" + effect_data.ToJson());
                if(effect_data.Contains("EffectType")) {
                    var effect = RCG_CardEffectCreator.Create(effect_data["EffectType"].GetString());
                    if(effect != null) {
                        effect.LoadJson(effect_data);
                        m_CardEffects.Add(effect);
                    } else {
                        Debug.LogError("effect == null!!");
                    }
                }

            }
        }
        public void AddCardEffect(RCG_CardEffect effect) {
            m_CardEffects.Add(effect);
        }
        public UCL.Core.JsonLib.JsonData ToJson() {
            UCL.Core.JsonLib.JsonData data = new UCL.Core.JsonLib.JsonData();
            data["CardName"] = CardName;
            data["IconName"] = IconName;
            data["Cost"] = Cost;
            UCL.Core.JsonLib.JsonData effect_data = new UCL.Core.JsonLib.JsonData().ToArray();
            data["CardEffect"] = effect_data;
            for(int i = 0; i < m_CardEffects.Count; i++) {
                effect_data.Add(m_CardEffects[i].ToJson());
            }
            return data;
        }
        public void OnGUI() {
            int delete_at = -1;
            for(int i = 0; i < m_CardEffects.Count; i++) {
                var effect = m_CardEffects[i];
                using(var scope = new GUILayout.VerticalScope("box")) {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(effect.EffectType);
                    GUILayout.FlexibleSpace();
                    if(GUILayout.Button("Delete")) {
                        delete_at = i;
                    }
                    GUILayout.EndHorizontal();
                    effect.OnGUI();
                }
            }
            if(delete_at >= 0) {
                m_CardEffects.RemoveAt(delete_at);
            }
        }

        public RCG_CardData(RCG_CardSettings setting) {
            m_Setting = setting;
            CardName = m_Setting.m_CardName;
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
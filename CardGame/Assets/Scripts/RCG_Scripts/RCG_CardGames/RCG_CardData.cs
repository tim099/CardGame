﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace RCG {
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
    [System.Serializable]
    public class RCG_CardData {
        public static string CardDataPath {
            get {
                return Application.streamingAssetsPath + "/.CardDatas/Datas";
            }
        }
        public static string CardIconPath {
            get {
                return Path.Combine(UCL.Core.FileLib.Lib.RemoveFolderPath(CardDataPath, 1), "Icons");
            }
        }
        [System.Serializable]
        public struct CardData {
            public CardType m_CardType;
            public TargetType m_TargetType;
            public string m_CardName;
            public string m_IconName;
            public int m_Cost;
        }
        public Sprite Icon { get { return m_Icon; } }
        public Sprite m_Icon = null;
        virtual public string CardName { get { return m_Data.m_CardName; } set { m_Data.m_CardName = value; } }
        virtual public int Cost { get { return m_Data.m_Cost; } set { m_Data.m_Cost = value; } }
        virtual public string IconName { get { return m_Data.m_IconName; } set { m_Data.m_IconName = value; } }
        virtual public string Description {
            get {
                string des = string.Empty;
                foreach(var aEffect in m_CardEffects) {
                    string aDes = aEffect.Description;
                    if(!string.IsNullOrEmpty(aDes)) {
                        des += aDes;
                    }
                }
                return des;
            }
        }
        virtual public int Atk { get; protected set; }
        virtual public int AtkTimes { get; protected set; }
        virtual public int AtkRange { get; protected set; }
        virtual public int Defense { get; protected set; }
        virtual public TargetType Target { get; protected set; }

        public List<RCG_CardEffect> m_CardEffects = new List<RCG_CardEffect>();
        public CardType CardType { get { return m_Data.m_CardType; } set { m_Data.m_CardType = value; } }
        public TargetType TargetType { get { return m_Data.m_TargetType; } set { m_Data.m_TargetType = value; } }
        [SerializeField] protected CardData m_Data;
        public RCG_CardData() {

        }
        public RCG_CardData(string iJson) {
            LoadJson(UCL.Core.JsonLib.JsonData.ParseJson(iJson));
        }
        public RCG_CardData(UCL.Core.JsonLib.JsonData iSetting) {
            LoadJson(iSetting);
        }
        public void LoadJson(UCL.Core.JsonLib.JsonData iSetting) {
            m_Data = UCL.Core.JsonLib.JsonConvert.LoadDataFromJson<CardData>(iSetting);
            //var test = UCL.Core.JsonLib.JsonConvert.LoadListFromJson<float>(setting["Test"]);
            //Debug.LogWarning("test:" + test.UCL_ToString());
            Debug.LogWarning("m_Data:" + m_Data.UCL_ToString());
            m_CardEffects.Clear();
            if(iSetting.Contains("CardEffect")) {
                LoadCardEffect(iSetting["CardEffect"]);
            }
            if(RCG_CardDataService.ins != null) {
                m_Icon = RCG_CardDataService.ins.GetCardIcon(m_Data.m_IconName);
            }
        }
        public UCL.Core.JsonLib.JsonData ToJson() {
            UCL.Core.JsonLib.JsonData data = new UCL.Core.JsonLib.JsonData();
            UCL.Core.JsonLib.JsonConvert.SaveDataToJson(m_Data, data);
            data["CardEffect"] = new UCL.Core.JsonLib.JsonData(m_CardEffects);
            //data["Test"] = new UCL.Core.JsonLib.JsonData(new List<float>() { 0.4f, 12.4f, 5.3f });
            //data["Test2"] = new UCL.Core.JsonLib.JsonData(new List<string>() { "AAA", "BBB", "CCC" });
            //UCL.Core.JsonLib.JsonData effect_data = new UCL.Core.JsonLib.JsonData().ToArray();
            //data["CardEffect"] = effect_data;
            //for(int i = 0; i < m_CardEffects.Count; i++) {
            //    effect_data.Add(m_CardEffects[i].SerializeToJson());
            //}
            return data;
        }
        virtual protected void LoadCardEffect(UCL.Core.JsonLib.JsonData card_effect) {
            //Debug.LogWarning("CardEffect:" + card_effect.ToJson());
            for(int i = 0; i < card_effect.Count; i++) {
                var effect_data = card_effect[i];
                Debug.LogWarning("effect_data:" + effect_data.ToJson());
                if(effect_data.Contains("EffectType")) {
                    var effect = RCG_CardEffectCreator.Create(effect_data["EffectType"].GetString());
                    if(effect != null) {
                        effect.DeserializeFromJson(effect_data);
                        m_CardEffects.Add(effect);
                    } else {
                        Debug.LogError("effect == null!!");
                    }
                    effect.Init(i);
                }

            }
        }
        public void AddCardEffect(RCG_CardEffect effect) {
            m_CardEffects.Add(effect);
        }

        public void DrawCardDatas() {

        }
        #region Edit
        public void DrawCardEffects() {
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
                    effect.OnGUI(i);
                }
            }
            if(delete_at >= 0) {
                m_CardEffects.RemoveAt(delete_at);
            }
        }
        #endregion
        virtual public bool TargetCheck(int target) {
            return true;
        }
        virtual public void TriggerEffect(TriggerEffectData iTriggerEffectData) {
            foreach(var aCardEffect in m_CardEffects) {
                aCardEffect.TriggerEffect(iTriggerEffectData);
            }
        }
    }
}
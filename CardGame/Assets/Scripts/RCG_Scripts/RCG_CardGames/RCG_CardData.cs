using System.Collections;
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

        /// <summary>
        /// 關閉選擇狀態 開放切換玩家腳色
        /// </summary>
        Close,
        /// <summary>
        /// 關閉選擇狀態 執行卡牌行動階段
        /// </summary>
        Off,
    }
    /// <summary>
    /// 使用卡牌後的回收方式
    /// </summary>
    public enum UsedType
    {
        ToDiscardPile = 0,//到棄牌堆 預設
        ToDeckTop,//返回到牌堆頂端
        Banish,//直接從這局中消失
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
        public class CardData {
            public CardType m_CardType = CardType.Unknow;
            public TargetType m_TargetType = TargetType.None;
            public UsedType m_UsedType = UsedType.ToDiscardPile;
            public string m_CardName = string.Empty;
            public string m_IconName = string.Empty;
            public int m_Cost = 0;
            public List<UnitSkill> m_RequireSkills = new List<UnitSkill>();
        }
        virtual public Sprite Icon { get { return m_Icon; } }
        virtual public string CardName {
            get {
                return UCL.Core.LocalizeLib.UCL_LocalizeManager.Get(m_Data.m_CardName);
            }
            set { m_Data.m_CardName = value; } }
        virtual public int Cost { get { return m_Data.m_Cost; } set { m_Data.m_Cost = value; } }
        virtual public string IconName { get { return m_Data.m_IconName; } set { m_Data.m_IconName = value; } }
        virtual public string Description {
            get {
                string des = string.Empty;
                foreach (var aEffect in m_CardEffects) {
                    string aDes = aEffect.Description;
                    if (!string.IsNullOrEmpty(aDes)) {
                        des += aDes;
                    }
                }
                return des;
            }
        }
        virtual public TargetType Target { get; protected set; }

        virtual public List<UnitSkill> RequireSkills { get { return m_Data.m_RequireSkills; } }

        virtual public CardType CardType { get { return m_Data.m_CardType; } set { m_Data.m_CardType = value; } }
        virtual public TargetType TargetType { get { return m_Data.m_TargetType; } set { m_Data.m_TargetType = value; } }
        virtual public UsedType UsedType { get { return m_Data.m_UsedType; } set { m_Data.m_UsedType = value; } }
        virtual public CardData Data { get{ return m_Data; } }
        protected List<RCG_CardEffect> m_CardEffects = new List<RCG_CardEffect>();
        [SerializeField] protected CardData m_Data;
        protected Sprite m_Icon = null;
        //protected HashSet<UnitSkill> m_RequireSkillSets = new HashSet<UnitSkill>();
        public RCG_CardData() { }
        public RCG_CardData(string iJson) {
            //Debug.LogWarning("iJson:" + iJson);
            LoadJson(UCL.Core.JsonLib.JsonData.ParseJson(iJson));
        }
        public RCG_CardData(UCL.Core.JsonLib.JsonData iSetting) {
            LoadJson(iSetting);
        }
        virtual public bool CheckRequireSkill(HashSet<UnitSkill> iSkills)
        {
            foreach (var aSkill in m_Data.m_RequireSkills)
            {
                if (!iSkills.Contains(aSkill)) return false;
            }
            return true;
        }
        virtual public void TriggerEffect(TriggerEffectData iTriggerEffectData, System.Action iEndAction)
        {
            if (m_CardEffects.Count == 0)
            {
                iEndAction.Invoke();
                return;
            }
            System.Action<int> aTriggerAct = null;
            //int aTriggerAt = 0;
            aTriggerAct = delegate (int iTriggerAt)
            {
                Debug.LogWarning("iTriggerAt:" + iTriggerAt);
                var aCardEffect = m_CardEffects[iTriggerAt];
                try
                {
                    aCardEffect.TriggerEffect(iTriggerEffectData, delegate () {
                        if (iTriggerAt + 1 < m_CardEffects.Count)
                        {
                            aTriggerAct.Invoke(iTriggerAt + 1);
                        }
                        else
                        {
                            iEndAction.Invoke();
                        }
                    });
                }
                catch (System.Exception e)
                {
                    Debug.LogError("aCardEffect.TriggerEffect Exception:" + e);
                    iEndAction.Invoke();
                }
            };
            aTriggerAct.Invoke(0);
        }
        virtual public void CardUsed(RCG_Player iPlayer) { }
        public void LoadJson(UCL.Core.JsonLib.JsonData iSetting) {
            m_Data = UCL.Core.JsonLib.JsonConvert.LoadDataFromJson<CardData>(iSetting);
            //var test = UCL.Core.JsonLib.JsonConvert.LoadListFromJson<float>(setting["Test"]);
            //Debug.LogWarning("test:" + test.UCL_ToString());
            //Debug.LogWarning("m_Data:" + m_Data.UCL_ToString());
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
                //Debug.LogWarning("effect_data:" + effect_data.ToJson());
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

        #region Edit
        public void OnGUICardDatas()
        {
            CardName = UCL.Core.UI.UCL_GUILayout.TextField("CardName", CardName);
            {
                string aFieldName = "CardType";
                GUILayout.BeginHorizontal();
                UCL.Core.UI.UCL_GUILayout.LabelAutoSize(aFieldName);
                bool flag = RCG_CardEditor.GetCardEditTmpData(aFieldName, false);
                CardType = UCL.Core.UI.UCL_GUILayout.Popup(CardType, ref flag);
                RCG_CardEditor.SetCardEditTmpData(aFieldName, flag);
                GUILayout.EndHorizontal();
            }
            {
                string aFieldName = "TargetType";
                GUILayout.BeginHorizontal();
                UCL.Core.UI.UCL_GUILayout.LabelAutoSize(aFieldName);
                bool flag = RCG_CardEditor.GetCardEditTmpData(aFieldName, false);
                TargetType = UCL.Core.UI.UCL_GUILayout.Popup(TargetType, ref flag);
                RCG_CardEditor.SetCardEditTmpData(aFieldName, flag);
                GUILayout.EndHorizontal();
            }
            {
                string aFieldName = "UsedType";
                GUILayout.BeginHorizontal();
                UCL.Core.UI.UCL_GUILayout.LabelAutoSize(aFieldName);
                bool flag = RCG_CardEditor.GetCardEditTmpData(aFieldName, false);
                UsedType = UCL.Core.UI.UCL_GUILayout.Popup(UsedType, ref flag);
                RCG_CardEditor.SetCardEditTmpData(aFieldName, flag);
                GUILayout.EndHorizontal();
            }
            OnGUIRequireSkills();
        }
        public void OnGUICardEffects() {
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
            //Debug.LogWarning("GUIUtility.hotControl:" + GUIUtility.hotControl);
        }
        public void OnGUIRequireSkills()
        {
            {
                GUILayout.BeginHorizontal();
                UCL.Core.UI.UCL_GUILayout.LabelAutoSize("Skill");
                string aKey = "AddSkill";
                UnitSkill aSkill = RCG_CardEditor.GetCardEditTmpData(aKey, UnitSkill.Melee);
                bool flag = RCG_CardEditor.GetCardEditTmpData(aKey + "_Flag", false);
                aSkill = UCL.Core.UI.UCL_GUILayout.Popup(aSkill, ref flag);
                RCG_CardEditor.SetCardEditTmpData(aKey, aSkill);
                RCG_CardEditor.SetCardEditTmpData(aKey + "_Flag", flag);
                if (UCL.Core.UI.UCL_GUILayout.ButtonAutoSize("Add",16))
                {
                    m_Data.m_RequireSkills.Add(aSkill);
                }
                GUILayout.EndHorizontal();
            }


            int delete_at = -1;
            for (int i = 0; i < m_Data.m_RequireSkills.Count; i++)
            {
                var aSkill = m_Data.m_RequireSkills[i];

                using (var scope = new GUILayout.VerticalScope("box"))
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(aSkill.ToString());
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Delete"))
                    {
                        delete_at = i;
                    }
                    GUILayout.EndHorizontal();
                }
            }
            if (delete_at >= 0)
            {
                m_Data.m_RequireSkills.RemoveAt(delete_at);
            }
        }
        #endregion

    }
}
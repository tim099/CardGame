using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG {
    public enum ItemType
    {
        Attack = 0,
        Defense,
        Buff,
        DeBuff,
        Unknow,
    }

    /// <summary>
    /// 對應RCG_CardData 全局共用的道具資料設定
    /// </summary>
    [System.Serializable]
    public class RCG_ItemData
    {

        public static string ItemDataRelativePath
        {
            get
            {
                return "ItemDatas/Datas";
            }
        }
        public static string ItemIconRelativePath
        {
            get
            {
                return "ItemDatas/Icons";
            }
        }
        public static string ItemDataPath
        {
            get
            {
                return Application.streamingAssetsPath + "/" + ItemDataRelativePath;
            }
        }
        [System.Serializable]
        public class ItemData
        {
            public ItemType m_ItemType = ItemType.Unknow;
            public TargetType m_TargetType = TargetType.None;
            public UsedType m_UsedType = UsedType.ToDiscardPile;
            public string m_ItemName = string.Empty;
            public string m_IconName = string.Empty;
            public int m_Cost = 0;
            public List<UnitSkill> m_RequireSkills = new List<UnitSkill>();
        }
        protected List<RCG_CardEffect> m_ItemEffects = new List<RCG_CardEffect>();
        [SerializeField] protected ItemData m_Data = new ItemData();
        protected Sprite m_Icon = null;
        virtual public string ItemName { get { return m_Data.m_ItemName; } set { m_Data.m_ItemName = value; } }
        virtual public string IconName { get { return m_Data.m_IconName; } set { m_Data.m_IconName = value; } }
        virtual public Sprite Icon { get { return m_Icon; } }
        virtual public ItemType ItemType { get { return m_Data.m_ItemType; } set { m_Data.m_ItemType = value; } }
        virtual public TargetType TargetType { get { return m_Data.m_TargetType; } set { m_Data.m_TargetType = value; } }
        virtual public UsedType UsedType { get { return m_Data.m_UsedType; } set { m_Data.m_UsedType = value; } }
        virtual public string Description
        {
            get
            {
                string des = string.Empty;
                foreach (var aEffect in m_ItemEffects)
                {
                    string aDes = aEffect.Description;
                    if (!string.IsNullOrEmpty(aDes))
                    {
                        des += aDes;
                    }
                }
                return des;
            }
        }

        public RCG_ItemData() { }
        public RCG_ItemData(string iJson)
        {
            //Debug.LogWarning("iJson:" + iJson);
            LoadJson(UCL.Core.JsonLib.JsonData.ParseJson(iJson));
        }
        public RCG_ItemData(UCL.Core.JsonLib.JsonData iSetting)
        {
            LoadJson(iSetting);
        }
        public UCL.Core.JsonLib.JsonData ToJson()
        {
            UCL.Core.JsonLib.JsonData data = new UCL.Core.JsonLib.JsonData();
            UCL.Core.JsonLib.JsonConvert.SaveDataToJson(m_Data, data);
            data["ItemEffect"] = new UCL.Core.JsonLib.JsonData(m_ItemEffects);
            return data;
        }
        public void LoadJson(UCL.Core.JsonLib.JsonData iSetting)
        {
            m_Data = UCL.Core.JsonLib.JsonConvert.LoadDataFromJson<ItemData>(iSetting);
            m_ItemEffects.Clear();
            if (iSetting.Contains("ItemEffect"))
            {
                LoadCardEffect(iSetting["ItemEffect"]);
            }
            if (RCG_ItemDataService.ins != null)
            {
                m_Icon = RCG_ItemDataService.ins.GetItemIcon(m_Data.m_IconName);
            }

        }
        virtual protected void LoadCardEffect(UCL.Core.JsonLib.JsonData card_effect)
        {
            for (int i = 0; i < card_effect.Count; i++)
            {
                var effect_data = card_effect[i];
                if (effect_data.Contains("EffectType"))
                {
                    var effect = RCG_CardEffectCreator.Create(effect_data["EffectType"].GetString());
                    if (effect != null)
                    {
                        effect.DeserializeFromJson(effect_data);
                        m_ItemEffects.Add(effect);
                    }
                    else
                    {
                        Debug.LogError("effect == null!!");
                    }
                    effect.Init(i);
                }

            }
        }
        public void AddItemEffect(RCG_CardEffect effect)
        {
            m_ItemEffects.Add(effect);
        }
        #region Edit
        public void OnGUICardDatas()
        {
            m_Data.m_ItemName = UCL.Core.UI.UCL_GUILayout.TextField("Name", m_Data.m_ItemName);
            {
                string aFieldName = "ItemType";
                GUILayout.BeginHorizontal();
                UCL.Core.UI.UCL_GUILayout.LabelAutoSize(aFieldName);
                bool flag = RCG_CardEditor.GetCardEditTmpData(aFieldName, false);
                ItemType = UCL.Core.UI.UCL_GUILayout.Popup(ItemType, ref flag);
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
        public void OnGUICardEffects()
        {
            int delete_at = -1;
            for (int i = 0; i < m_ItemEffects.Count; i++)
            {
                var effect = m_ItemEffects[i];
                using (var scope = new GUILayout.VerticalScope("box"))
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(effect.EffectType);
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Delete"))
                    {
                        delete_at = i;
                    }
                    GUILayout.EndHorizontal();
                    effect.OnGUI(i);
                }
            }
            if (delete_at >= 0)
            {
                m_ItemEffects.RemoveAt(delete_at);
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
                if (UCL.Core.UI.UCL_GUILayout.ButtonAutoSize("Add", 16))
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



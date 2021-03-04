using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG {
    
    public static class RCG_CardEffectCreator {
        public static List<string> EffectNameList {
            get
            {
                if(m_EffectNameList == null)
                {
                    Init();
                }
                return m_EffectNameList;
            }
        }
        public static List<string> m_EffectNameList = null;
        static Dictionary<string, System.Func<RCG_CardEffect>> m_CreateDic = null;
        [RuntimeInitializeOnLoadMethod]
        static void Init() {
            //Debug.LogError("RCG_CardEffectCreator Init!!");
            m_CreateDic = new Dictionary<string, System.Func<RCG_CardEffect>>();
            m_EffectNameList = new List<string>();
            AddEffect<RCG_CardAttackEffect>();
            AddEffect<RCG_CardHealEffect>();
            AddEffect<RCG_CardDrawEffect>();
            AddEffect<RCG_CardCostEffect>();
            AddEffect<RCG_CardDefenseEffect>();
            AddEffect<RCG_CardStatusEffect>();
            AddEffect<RCG_CardDiscardEffect>();
        }
        static void AddEffect<T>() where T : RCG_CardEffect, new() {
            string key = typeof(T).Name.ToString().Replace("RCG_Card", "");//.Replace("Effect", "");
            m_CreateDic.Add(key, delegate () { return new T(); });
            m_EffectNameList.Add(key);
        }
        public static RCG_CardEffect Create(int id) {
            if(id < 0 || id >= m_EffectNameList.Count) return null;
            return Create(m_EffectNameList[id]);
        }
        public static RCG_CardEffect Create(string type) {
            if(m_CreateDic == null) Init();
            if(m_CreateDic.ContainsKey(type)) {
                var aEffect = m_CreateDic[type].Invoke();
                return aEffect;
            }
            Debug.LogError("RCG_CardEffectCreator Create:" + type + ",Fail!!");
            return null;
        }
    }
}
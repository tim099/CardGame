using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RCG
{
    public static class RCG_MonsterActionCreator
    {
        public static List<string> m_ActionNameList = null;
        static Dictionary<string, System.Func<RCG_MonsterAction>> m_CreateDic = null;
        [RuntimeInitializeOnLoadMethod]
        static void Init()
        {
            m_CreateDic = new Dictionary<string, System.Func<RCG_MonsterAction>>();
            m_ActionNameList = new List<string>();
            AddAction<RCG_MonsterSimpleAttackAction>();
            //AddAction<RCG_MonsterIdleAction>();
        }
        static void AddAction<T>() where T : RCG_MonsterAction, new()
        {
            string key = typeof(T).Name.ToString().Replace("RCG_Monster", "");
            m_CreateDic.Add(key, delegate () { return new T(); });
            m_ActionNameList.Add(key);
        }
        public static RCG_MonsterAction CreateBasicAction(string type)
        {
            if (m_CreateDic == null) Init();
            Debug.Log("Create Basic Action : " + type);
            if (m_CreateDic.ContainsKey(type))
            {
                var aAction = m_CreateDic[type].Invoke();
                return aAction;
            }
            Debug.LogError("RCG_MonsterActionCreator Create:" + type + ",Fail!!");
            return null;
        }

        public static RCG_MonsterAction Create(string typeVarientName)
        {
            var aFiles = UCL.Core.FileLib.Lib.GetFiles(RCG_MonsterAction.MonsterActionPath);
            RCG_MonsterAction iAction = new RCG_MonsterAction();
            foreach (var aFile in aFiles)
            {
                if (!aFile.Contains(".json")) continue;
                if (aFile.Contains(".meta")) continue; //ignore meta files
                var aData = UCL.Core.JsonLib.JsonData.ParseJson(System.IO.File.ReadAllText(aFile));
                if (aData["m_ActionName"] != typeVarientName) continue;
                iAction = CreateBasicAction(aData["m_ActionName"]);
                iAction.DeserializeFromJson(aData);
                break; 
            }
            return iAction;
        }
    }
}
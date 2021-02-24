using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Reflection;

namespace RCG
{
    public class RCG_MonsterAction : UCL.Core.JsonLib.IJsonSerializable
    {
        virtual public string ActionType { get { return this.GetType().Name.Replace("RCG_Monster", ""); } }
        virtual public string Description { get { return string.Empty; } }
        protected string m_Icon;
        protected string m_Description;
        protected string m_DescriptionShort;

        public static string MonsterActionPath
        {
            get
            {
                return Application.streamingAssetsPath + "/.MonsterActions/Datas";
            }
        }
        public static string MonsterActionIconPath
        {
            get
            {
                return Path.Combine(UCL.Core.FileLib.Lib.RemoveFolderPath(MonsterActionPath, 1), "Icons");
            }
        }

        virtual public void DeserializeFromJson(UCL.Core.JsonLib.JsonData data)
        {
            UCL.Core.JsonLib.JsonConvert.LoadDataFromJson(this, data);
        }

        virtual public UCL.Core.JsonLib.JsonData SerializeToJson()
        {
            UCL.Core.JsonLib.JsonData data = new UCL.Core.JsonLib.JsonData();
            data["ActionType"] = ActionType;
            UCL.Core.JsonLib.JsonConvert.SaveDataToJson(this, data);
            return data;
        }

        virtual public bool ActionAvailable()
        {
            return false;
        }

        virtual public void TriggerAction(Action iEndAction)
        {
            iEndAction.Invoke();
        }

        public virtual string GetIcon()
        {
            return m_Icon + ".png";
        }

        public virtual string GetDescription()
        {
            List<FieldInfo> props = new List<FieldInfo>();
            foreach (var prop in this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                props.Add(prop);
            }
            props.Sort(delegate (FieldInfo x, FieldInfo y)
            {
                if (x.Name.Length < y.Name.Length) return 1;
                else if (x.Name.Length > y.Name.Length) return -1;
                else return 0;
            });
            foreach (var prop in props)
            {
                m_Description = m_Description.Replace("$" + prop.Name, prop.GetValue(this).ToString());
            }
            return m_Description;
        }

        public virtual string GetDescriptionShort()
        {

            List<FieldInfo> props = new List<FieldInfo>();
            foreach (var prop in this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                props.Add(prop);
            }
            props.Sort(delegate (FieldInfo x, FieldInfo y)
            {
                if (x.Name.Length < y.Name.Length) return 1;
                else if (x.Name.Length > y.Name.Length) return -1;
                else return 0;
            });
            foreach (var prop in props)
            {
                m_DescriptionShort = m_DescriptionShort.Replace("$" + prop.Name, prop.GetValue(this).ToString());
            }
            return m_DescriptionShort;
        }
    }
}

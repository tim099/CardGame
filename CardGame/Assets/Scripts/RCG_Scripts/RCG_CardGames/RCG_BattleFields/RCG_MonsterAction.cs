using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace RCG
{
    public class RCG_MonsterAction : UCL.Core.JsonLib.IJsonSerializable
    {
        virtual public string ActionType { get { return this.GetType().Name.Replace("RCG_Monster", ""); } }
        virtual public string Description { get { return string.Empty; } }

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
    }
}

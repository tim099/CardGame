using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG
{
    public class RCG_MonsterAction
    {
        virtual public string ActionType { get { return this.GetType().Name.Replace("RCG_Monster", ""); } }
        virtual public string Description { get { return string.Empty; } }

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
    }
}

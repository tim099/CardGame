using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG {
    public class RCG_CardEffect {
        virtual public string EffectType { get { return this.GetType().Name.Replace("RCG_Card",""); } }

        virtual public void LoadJson(UCL.Core.JsonLib.JsonData data) {
            UCL.Core.JsonLib.JsonConvert.LoadDataFromJson(this, data);
        }
        virtual public void OnGUI() { }
        virtual public UCL.Core.JsonLib.JsonData ToJson() {
            UCL.Core.JsonLib.JsonData data = new UCL.Core.JsonLib.JsonData();
            data["EffectType"] = EffectType;
            UCL.Core.JsonLib.JsonConvert.SaveDataToJson(this, data);
            return data;
        }
    }
}


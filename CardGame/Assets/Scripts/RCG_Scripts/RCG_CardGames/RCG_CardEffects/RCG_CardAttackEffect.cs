using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG {
    public class RCG_CardAttackEffect : RCG_BattleEffect {
        public int m_Atk = 0;

        override public void OnGUI() {
            using(var scope = new GUILayout.VerticalScope("box")) {
                base.OnGUI();
                m_Atk = UCL.Core.UI.UCL_GUILayout.IntField("Atk", m_Atk);
            }
        }
        //override public void LoadJson(UCL.Core.JsonLib.JsonData data) {
        //    UCL.Core.JsonLib.JsonConvert.LoadDataFromJson(this, data);
        //}
        //override public UCL.Core.JsonLib.JsonData ToJson() {
        //    UCL.Core.JsonLib.JsonData data = base.ToJson();
        //    UCL.Core.JsonLib.JsonConvert.SaveDataToJson(this, data);
        //    return data;
        //}
    }
}
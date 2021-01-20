using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RCG {
    public class RCG_CardDrawEffect : RCG_CardEffect {
        public int m_DrawCardNum = 0;
        override public void OnGUI() {
            base.OnGUI();
            //using(var scope = new GUILayout.VerticalScope("box")) {
                
            //    m_DrawCardNum = UCL.Core.UI.UCL_GUILayout.IntField("DrawCardNum", m_DrawCardNum);
            //}
        }
    }
}
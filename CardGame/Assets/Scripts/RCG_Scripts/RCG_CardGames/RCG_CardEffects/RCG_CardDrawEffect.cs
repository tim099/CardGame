using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RCG {
    public class RCG_CardDrawEffect : RCG_CardEffect {
        public int m_DrawCardNum = 0;
        override public void OnGUI(int iID) {
            base.OnGUI(iID);
            //using(var scope = new GUILayout.VerticalScope("box")) {
                
            //    m_DrawCardNum = UCL.Core.UI.UCL_GUILayout.IntField("DrawCardNum", m_DrawCardNum);
            //}
        }
        override public string Description {
            get {
                return UCL.Core.LocalizeLib.UCL_LocalizeManager.Get("DrawCard_Des", m_DrawCardNum) + System.Environment.NewLine;
            }
        }
        public override void TriggerEffect(TriggerEffectData iTriggerEffectData) {
            base.TriggerEffect(iTriggerEffectData);
            iTriggerEffectData.p_Player.DrawCard(m_DrawCardNum);
        }
    }
}
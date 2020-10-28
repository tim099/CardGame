using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UCL.Core;
using UCL.Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace RCG {

    public class RCG_Card : MonoBehaviour {
        public Image m_Image;
        public Text m_NameText;
        public Text m_DescriptionText;
        public UCL.Core.UI.UCL_Button m_Button;
        protected RCG_CardData m_Data;

        virtual public bool IsDragging() {
            return m_Button.m_Dragging;
        }
        //void Onvalidate() {
        //    if(m_Button == null) m_Button = UCL.Core.GameObjectLib.SearchChild<UCL_Button>(transform);
        //}
        virtual public void SetData(RCG_CardData _Data) {
            m_Data = _Data;
            m_Image.sprite = m_Data.m_Icon;
            m_NameText.text = m_Data.GetName();
            m_DescriptionText.text = m_Data.GetDescription();
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(gameObject);
#endif
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UCL.Core;
using UCL.Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace RCG {

    public class RCG_Card : MonoBehaviour {
        public RCG_Player p_Player;
        public Image m_Image;
        public Text m_NameText;
        public Text m_DescriptionText;
        public UCL.Core.UI.UCL_Button m_Button;
        protected RCG_CardData m_Data;

        virtual public bool IsDragging {
            get { return m_Button.m_Dragging; }
        }
        
        //void Onvalidate() {
        //    if(m_Button == null) m_Button = UCL.Core.GameObjectLib.SearchChild<UCL_Button>(transform);
        //}
        virtual public void Init(RCG_Player _Player,RCG_CardData _Data) {
            p_Player = _Player;
            m_Data = _Data;
            m_Image.sprite = m_Data.Icon;
            m_NameText.text = m_Data.CardName;
            m_DescriptionText.text = m_Data.Description;
            m_Button.m_OnPointerUp.AddListener(p_Player.CardRelease);
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(gameObject);
#endif
        }
    }
}
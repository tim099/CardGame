using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UCL.Core;
using UCL.Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace RCG
{
    public class RCG_CardDisplayer : MonoBehaviour
    {
        public Image m_Image;
        public Text m_NameText;
        public Text m_DescriptionText;
        public Text m_CostText;
        protected RCG_CardData m_Data;
        virtual public void Init(RCG_CardData _Data) {
            m_Data = _Data;
            m_Image.SetSprite(m_Data.Icon);
            m_NameText.SetText(m_Data.CardName);
            m_DescriptionText.SetText(m_Data.Description);
            m_CostText.SetText(m_Data.m_Cost);
        }
    }
}
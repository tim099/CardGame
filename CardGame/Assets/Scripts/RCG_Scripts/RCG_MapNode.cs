using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCL.GameLib;
using UnityEngine.UI;

namespace RCG {
    public class RCG_MapNode : UCL_GraphNode {
        public string m_NodeName;
        public RCG_NodeEvent[] m_Events;
        public Text m_NodeText;
        public Button m_NodeButton;

        protected RCG_Map p_RCGMap;

        virtual public void InitMapNode(RCG_Map _RCGMap) {
            p_RCGMap = _RCGMap;
            m_NodeText.SetText(m_NodeName);
            m_NodeButton.onClick.AddListener(delegate () {
                Debug.LogWarning("ClickNode" + m_NodeName);
            });
        }
    }
}


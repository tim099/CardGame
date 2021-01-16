using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCL.GameLib;
using UnityEngine.UI;

namespace RCG {
    public class RCG_MapNode : UCL_GraphNode {
        public string m_NodeName;
        public RCG_NodeEvent[] m_Events;

        protected RCG_Map p_RCGMap;
        protected RCG_MapNodeContent m_MapNodeContent;
        virtual public void InitMapNode(RCG_Map _RCGMap, RCG_MapNodeContent _MapNodeContent) {
            p_RCGMap = _RCGMap;
            m_MapNodeContent = Instantiate(_MapNodeContent, transform);
            m_MapNodeContent.gameObject.SetActive(true);
            m_MapNodeContent.m_NodeText.SetText(m_NodeName);
            m_MapNodeContent.m_NodeButton.onClick.AddListener(delegate () {
                p_RCGMap.PlayerMoveTo(this);
                Debug.LogWarning("ClickNode" + m_NodeName);
            });
        }
        virtual public void UpdateSelectNode(RCG_MapNode node) {
            if(node == this || node == null) {
                m_MapNodeContent.m_NodeButton.interactable = false;
            } else {
                m_MapNodeContent.m_NodeButton.interactable = node.CanMoveTo(this);
            }
        }
        virtual public void Selected() {
            if(m_Events != null && m_Events.Length > 0) {
                var target_event = m_Events[UCL.Core.MathLib.UCL_Random.Instance.Next(m_Events.Length)];
                if(target_event != null) target_event.StartEvent();
            }
        }
    }
}


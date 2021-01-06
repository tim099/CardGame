using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG {
    public class RCG_Map : MonoBehaviour {
        public Transform m_NodesRoot;
        public List<RCG_MapNode> m_MapNodes = new List<RCG_MapNode>();
        virtual public void Init() {
            m_MapNodes.Clear();
            UCL.Core.GameObjectLib.SearchChild(m_NodesRoot, m_MapNodes);
            foreach(var node in m_MapNodes) {
                node.InitMapNode(this);
            }
        }
    }
}
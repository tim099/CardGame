using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG {
    public class RCG_CardPos : MonoBehaviour {
        public bool IsCardActive {
            get { return m_Card.Data != null; }
        }
        public Transform m_CardPos = null;
        public RCG_Card m_Card = null;
        public float m_Angle = 0;
        public void SetAngle(float iAngle) {
            m_Angle = iAngle;
            transform.rotation = Quaternion.Euler(0, 0, m_Angle);
        }
        public void SetCard(RCG_Card iChild) {
            m_Card = iChild;
            m_Card.transform.SetParent(m_CardPos);
            m_Card.transform.position = m_CardPos.position;
            m_Card.transform.rotation = m_CardPos.rotation;
            //m_Child.transform.SetAsFirstSibling();
        }
    }
}
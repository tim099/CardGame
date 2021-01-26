using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG {
    public class RCG_CardPos : MonoBehaviour {
        public bool IsCardActive {
            get { return !m_Card.IsEmpty; }
        }
        public Transform m_CardPos = null;
        public RCG_Card m_Card = null;
        public float m_Angle = 0;
        public float m_TargetAngle = 0;
        public void SetAngle(float iAngle) {
            if(m_Angle == iAngle) {
                return;
            }
            m_Angle = iAngle;
            m_TargetAngle = m_Angle;
            transform.rotation = Quaternion.Euler(0, 0, m_Angle);
        }
        public void SetTargetAngle(float iAngle) {
            m_TargetAngle = iAngle;
        }
        public void SetCard(RCG_Card iChild) {
            m_Card = iChild;
            m_Card.transform.SetParent(m_CardPos);
            m_Card.transform.position = m_CardPos.position;
            m_Card.transform.rotation = m_CardPos.rotation;
            //m_Child.transform.SetAsFirstSibling();
        }
        private void Update() {
            if(m_TargetAngle != m_Angle) {
                float aDel = m_TargetAngle - m_Angle;
                const float Vel = 0.12f;
                const float MinVel = 0.16f;
                if(Mathf.Abs(aDel) < Vel) {
                    m_Angle = m_TargetAngle;
                } else {
                    float V = Mathf.Abs(aDel);
                    if(V < MinVel) {
                        V = MinVel;
                    }
                    V = Mathf.Sqrt(V);
                    if(aDel > 0) {
                        m_Angle += V * Vel;
                    } else {
                        m_Angle -= V * Vel;
                    }
                    
                }
                transform.rotation = Quaternion.Euler(0, 0, m_Angle);
            }
        }
    }
}
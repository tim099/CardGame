using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG {
    public class RCG_CardPosController : MonoBehaviour {
        public RCG_CardPos m_CardPosTmp = null;
        public Transform m_CardPosRoot = null;
        public List<RCG_CardPos> m_CardPosList = new List<RCG_CardPos>();
        public List<RCG_CardPos> m_ActiveCardPosList = new List<RCG_CardPos>();
        public List<RCG_CardPos> m_InActiveCardPosList = new List<RCG_CardPos>();
        public int m_MaxAngle = 7;
        public int m_MaxCardCount = 10;
        public float m_MaxMoveDeg = 0.1f;
        public float m_SelectedCardAngle = 3f;
        virtual public void Init() {
            //for(int i = 0; i < m_MaxCardCount; i++) {
            //    var aCardPos = Instantiate(m_CardPosTmp, m_CardPosRoot);
            //    m_CardPosList.Add(aCardPos);
            //}
        }
        virtual public void AddCard(RCG_Card iCard) {
            var aCardPos = Instantiate(m_CardPosTmp, m_CardPosRoot);
            aCardPos.transform.position = m_CardPosRoot.transform.position;
            aCardPos.SetCard(iCard);
            aCardPos.SetAngle(m_MaxAngle - m_CardPosList.Count*2);
            iCard.SetCardPos(aCardPos);
            m_CardPosList.Add(aCardPos);
        }
        private void Update() {
            m_ActiveCardPosList.Clear();
            m_InActiveCardPosList.Clear();
            int aSelectedCardNum = 0;
            for(int i = 0; i < m_CardPosList.Count; i++) {
                var aCardPos = m_CardPosList[i];
                if(aCardPos.IsCardActive) {
                    m_ActiveCardPosList.Add(aCardPos);
                    if(aCardPos.m_Card.IsSelected) {
                        aSelectedCardNum++;
                    }
                } else {
                    m_InActiveCardPosList.Add(aCardPos);
                }
            }
            m_CardPosList.Clear();
            int aDiv = m_ActiveCardPosList.Count - 1 - aSelectedCardNum;
            if(aDiv <= 0) aDiv = 1;
            
            float aSpaceAngle = ((2 * m_MaxAngle - m_SelectedCardAngle * aSelectedCardNum) / (float)aDiv);
            //Debug.LogWarning("aSelectedCardNum:" + aSelectedCardNum+ ",aSpaceAngle:"+ aSpaceAngle+".aDiv:" + aDiv);
            float aAngleAt = m_MaxAngle;
            for(int i = 0; i < m_ActiveCardPosList.Count; i++) {
                var aCardPos = m_ActiveCardPosList[i];
                m_CardPosList.Add(aCardPos);
                aCardPos.transform.SetAsLastSibling();
                aCardPos.SetAngle(aAngleAt);
                if(aCardPos.m_Card.IsSelected) {
                    aAngleAt -= m_SelectedCardAngle;
                } else {
                    aAngleAt -= aSpaceAngle;
                }
            }
            for(int i = 0; i < m_InActiveCardPosList.Count; i++) {
                var aCardPos = m_InActiveCardPosList[i];
                m_CardPosList.Add(aCardPos);
                aCardPos.transform.SetAsLastSibling();
            }
        }
    }
}
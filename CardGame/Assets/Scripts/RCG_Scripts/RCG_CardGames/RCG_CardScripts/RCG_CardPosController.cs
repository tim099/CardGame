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
        public float m_CardAngle = 2f;
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
        /// <summary>
        /// 更新卡牌資訊 包含判斷玩家費用是否足夠
        /// </summary>
        virtual public void UpdateCardStatus() {
            foreach(var aCardPos in m_CardPosList) {
                aCardPos.m_Card.UpdateCardStatus();
            }
            UpdateActiveCardList();
        }
        virtual public RCG_Card GetAvaliableCard() {
            UpdateActiveCardList();
            if (m_InActiveCardPosList.Count == 0)
            {
                Debug.LogError("GetAvaliableCard() Fail!!m_InActiveCardPosList.Count == 0");
                return null;
            }
            for(int i = 0; i < m_InActiveCardPosList.Count; i++)
            {
                if(m_InActiveCardPosList[i].IsCardAvaliable) return m_InActiveCardPosList[i].m_Card;
            }

            Debug.LogError("GetAvaliableCard() Fail!!m_InActiveCardPosList[i].IsCardAvaliable can't find avaliable card!!");
            return null;
        }
        public void UpdateActiveCardList()
        {
            RCG_CardPos aUsingCardPos = null;
            m_ActiveCardPosList.Clear();
            m_InActiveCardPosList.Clear();

            for (int i = 0; i < m_CardPosList.Count; i++)
            {
                var aCardPos = m_CardPosList[i];
                if (aCardPos.IsCardActive)
                {
                    if (!aCardPos.IsUsing)
                    {
                        m_ActiveCardPosList.Add(aCardPos);
                    }
                    else
                    {
                        aUsingCardPos = aCardPos;
                    }
                }
                else
                {
                    m_InActiveCardPosList.Add(aCardPos);
                }
            }
            m_CardPosList.Clear();
            for (int i = 0; i < m_ActiveCardPosList.Count; i++)
            {
                var aCardPos = m_ActiveCardPosList[i];
                m_CardPosList.Add(aCardPos);
            }
            if (aUsingCardPos != null)
            {
                m_CardPosList.Add(aUsingCardPos);
            }
            for (int i = 0; i < m_InActiveCardPosList.Count; i++)
            {
                var aCardPos = m_InActiveCardPosList[i];
                m_CardPosList.Add(aCardPos);
            }
        }
        private void Update() {
            //UpdateActiveCardList();
            List<RCG_CardPos> aSelectedCardPos = new List<RCG_CardPos>();
            int aSelectedCardNum = 0;
            for (int i = 0; i < m_ActiveCardPosList.Count - 1; i++) {
                var aCardPos = m_ActiveCardPosList[i];
                if(aCardPos.m_Card.IsCardDisplayerSelected) {
                    aSelectedCardNum++;
                    aSelectedCardPos.Add(aCardPos);
                }
                //if(aCardPos.m_Card.IsSelected) {
                //    aSelectedCardPos.Add(aCardPos);
                //}
            }
            //m_CardPosList.Clear();
            int aDiv = m_ActiveCardPosList.Count - 1 - aSelectedCardNum;
            if(aDiv <= 0) aDiv = 1;

            float aMaxSpace = m_SelectedCardAngle * aSelectedCardNum + m_CardAngle * (m_ActiveCardPosList.Count - 1 - aSelectedCardNum);

            float aSpaceAngle = ((2 * m_MaxAngle - m_SelectedCardAngle * aSelectedCardNum) / (float)aDiv);
            //Debug.LogWarning("aSelectedCardNum:" + aSelectedCardNum+ ",aSpaceAngle:"+ aSpaceAngle+".aDiv:" + aDiv);
            float aAngleAt = m_MaxAngle;
            if(aMaxSpace <= 2 * m_MaxAngle) {
                aSpaceAngle = m_CardAngle;
                aAngleAt = 0.5f * aMaxSpace;
                //Debug.LogWarning("aMaxSpace:" + aMaxSpace + ",aAngleAt:" + aAngleAt);
            }
            for(int i = 0; i < m_ActiveCardPosList.Count; i++) {
                var aCardPos = m_ActiveCardPosList[i];
                //m_CardPosList.Add(aCardPos);
                aCardPos.transform.SetAsLastSibling();
                aCardPos.SetTargetAngle(aAngleAt);
                if(aCardPos.m_Card.IsCardDisplayerSelected) {
                    aAngleAt -= m_SelectedCardAngle;
                } else {
                    aAngleAt -= aSpaceAngle;
                }
            }
            for(int i = 0; i < m_InActiveCardPosList.Count; i++) {
                var aCardPos = m_InActiveCardPosList[i];
                //m_CardPosList.Add(aCardPos);
                aCardPos.SetAngle(aAngleAt);
                //aCardPos.transform.SetAsLastSibling();
            }

            foreach(var fSelectedCardPos in aSelectedCardPos) {
                fSelectedCardPos.transform.SetAsLastSibling();
            }
        }
    }
}
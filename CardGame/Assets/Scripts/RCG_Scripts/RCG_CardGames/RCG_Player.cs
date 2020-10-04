using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG {
    [UCL.Core.ATTR.EnableUCLEditor]
    public class RCG_Player : MonoBehaviour {
        public RCG_Deck m_Deck;
        public List<RCG_Card> m_Cards;
        public List<RCG_CardData> m_CardDatas;

        private void Awake() {
            Test();
        }

        [UCL.Core.ATTR.UCL_FunctionButton]
        public void Test() {
            if(m_Cards == null) {
                m_Cards = new List<RCG_Card>();
            }
            if(m_Cards.Count == 0) {
                UCL.Core.GameObjectLib.SearchChild(transform, m_Cards);
            }
            if(m_CardDatas.Count == 0) {
                return;
            }
            for(int i = 0; i < m_Cards.Count; i++) {
                var card = m_Cards[i];
                card.SetData(m_CardDatas[UCL.Core.MathLib.UCL_Random.Instance.Next(m_CardDatas.Count)]);
            }
        }
    }
}
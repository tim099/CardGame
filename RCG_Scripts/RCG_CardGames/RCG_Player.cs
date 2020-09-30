using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG {
    [UCL.Core.ATTR.EnableUCLEditor]
    public class RCG_Player : MonoBehaviour {
        public RCG_Deck m_Deck;
        public List<RCG_Card> m_Cards;
        public List<RCG_CardData> m_CardDatas;
        [UCL.Core.ATTR.UCL_FunctionButton]
        public void Test() {
            if(m_CardDatas.Count == 0) return;
            for(int i = 0; i < m_Cards.Count; i++) {
                var card = m_Cards[i];
                card.SetData(m_CardDatas[UCL.Core.MathLib.UCL_Random.Instance.Next(m_CardDatas.Count)]);
            }
        }
    }
}
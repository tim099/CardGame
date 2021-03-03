using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG
{
    public class RCG_CardRowsDisplayUI : MonoBehaviour
    {
        public List<RCG_CardDisplayer> m_CardDisplayer = new List<RCG_CardDisplayer>();
        public void Show(List<RCG_CardData> cards, int s_id, int count) {
            gameObject.SetActive(true);
            for(int i = 0;i < m_CardDisplayer.Count; i++) {
                var aCard = m_CardDisplayer[i];
                if(i < count) {
                    aCard.gameObject.SetActive(true);
                    int at = s_id + i;
                    if(at < cards.Count) {
                        aCard.Init(cards[at]);
                    }
                } else {
                    aCard.gameObject.SetActive(false);
                }
            }
        }
        public void Hide() {
            gameObject.SetActive(false);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RCG
{
    public class RCG_ItemRowsDisplayUI : MonoBehaviour
    {
        public List<RCG_ItemDisplayer> m_ItemDisplayer = new List<RCG_ItemDisplayer>();
        public void Show(List<RCG_ItemData> cards, int s_id, int count)
        {
            gameObject.SetActive(true);
            for (int i = 0; i < m_ItemDisplayer.Count; i++)
            {
                var aItem = m_ItemDisplayer[i];
                if (i < count)
                {
                    aItem.gameObject.SetActive(true);
                    int at = s_id + i;
                    if (at < cards.Count)
                    {
                        aItem.Init(cards[at]);
                    }
                }
                else
                {
                    aItem.gameObject.SetActive(false);
                }
            }
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
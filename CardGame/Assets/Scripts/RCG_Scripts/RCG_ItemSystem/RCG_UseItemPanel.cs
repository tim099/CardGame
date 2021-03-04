using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG
{
    public class RCG_UseItemPanel : MonoBehaviour
    {
        [SerializeField] RCG_ItemDisplayer m_ItemDisplayer = null;
        RCG_Item m_Item = null;
        public void Init()
        {
            gameObject.SetActive(false);
        }
        public void Show(RCG_Item iItem)
        {
            m_Item = iItem;
            m_ItemDisplayer.SetItem(m_Item);
            gameObject.SetActive(true);
        }
        public void UseItem()
        {
            if (m_Item == null) return;
            m_Item.Use(delegate() { 
            
            });
            Hide();
        }
        public void Hide()
        {
            m_Item = null;
            gameObject.SetActive(false);
        }
    }
}
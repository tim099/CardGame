using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG
{
    public class RCG_ItemUI : MonoBehaviour
    {
        [SerializeField] protected RCG_ShowItemUI m_ShowItemUI = null;
        [SerializeField] protected RCG_UseItemPanel m_UseItemPanel = null;
        public void Init()
        {
            m_ShowItemUI.Init(OnItemSelected);
            m_UseItemPanel.Init();
        }
        public void OnItemSelected(RCG_Item iItem)
        {
            //Debug.LogError("OnItemSelected:" + iItem.ItemData.ItemName);
            m_UseItemPanel.Show(iItem);
            Hide();
        }
        public void Toggle()
        {
            if (m_ShowItemUI.IsShowing)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
        public void Show()
        {
            m_UseItemPanel.Hide();
            m_ShowItemUI.Show(RCG_DataService.ins.m_ItemsData.GetItems());
        }
        public void Hide()
        {
            m_ShowItemUI.Hide();
        }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG
{
    public class RCG_ItemUI : MonoBehaviour
    {
        [SerializeField] protected RCG_ShowItemUI m_ShowItemUI = null;
        public void Init()
        {
            m_ShowItemUI.Init();

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
            m_ShowItemUI.Show(RCG_DataService.ins.m_ItemsData.GetItems());
        }
        public void Hide()
        {
            m_ShowItemUI.Hide();
        }

    }
}
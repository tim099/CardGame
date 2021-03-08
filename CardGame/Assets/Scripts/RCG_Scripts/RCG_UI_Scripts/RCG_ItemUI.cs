using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG
{
    public class RCG_ItemUI : MonoBehaviour
    {
        public static RCG_ItemUI Ins = null;
        public bool IsShowing { get { return m_ShowItemUI.IsShowing; } }
        [SerializeField] protected RCG_ShowItemUI m_ShowItemUI = null;
        [SerializeField] protected RCG_UseItemPanel m_UseItemPanel = null;
        [SerializeField] protected UCL.Core.UI.UCL_Button m_ItemButton = null;
        public void Init()
        {
            Ins = this;
            m_ShowItemUI.Init(OnItemSelected);
            m_UseItemPanel.Init();
        }
        /// <summary>
        /// 封鎖物品使用
        /// </summary>
        public void BlockItem()
        {
            m_ItemButton.Interactable = false;
            if (IsShowing) Hide();
        }
        /// <summary>
        /// 解鎖物品使用
        /// </summary>
        public void UnBlockItem()
        {
            m_ItemButton.Interactable = true;
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
            RCG_Player.Ins.ClearSelectedCard();
            m_UseItemPanel.Hide();
            m_ShowItemUI.Show(RCG_DataService.ins.m_ItemsData.GetItems());
        }
        public void Hide()
        {
            m_ShowItemUI.Hide();
        }

    }
}
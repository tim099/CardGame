using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCL.TweenLib;
using UnityEngine.UI;

namespace RCG
{
    /// <summary>
    /// 物品欄介面
    /// </summary>
    public class RCG_ShowItemUI : MonoBehaviour
    {
        public bool IsShowing { get; protected set; } = false;
        public Transform m_HidePos;
        public ScrollRect m_ScrollRect;
        
        [SerializeField] RCG_ItemRowsDisplayUI m_RowsDisplayUITmp = null;
        List<RCG_ItemRowsDisplayUI> m_RowsDisplayUIs = new List<RCG_ItemRowsDisplayUI>();
        Vector3 m_ShowPos = Vector3.zero;
        System.Action<RCG_Item> m_SelectItemAction = null;
        public void Init(System.Action<RCG_Item> iSelectItemAction)
        {
            m_SelectItemAction = iSelectItemAction;
            m_RowsDisplayUITmp.Hide();
            AddItemRowsDisplayUI(m_RowsDisplayUITmp);
            m_ShowPos = transform.position;
            gameObject.SetActive(false);
        }
        void AddItemRowsDisplayUI(RCG_ItemRowsDisplayUI iItemRowsDisplayUI)
        {
            iItemRowsDisplayUI.Init(OnItemSelected);
            m_RowsDisplayUIs.Add(iItemRowsDisplayUI);
        }
        public void OnItemSelected(RCG_Item iItem)
        {
            //Debug.LogError("OnItemSelected:" + iItem.ItemData.ItemName);
            m_SelectItemAction?.Invoke(iItem);
        }
        public void Show(List<RCG_Item> iItems)
        {
            if (IsShowing) return;
            IsShowing = true;
            int card_num_in_row = m_RowsDisplayUITmp.m_ItemDisplayer.Count;
            while (iItems.Count > m_RowsDisplayUIs.Count * card_num_in_row)
            {
                var aNewRow = Instantiate(m_RowsDisplayUITmp, m_RowsDisplayUITmp.transform.parent);
                aNewRow.name = m_RowsDisplayUITmp.name + "_" + m_RowsDisplayUIs.Count;
                AddItemRowsDisplayUI(aNewRow);
            }
            for (int i = 0; i < m_RowsDisplayUIs.Count; i++)
            {
                var aRow = m_RowsDisplayUIs[i];
                int count = i * card_num_in_row;
                if (count < iItems.Count - card_num_in_row)
                {
                    aRow.Show(iItems, count, card_num_in_row);
                }
                else if (count < iItems.Count)
                {
                    aRow.Show(iItems, count, iItems.Count - count);
                }
                else
                {
                    aRow.Hide();
                }
            }
            GetComponent<RectTransform>().ForceRebuildLayoutImmediate();

            gameObject.SetActive(true);
            transform.position = m_HidePos.position;
            transform.localScale = new Vector3(0.3f, 0.3f, 1f);
            m_ScrollRect.ToTop();
            var aTweener = transform.UCL_Scale(0.3f, 1f).SetEase(EaseType.OutElastic);
            aTweener.AddComponent(transform.TC_Move(m_ShowPos));
            aTweener.OnComplete(() =>
            {
                m_ScrollRect.ToTop();
            });
            aTweener.Start();

        }
        public void Hide()
        {
            if (!IsShowing) return;
            IsShowing = false;
            transform.position = m_ShowPos;
            transform.localScale = Vector3.one;
            var twn = transform.UCL_Scale(0.1f, 0f).SetEase(EaseType.OutQuint);
            twn.AddComponent(transform.TC_Move(m_HidePos));
            twn.OnComplete(() => {
                gameObject.SetActive(false);
                transform.position = m_ShowPos;
            });
            twn.Start();
        }
    }
}
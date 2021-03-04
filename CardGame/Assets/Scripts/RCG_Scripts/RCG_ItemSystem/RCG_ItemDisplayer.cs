using System.Collections;
using System.Collections.Generic;
using UCL.Core.UI;
using UnityEngine;
using UnityEngine.UI;
namespace RCG
{
    public class RCG_ItemDisplayer : MonoBehaviour
    {
        [SerializeField] Image m_IconImage = null;
        [SerializeField] Text m_NameText = null;
        [SerializeField] Text m_DescriptionText = null;
        [SerializeField] GameObject m_Description = null;
        [SerializeField] UCL_Button m_Button = null;
        protected RCG_Item m_Data = null;
        virtual public void Init(System.Action<RCG_Item> iOnClikAction)
        {
            m_Button.m_OnClick.AddListener(delegate ()
            {
                iOnClikAction.Invoke(m_Data);
            });
        }
        virtual public void SetItem(RCG_Item _Data)
        {
            m_Data = _Data;
            if (m_Data != null)
            {
                gameObject.SetActive(true);
                m_IconImage.SetSprite(m_Data.ItemData.Icon);
                m_NameText.SetText(m_Data.ItemData.ItemName);
                UpdateDiscription();
                ShowDescription();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        virtual public void UpdateDiscription()
        {
            if (m_Data == null) return;
            if (m_DescriptionText != null)
            {
                m_DescriptionText.SetText(m_Data.ItemData.Description);
            }
        }
        virtual public void ShowDescription()
        {
            m_Description.SetActive(true);
        }
        virtual public void HideDescription()
        {
            m_Description.SetActive(false);
        }
    }
}
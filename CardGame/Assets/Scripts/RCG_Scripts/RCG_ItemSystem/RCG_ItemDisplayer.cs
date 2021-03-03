using System.Collections;
using System.Collections.Generic;
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
        protected RCG_ItemData m_Data = null;
        virtual public void Init(RCG_ItemData _Data)
        {
            m_Data = _Data;
            if (m_Data != null)
            {
                gameObject.SetActive(true);
                m_IconImage.SetSprite(m_Data.Icon);
                m_NameText.SetText(m_Data.ItemName);
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
                m_DescriptionText.SetText(m_Data.Description);
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
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UCL.Core;
using UCL.Core.UI;
using UnityEngine;
using UnityEngine.UI;
using UCL.TweenLib;
namespace RCG
{
    public class RCG_CardDisplayer : MonoBehaviour
    {
        public UCL.Core.UCL_Event m_OnSelected;
        public bool IsSelected { get { return m_Selected; } }

        public UCL_TB_Tweener m_ScaleUpTB;
        public UCL_TB_Tweener m_ScaleBackTB;
        public Image m_Image;
        public Text m_NameText;
        public GameObject m_Description;
        public Text m_DescriptionText;
        public Text m_CostText;
        protected RCG_CardData m_Data;
        protected bool m_Selected = false;
        protected bool m_BlockSelection = false;
        virtual public void Init(RCG_CardData _Data) {
            m_Data = _Data;
            if(m_Data != null) {
                gameObject.SetActive(true);
                m_Image.SetSprite(m_Data.Icon);
                m_NameText.SetText(m_Data.CardName);
                m_DescriptionText.SetText(m_Data.Description);
                m_CostText.SetText(m_Data.m_Cost);
                HideDescription();
            } else {
                gameObject.SetActive(false);
            }
        }
        virtual public void BlockSelection() {
            DeSelect();
            m_BlockSelection = true;
        }
        virtual public void UnBlockSelection() {
            m_BlockSelection = false;
        }

        virtual public void Select() {
            if(m_BlockSelection) return;
            m_Selected = true;
            ShowDescription();
            m_ScaleBackTB.Kill();
            m_ScaleUpTB.StartTween();
            m_OnSelected.UCL_Invoke();
        }

        virtual public void DeSelect() {
            if(m_BlockSelection) return;
            m_Selected = false;
            HideDescription();
            m_ScaleUpTB.Kill();
            m_ScaleBackTB.StartTween();
        }
        virtual public void ShowDescription() {
            m_Description.SetActive(true);
        }
        virtual public void HideDescription() {
            m_Description.SetActive(false);
        }
    }
}
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

        public UCL_TB_Tweener m_ScaleUpTB = null;
        public UCL_TB_Tweener m_ScaleBackTB = null;
        public Image m_Image = null;
        public Text m_NameText = null;
        public GameObject m_Description = null;
        public Text m_DescriptionText = null;
        public Text m_CostText = null;
        public UCL_OnPointerEvent m_OnPointerEvent = null;
        public List<Image> m_RequireSkillImages = new List<Image>();
        protected RCG_CardData m_Data;
        protected bool m_Selected = false;
        protected bool m_BlockSelection = false;
        virtual public void Init(RCG_CardData _Data) {
            m_Data = _Data;
            if(m_Data != null) {
                gameObject.SetActive(true);
                m_Image.SetSprite(m_Data.Icon);
                m_NameText.SetText(m_Data.CardName);
                UpdateCardDiscription();
                int cost = m_Data.Cost;
                if(cost < 0) cost = 0;
                m_CostText.SetText(cost);
                var aRequireSkill = m_Data.RequireSkills;
                for (int i = 0; i < m_RequireSkillImages.Count; i++)
                {
                    var aImg = m_RequireSkillImages[i];
                    if (i < aRequireSkill.Count)
                    {
                        aImg.gameObject.SetActive(true);
                        aImg.sprite = RCG_CardDataService.ins.GetUnitSkillSprite(aRequireSkill[i]);
                    }
                    else
                    {
                        aImg.gameObject.SetActive(false);
                    }
                }
                //HideDescription();
                ShowDescription();
            } else {
                gameObject.SetActive(false);
            }
        }
        virtual public void OnPointerEnter(System.Action iAction) {
            m_OnPointerEvent.m_OnPointerEnter.AddListener(delegate() { iAction.Invoke(); });
        }
        virtual public void OnPointerExit(System.Action iAction) {
            m_OnPointerEvent.m_OnPointerExit.AddListener(delegate () { iAction.Invoke(); });
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
            //ShowDescription();
            m_ScaleBackTB.Kill();
            m_ScaleUpTB.StartTween();
            m_OnSelected.UCL_Invoke();
        }

        virtual public void DeSelect() {
            if(m_BlockSelection) return;
            m_Selected = false;
            //HideDescription();
            m_ScaleUpTB.Kill();
            m_ScaleBackTB.StartTween();
        }
        virtual public void UpdateCardDiscription()
        {
            if (m_Data == null) return;
            if (m_DescriptionText != null)
            {
                m_DescriptionText.SetText(m_Data.Description);
            }
        }
        virtual public void ShowDescription() {
            m_Description.SetActive(true);
        }
        virtual public void HideDescription() {
            m_Description.SetActive(false);
        }
    }
}
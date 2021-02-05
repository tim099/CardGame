using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UCL.TweenLib;
namespace RCG
{
    [UCL.Core.ATTR.EnableUCLEditor]
    public class RCG_CostUI : MonoBehaviour
    {
        public int Cost { get; protected set; } = 0;
        
        [SerializeField] protected Text m_CostText = null;
        [SerializeField] protected GameObject m_CostUpEffect = null;
        [SerializeField] protected GameObject m_CostDownEffect = null;
        [SerializeField] protected float m_AnimTime = 0.3f;
        [SerializeField] protected EaseType m_AnimEase = EaseType.InSin;
        protected UCL_Tweener m_AnimTween = null;
        protected int m_DisplayCost = 0;
        private void Awake()
        {
            m_CostUpEffect.SetActive(false);
            m_CostDownEffect.SetActive(false);
        }
        public void SetCost(int iCost)
        {
            Cost = iCost;
            SetDisplayCost(Cost);
        }
        public void SetDisplayCost(int iCost)
        {
            m_DisplayCost = iCost;
            m_CostText.text = iCost.ToString();
        }
        [UCL.Core.ATTR.UCL_FunctionButton("AlterCost +1", 1)]
        public bool AlterCost(int iAlter)
        {
            if (iAlter == 0) return true;
            if (Cost + iAlter < 0) return false;
            if (m_AnimTween != null)
            {
                m_AnimTween.Kill(true);
            }
            //if (iAlter > 0)
            {
                if (iAlter > 0)
                {
                    m_CostUpEffect.SetActive(false);
                    m_CostUpEffect.SetActive(true);
                }
                else
                {
                    m_CostDownEffect.SetActive(false);
                    m_CostDownEffect.SetActive(true);
                }

                Cost += iAlter;
                m_AnimTween = LibTween.Tweener(m_AnimTime);
                m_AnimTween.SetEase(m_AnimEase);
                var aPosO = m_CostText.transform.position;
                bool aUpdated = false;
                m_AnimTween.OnComplete(delegate ()
                {
                    //m_CostText.transform.localRotation = Quaternion.identity;
                    m_CostText.transform.position = aPosO;
                    m_CostText.transform.localScale = Vector3.one;
                    if (!aUpdated)
                    {
                        SetDisplayCost(Cost);
                    }
                    m_AnimTween = null;
                });
                m_AnimTween.AddComponent(m_CostText.transform.TC_MoveBy(0, 50, 0));
                m_AnimTween.AddComponent(m_CostText.transform.TC_Scale(1.5f, 1.5f, 1f));
                m_AnimTween.OnUpdate(delegate (float y)
                {
                    if (!aUpdated && m_AnimTween.GetProgress() > 0.4f)
                    {
                        SetDisplayCost(Cost);
                        aUpdated = true;
                    }
                    //Debug.LogWarning(m_CostText.transform.localPosition);
                });
                m_AnimTween.SetBackfolding(true);
                m_AnimTween.Start();
            }
            //else
            //{
            //    SetCost(Cost + iAlter);
            //}
            return true;
        }
    }
}


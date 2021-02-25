using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RCG
{
    public class RCG_UnitUI : MonoBehaviour
    {
        public GameObject m_SelectedItem = null;
        public GameObject m_HitEffect = null;
        public RCG_StatusEffectUI m_StatusEffectUI = null;
        protected RCG_Unit p_Unit = null;
        virtual public void Init(RCG_Unit iUnit)
        {
            p_Unit = iUnit;
            m_SelectedItem.SetActive(false);
            m_HitEffect.SetActive(false);
            m_StatusEffectUI.Init(p_Unit);
        }
        virtual public void Hit()
        {
            m_HitEffect.SetActive(false);
            m_HitEffect.SetActive(true);
        }
        virtual public void TurnEnd()
        {
            m_StatusEffectUI.TurnEnd();
        }
    }
}


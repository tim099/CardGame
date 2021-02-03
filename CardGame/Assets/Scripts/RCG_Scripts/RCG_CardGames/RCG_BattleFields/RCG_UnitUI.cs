using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RCG
{
    public class RCG_UnitUI : MonoBehaviour
    {
        public GameObject m_SelectedItem = null;
        public GameObject m_HitEffect = null;
        virtual public void Init()
        {
            m_SelectedItem.SetActive(false);
            m_HitEffect.SetActive(false);
        }
        virtual public void Hit()
        {
            m_HitEffect.SetActive(false);
            m_HitEffect.SetActive(true);
        }
    }
}


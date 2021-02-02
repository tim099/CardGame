using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RCG
{
    public class RCG_UnitUI : MonoBehaviour
    {
        public GameObject m_SelectedItem = null;
        virtual public void Init()
        {
            m_SelectedItem.SetActive(false);
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG
{
    public class RCG_VFX : MonoBehaviour
    {
        public UnityEngine.Events.UnityEvent m_ShowEvents;
        bool m_Created = false;
        virtual public void Init()
        {
            gameObject.SetActive(false);
        }
        virtual protected void OnDisable()
        {
            DeleteVFX();
        }
        virtual public void ShowVFX()
        {
            if (m_Created) return;
            m_Created = true;
            gameObject.SetActive(true);
            m_ShowEvents.Invoke();
        }
        /// <summary>
        /// 被回收時會呼叫
        /// </summary>
        virtual public void HideVFX()
        {
            gameObject.SetActive(false);
        }
        /// <summary>
        /// Recycle VFX!!
        /// </summary>
        virtual public void DeleteVFX()
        {
            if (!m_Created) return;
            m_Created = false;
            RCG_VFXManager.ins.DeleteVFX(this);
        }
    }
}
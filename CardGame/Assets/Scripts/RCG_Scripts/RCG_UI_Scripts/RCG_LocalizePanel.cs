using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCL.TweenLib;
namespace RCG {
    public class RCG_LocalizePanel : MonoBehaviour {
        bool m_Show = false;
        UCL_Tweener m_Tweener = null;
        public void Init() {
            gameObject.SetActive(false);
        }
        public void Toggle() {
            if(m_Show) Hide();
            else Show();
        }
        public void Show() {
            m_Show = true;
            if(m_Tweener != null) {
                m_Tweener.Kill();
            }
            gameObject.SetActive(true);
            transform.localScale = new Vector3(0.01f, 0.01f, 01f);
            m_Tweener = transform.UCL_Scale(0.3f, Vector3.one);
            m_Tweener.SetEase(EaseType.OutQuart);
            m_Tweener.OnComplete(delegate () {
                m_Tweener = null;
                transform.localScale = Vector3.one;
            });
            m_Tweener.Start();
        }
        public void Hide() {
            m_Show = false;
            if(m_Tweener != null) {
                m_Tweener.Kill();
            }
            gameObject.SetActive(true);
            transform.localScale = new Vector3(1f, 1f, 1f);
            m_Tweener = transform.UCL_Scale(0.3f, new Vector3(0.01f, 0.01f, 1f));
            m_Tweener.SetEase(EaseType.OutQuart);
            m_Tweener.OnComplete(delegate () {
                m_Tweener = null;
                gameObject.SetActive(false);
            });
            m_Tweener.Start();
        }
    }
}



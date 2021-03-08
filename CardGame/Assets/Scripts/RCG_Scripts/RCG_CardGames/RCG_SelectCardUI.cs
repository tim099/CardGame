using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RCG {
    public class RCG_SelectCardUI : MonoBehaviour
    {
        [SerializeField] protected Button m_SelectCardConfirmButton = null;
        [SerializeField] protected Text m_SelectCardDescriptionText = null;
        virtual public void Init()
        {
            m_SelectCardConfirmButton.onClick.AddListener(ConfirmSelected);
            gameObject.SetActive(false);
        }
        virtual protected void ConfirmSelected()
        {
            if (!RCG_Player.Ins.ConfirmSelectedCard())
            {
                return;
            }
            Hide();
        }
        virtual public void Show(string iSelectDescription)
        {
            gameObject.SetActive(true);
            m_SelectCardConfirmButton.gameObject.SetActive(true);
            m_SelectCardDescriptionText.text = iSelectDescription;
        }
        virtual public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}



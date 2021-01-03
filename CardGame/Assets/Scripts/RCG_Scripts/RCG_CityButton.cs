using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RCG
{
    public class RCG_CityButton : MonoBehaviour
    {
        public string m_CityName;
        public Text m_CityNameText;
        public Button m_CityButton;
        RCG_BigMap p_Map;
        virtual public void Init(RCG_BigMap _Map) {
            p_Map = _Map;
            m_CityNameText.text = m_CityName;
            m_CityButton.onClick.AddListener(delegate () {
                p_Map.SelectCity(m_CityName);
            });
        }
    }
}
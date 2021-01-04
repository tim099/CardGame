using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RCG {
    public class RCG_EmbarkUI : MonoBehaviour {
        public static RCG_EmbarkUI ins = null;
        public Text m_SelectedCityNameText = null;
        public UCL.SceneLib.UCL_SceneLoader m_SceneLoader;
        protected string m_SelectedCityName = "";
        virtual public void Init() {
            ins = this;
            Hide();
        }
        virtual public void Show(string selected_city_name) {
            gameObject.SetActive(true);
            m_SelectedCityName = selected_city_name;
            m_SelectedCityNameText.text = selected_city_name;
        }
        virtual public void Hide() {
            gameObject.SetActive(false);
        }
        virtual public void Embark() {
            m_SceneLoader.Load();
        }
    }
}
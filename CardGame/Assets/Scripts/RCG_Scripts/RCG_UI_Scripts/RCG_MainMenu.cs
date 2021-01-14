using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RCG {
    public class RCG_MainMenu : MonoBehaviour {
        public RCG_LocalizePanel m_LocalizePanel;
        public Button[] m_Buttons;
        // Start is called before the first frame update
        void Start() {
            m_LocalizePanel.Init();
            if(m_Buttons.Length > 0) {
                m_Buttons[0].Select();
            }
        }

        // Update is called once per frame
        void Update() {

        }

        public void SetLocalize(string lang) {
            Debug.LogWarning("SetLocalize:" + lang);
            UCL.Core.Game.UCL_LocalizeService.ins.LoadLanguage(lang);
            m_LocalizePanel.Hide();
        }
    }
}


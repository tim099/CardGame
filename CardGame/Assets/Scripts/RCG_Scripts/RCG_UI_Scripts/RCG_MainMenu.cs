using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG {
    public class RCG_MainMenu : MonoBehaviour {
        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        public void SetLocalize(string lang) {
            Debug.LogWarning("SetLocalize:" + lang);
            UCL.Core.Game.UCL_LocalizeService.ins.LoadLanguage(lang);
        }
    }
}


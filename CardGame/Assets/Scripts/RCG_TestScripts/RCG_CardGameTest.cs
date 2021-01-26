using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RCG {
    public class RCG_CardGameTest : MonoBehaviour {
        public RCG_CardGame m_Game;
        public Button m_DeselectButton;
        public bool m_AutoInit = true;
        bool m_Inited = false;
        private void Start() {
            if(m_AutoInit) Init();
        }
        public void TriggerCard() {
            m_Game.TriggerCard();
        }
        public void Init() {
            if(m_Inited) return;
            m_Inited = true;
            m_Game.Init();
            m_DeselectButton.onClick.AddListener(delegate () {
                m_Game.m_Player.SetSelectedCard(null);
            });
        }

    }
}
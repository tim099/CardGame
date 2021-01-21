using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG {
    public class RCG_QuestLocationManager : MonoBehaviour
    {
        // Start is called before the first frame update
        public Transform m_quest_hover_panel;
        void Start()
        {
            m_quest_hover_panel.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void showHoverPanel(Transform pos) {
            m_quest_hover_panel.position = pos.position + Vector3.up * 5;
            m_quest_hover_panel.gameObject.SetActive(true);
        }

        public void hideHoverPanel() {
            m_quest_hover_panel.gameObject.SetActive(false);
        }
    }
}

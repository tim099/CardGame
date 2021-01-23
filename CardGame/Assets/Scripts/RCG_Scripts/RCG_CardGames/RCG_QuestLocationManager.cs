using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            m_quest_hover_panel.SetParent(pos);
            m_quest_hover_panel.position = pos.position + Vector3.up * 8;
            m_quest_hover_panel.gameObject.SetActive(true);
            m_quest_hover_panel.GetComponentInChildren<Text>().text = pos.gameObject.GetComponentInChildren<RCG_CityButton>().m_CityNameText.text;
            pos.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 150);
            m_quest_hover_panel.GetComponent<Button>().onClick = pos.gameObject.GetComponentInChildren<Button>().onClick;
        }

        public void hideHoverPanel(Transform pos) {
            pos.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
            m_quest_hover_panel.gameObject.SetActive(false);
        }
    }
}

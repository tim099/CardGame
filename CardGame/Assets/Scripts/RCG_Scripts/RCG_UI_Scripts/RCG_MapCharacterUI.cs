using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RCG { 
    public class RCG_MapCharacterUI : MonoBehaviour
    {
        private List<RCG_CharacterData> m_CharacterDatas;
        public Transform m_character_panel;
        private List<Transform> m_panels;
        // Start is called before the first frame update
        void Start()
        {
            m_panels = new List<Transform>();
            LoadCharacterData();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void ClearCharacterData()
        {
            foreach (var panel in m_panels)
            {
                Destroy(panel);
            }
            m_panels = new List<Transform>();
        }

        public void LoadCharacterData()
        {
            Debug.Log("LoadCharacterData");
            ClearCharacterData();

            m_CharacterDatas = RCG_GameManager.ins.GetDataService().m_CharacterDatas;
            foreach (var character_data in m_CharacterDatas)
            {
                var new_panel = Instantiate(m_character_panel, transform);
                new_panel.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/" + character_data.m_character_name);
                m_panels.Add(new_panel);
            }
        }
    }
}

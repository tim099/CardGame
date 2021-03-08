using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG
{
    public class RCG_CharacterUI : MonoBehaviour
    {
        public RCG_CharacterDetailPanel m_DetailPanel;
        public RCG_CharacterPanel m_CharacterPanel;
        bool m_Showed = true;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (RCG_BattleManager.Ins) { //can be better QWQ
                if (m_Showed && RCG_BattleManager.Ins.gameObject.activeInHierarchy)
                {
                    m_DetailPanel.gameObject.SetActive(false);
                    hidePanels();
                    m_Showed = false;
                }
                else if (!m_Showed && !RCG_BattleManager.Ins.gameObject.activeInHierarchy)
                {
                    m_DetailPanel.gameObject.SetActive(false);
                    showPanels();
                    m_Showed = true;
                }
            }
        }

        void Awake()
        {
            InitUI();
            m_DetailPanel.gameObject.SetActive(false);
        }

        public void ShowDetail(RCG_CharacterData data)
        {
            m_DetailPanel.SetCharacterData(data);
            m_DetailPanel.Show();
        }

        public void InitUI()
        {
            foreach (var character_data in RCG_DataService.ins.m_CharacterDatas)
            {
                var panel = Instantiate(m_CharacterPanel, transform);
                panel.GetComponent<RCG_CharacterPanel>().Init(character_data);
            }
        }

        public void hidePanels()
        {
            foreach (Transform child in transform) { 
                child.gameObject.SetActive(false);
            }
        }

        public void showPanels()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}

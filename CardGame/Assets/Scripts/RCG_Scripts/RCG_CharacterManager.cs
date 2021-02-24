using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG
{
    public class RCG_CharacterManager : MonoBehaviour
    {
        public static RCG_CharacterManager ins = null;
        public RCG_CharacterUI m_CharacterUI;
        public RCG_CharacterPanel m_CharacterPanel;

        List<RCG_CharacterData> m_CharacterDatas;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void Awake()
        {
            ins = this;
        }

        public void InitUI(RCG_CharacterUI ui)
        {
            m_CharacterDatas = RCG_DataService.ins.m_CharacterDatas;
            foreach (var character_data in m_CharacterDatas)
            {
                var panel = Instantiate(m_CharacterPanel, ui.transform);
                panel.GetComponent<RCG_CharacterPanel>().Init(character_data);
            }
        }
    }
}

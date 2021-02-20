using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG
{
    public class RCG_CharacterUI : MonoBehaviour
    {
        public RCG_CharacterDetailPanel m_DetailPanel;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void Awake()
        {
            m_DetailPanel.gameObject.SetActive(false);
        }

        public void ShowDetail(RCG_CharacterData data)
        {
            m_DetailPanel.SetCharacterData(data);
            m_DetailPanel.Show();
        }
    }
}

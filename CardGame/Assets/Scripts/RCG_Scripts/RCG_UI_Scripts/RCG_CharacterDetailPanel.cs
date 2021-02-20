using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RCG
{
    public class RCG_CharacterDetailPanel : MonoBehaviour
    {
        public Text m_CharacterNameText;
        public Image m_CharacterProtrait;
        RCG_CharacterData m_CharacterData;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetCharacterData(RCG_CharacterData data)
        {
            m_CharacterData = data;
            m_CharacterNameText.text = data.m_character_name;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}

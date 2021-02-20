using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RCG
{
    public class RCG_CharacterPanel : MonoBehaviour
    {
        public Image m_characterAvatarImage;
        RCG_CharacterData m_CharacterData;

        // Start is called before the first frame update
        void Start(){ }

        // Update is called once per frame
        void Update(){ }

        void Awake() { }

        public void Init(RCG_CharacterData data)
        {
            m_CharacterData = data;
            m_characterAvatarImage.sprite = Resources.Load<Sprite>("Sprites/Character/" + data.m_character_name + "/avatar");
        }

        public void OnClick()
        {
            GetComponentInParent<RCG_CharacterUI>().ShowDetail(m_CharacterData);
        }
    }
}

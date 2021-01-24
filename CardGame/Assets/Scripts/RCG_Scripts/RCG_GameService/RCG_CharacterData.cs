using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG { 
    public class RCG_CharacterData
    {
        public string m_character_name;
        public int m_max_hp;
        public int m_hp;
        public int m_level;

        public RCG_CharacterData(string name, int max_hp, int hp, int level)
        {
            m_character_name = name;
            m_max_hp = max_hp;
            m_hp = hp;
            m_level = level;
        }
    }
}
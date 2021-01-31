using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG
{
    public class RCG_DeckData
    {

    }

    public class RCG_QuestData
    {
        public string m_location;
        public string[] m_avilible_quests; 
    }

    public class RCG_DataService : UCL.Core.Game.UCL_GameService
    {
        public RCG_DataService ins = null;
        public List<RCG_CharacterData> m_all_character_data;
        public override void Init() {
            base.Init();
            ins = this;

            m_all_character_data = new List<RCG_CharacterData>();
            m_all_character_data.Add(new RCG_CharacterData("Trist", 20, 20, 1));
        }
    }
}
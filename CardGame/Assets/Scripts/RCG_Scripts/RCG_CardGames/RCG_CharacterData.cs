using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace RCG
{
    /// <summary>
    /// 玩家角色資料
    /// </summary>
    [System.Serializable]
    public class RCG_CharacterData
    {
        public string m_character_name;
        public int m_max_hp;
        public int m_hp;
        public bool m_in_party;
        public UnitPos m_battle_position;

        public static string CardDataPath
        {
            get
            {
                return Application.streamingAssetsPath + "/" + CardDataRelativePath;
            }
        }
        public static string CardDataRelativePath
        {
            get
            {
                return "CharacterDatas/Datas";
            }
        }
        public static UCL.Core.JsonLib.JsonData GetCharacterJsonData(string character_name)
        {
            string character_file_name = character_name + ".json";
            string data = BetterStreamingAssets.ReadAllText(Path.Combine(CardDataRelativePath, character_file_name));
            //File.ReadAllText(Path.Combine(CardDataPath, character_file_name));
            var json = UCL.Core.JsonLib.JsonData.ParseJson(data);
            return json;
        }

        public RCG_CharacterData() : this("Dummy") { }

        public RCG_CharacterData(string character_name) : this(GetCharacterJsonData(character_name)) { }

        public RCG_CharacterData(UCL.Core.JsonLib.JsonData character_json_data)
        {
            m_character_name = character_json_data.Get("m_CharacterName").GetString();
            m_max_hp = character_json_data.Get("m_MaxHp").GetInt();
            m_hp = character_json_data.Get("m_Hp").GetInt();
            switch (character_json_data.Get("m_BattlePosition").GetString())
            {
                case "Front":
                    m_battle_position = UnitPos.Front; break;
                case "Back":
                    m_battle_position = UnitPos.Back; break;
                default:
                    m_battle_position = UnitPos.Front; break;
            }
        }

    }
}

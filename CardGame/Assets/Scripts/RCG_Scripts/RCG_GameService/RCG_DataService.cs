using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UCL.Core.JsonLib;
namespace RCG
{
    #region DeckData
    /// <summary>
    /// 玩家牌組資料
    /// </summary>
    [System.Serializable]
    public class RCG_DeckData : IJsonSerializable
    {
        public static string DeckDataPath
        {
            get
            {
                return Application.streamingAssetsPath + "/.CardDatas/Deck.json";
            }
        }
        public static RCG_DeckData LoadDeckData()
        {
            var aDeckData = new RCG_DeckData();
            //var aDeckPath = DeckDataPath;
            aDeckData.LoadData(DeckDataPath);
            //if (File.Exists(aDeckPath))
            //{
            //    var aJson = File.ReadAllText(aDeckPath);
            //    JsonData aData = JsonData.ParseJson(aJson);
            //    aDeckData.DeserializeFromJson(aData);
            //}
            return aDeckData;
        }
        public void SaveData(string iDeckPath)
        {
            File.WriteAllText(iDeckPath, SerializeToJson().ToJsonBeautify());
        }
        public void LoadData(string iDeckPath)
        {
            if (File.Exists(iDeckPath))
            {
                var aJson = File.ReadAllText(iDeckPath);
                JsonData aData = JsonData.ParseJson(aJson);
                DeserializeFromJson(aData);
            }
        }
        public int CardCount
        {
            get
            {
                int aCount = 0;
                foreach (var aCard in m_Cards)
                {
                    aCount += aCard.m_CardCount;
                }
                return aCount;
            }
        }
        public JsonData SerializeToJson()
        {
            JsonData aData = new JsonData();

            aData["Cards"] = JsonConvert.SaveDataToJson(m_Cards);
            return aData;
        }
        public void DeserializeFromJson(JsonData iData)
        {
            if (iData.Contains("Cards"))
            {
                JsonConvert.LoadDataFromJson(m_Cards, iData["Cards"]);
            }
        }
        [System.Serializable]
        public class CardData
        {
            public CardData()
            {

            }
            public string m_CardName = string.Empty;
            public int m_CardCount = 1;
        }
        public List<RCG_CardData> GetCardDatas()
        {
            List<RCG_CardData> aCardDatas = new List<RCG_CardData>();
            for (int i = 0; i < m_Cards.Count; i++)
            {
                var aCard = m_Cards[i];
                var aCardData = RCG_CardDataService.ins.GetCardData(aCard.m_CardName);
                for (int j = 0; j < aCard.m_CardCount; j++)
                {
                    aCardDatas.Add(aCardData);
                }
            }
            return aCardDatas;
        }
        /// <summary>
        /// Delete card from deck
        /// </summary>
        /// <param name="iCardName"></param>
        public void DeleteCard(string iCardName)
        {
            foreach (var aCard in m_Cards)
            {
                if (aCard.m_CardName == iCardName)
                {
                    m_Cards.Remove(aCard);
                    return;
                }
            }
        }
        /// <summary>
        /// Remove one card from deck
        /// </summary>
        /// <param name="iCardName"></param>
        public void RemoveCard(string iCardName)
        {
            foreach (var aCard in m_Cards)
            {
                if (aCard.m_CardName == iCardName)
                {
                    --aCard.m_CardCount;
                    if (aCard.m_CardCount <= 0)
                    {
                        m_Cards.Remove(aCard);
                    }
                    return;
                }
            }
        }
        /// <summary>
        /// Add one card into deck
        /// </summary>
        /// <param name="iCardName"></param>
        public void AddCard(string iCardName)
        {
            foreach (var aCard in m_Cards)
            {
                if (aCard.m_CardName == iCardName)
                {
                    ++aCard.m_CardCount;
                    return;
                }
            }
            CardData aCardData = new CardData();
            aCardData.m_CardName = iCardName;
            aCardData.m_CardCount = 1;
            m_Cards.Add(aCardData);
        }
        public List<CardData> m_Cards = new List<CardData>();
    }
    #endregion
    
    /// <summary>
    /// 用來管理遊戲開始後的所有資料
    /// </summary>
    public class RCG_DataService : UCL.Core.Game.UCL_GameService
    {
        const string DeckSaveName = "Deck.txt";
        const string InfoSaveName = "DataService.txt";
        protected string GameSavePath { get{
                return Path.Combine(RCG_GameManager.ins.GetGameFolderPath(),"Saves");
            } }
        static public RCG_DataService ins = null;
        public RCG_DeckData m_DeckData = new RCG_DeckData();
        public List<RCG_CharacterData> m_CharacterDatas;
        public override void Init() {
            base.Init();
            ins = this;

            //placeholder QWQ
            m_CharacterDatas.Add(new RCG_CharacterData("Knight"));
            m_CharacterDatas.Add(new RCG_CharacterData("Archer"));
        }
        /// <summary>
        /// 新的一局遊戲 重置資料
        /// </summary>
        public void NewGame()
        {

        }
        /// <summary>
        /// 存檔
        /// </summary>
        public void SaveGame(string iSavePath = "Save01")
        {
            var aDir = Path.Combine(GameSavePath, iSavePath);
            if (!Directory.Exists(aDir))
            {
                Directory.CreateDirectory(aDir);
            }
            {//Deck
                string aPath = Path.Combine(aDir, DeckSaveName);
                m_DeckData.SaveData(aPath);
                //JsonData aData = UCL.Core.JsonLib.JsonConvert.ObjectToJson(m_DeckData);
                //File.WriteAllText(aPath, aData.ToJson());
            }
        }
        /// <summary>
        /// 讀檔
        /// </summary>
        public void LoadGame(string iLoadPath = "Save01")
        {
            var aDir = Path.Combine(GameSavePath, iLoadPath);
            if (!Directory.Exists(aDir))
            {
                Debug.LogError("LoadGame Fail!!!Directory.Exists(" + aDir + ")");
                return;
            }
            {//Deck
                string aPath = Path.Combine(aDir, DeckSaveName);
                //string aJson = File.ReadAllText(aPath);
                //JsonData aData = JsonData.ParseJson(aJson);
                m_DeckData.LoadData(aPath);
                //m_DeckData = UCL.Core.JsonLib.JsonConvert.JsonToObject<RCG_DeckData>(aData);
                
            }
        }
        /// <summary>
        /// 儲存全局資料(非單局遊戲資料 解鎖 成就等...)
        /// </summary>
        /// <param name="dir"></param>
        public override void Save(string dir)
        {
            string aPath = Path.Combine(dir, InfoSaveName);
            JsonData aData = new JsonData();
            //aData["RCG_DeckData"] = UCL.Core.JsonLib.JsonConvert.ObjectToJson(m_DeckData);
            File.WriteAllText(aPath, aData.ToJson());
        }
        /// <summary>
        /// 讀取全局資料
        /// </summary>
        /// <param name="dir"></param>
        public override void Load(string dir)
        {
            //Init Deck
            m_DeckData = RCG_DeckData.LoadDeckData();
            //Debug.LogWarning("m_DeckData:" + m_DeckData.UCL_ToString());
            string aPath = Path.Combine(dir, InfoSaveName);
            if (!File.Exists(aPath))
            {
                return;
            }
            string aJson = File.ReadAllText(aPath);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace RCG {
    /// <summary>
    /// 讀取後儲存卡牌資料(從設定檔轉換成RCG_CardData)
    /// </summary>
    public class RCG_CardDataService : UCL.Core.Game.UCL_GameService {
        static public RCG_CardDataService ins = null;
        public int CardCount {
            get { return m_CardDatas.Count; }
        }

        public List<RCG_CardData> m_CardDatas = new List<RCG_CardData>();
        public Dictionary<string, RCG_CardData> m_CardDataDic = new Dictionary<string, RCG_CardData>();
        public Dictionary<string, Sprite> m_CardSprites = new Dictionary<string, Sprite>();
        //public string
        public override void Init() {
            base.Init();
            ins = this;
            LoadCardData();
        }
        public Sprite GetCardIcon(string iIconName) {
            if(m_CardSprites.ContainsKey(iIconName)) {
                return m_CardSprites[iIconName];
            }
            Sprite aIcon = null;
            var aIconPath = Path.Combine(RCG_CardData.CardIconPath, iIconName);
            if(File.Exists(aIconPath)) {
                var aData = File.ReadAllBytes(aIconPath);
                Texture2D aTexture = new Texture2D(1, 1);
                aTexture.LoadImage(aData);
                var aSprite = Sprite.Create(aTexture, new Rect(0.0f, 0.0f, aTexture.width, aTexture.height),
                    new Vector2(0.5f, 0.5f), 100.0f);
                m_CardSprites.Add(iIconName, aSprite);
                aIcon = aSprite;
                var aImg = UCL.Core.GameObjectLib.Create<Image>(iIconName, transform);
                aImg.sprite = aIcon;
            } else {
                Debug.LogError("GetCardIcon:" + iIconName + ",File not found!!aIconPath:" + aIconPath);
            }

            return aIcon;
        }

        public RCG_CardData GetCardData(int iID) {
            if(iID < 0 || iID >= m_CardDatas.Count) return null;
            return m_CardDatas[iID];
        }
        public RCG_CardData GetCardData(string iCardName) {
            if(!m_CardDataDic.ContainsKey(iCardName)) return null;
            return m_CardDataDic[iCardName];
        }
        public void LoadCardData() {
            m_CardDatas.Clear();
            var aFiles = UCL.Core.FileLib.Lib.GetFiles(RCG_CardData.CardDataPath);
            foreach(var aFile in aFiles) {
                //Debug.LogWarning("LoadData");
                string aData = System.IO.File.ReadAllText(aFile);
                RCG_CardData aCardData = new RCG_CardData(aData);
                m_CardDatas.Add(aCardData);
                m_CardDataDic.Add(UCL.Core.FileLib.Lib.RemoveFileExtension(aFile), aCardData);
            }
        }
    }
}
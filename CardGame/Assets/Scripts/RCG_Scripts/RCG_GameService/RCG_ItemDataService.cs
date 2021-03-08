using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
namespace RCG
{
    public class RCG_Item {
        public RCG_ItemData ItemData
        {
            get { return m_Data; }
        }
        public RCG_Item(RCG_ItemData iData)
        {
            m_Data = iData;
        }
        /// <summary>
        /// 使用這個道具
        /// </summary>
        public void Use(System.Action iEndAct)
        {
            RCG_DataService.ins.m_ItemsData.RemoveItem(this);
            RCG_Player.Ins.UseItem(this, iEndAct);
        }
        RCG_ItemData m_Data;
    }
    /// <summary>
    /// 讀取後儲存物品資料(從設定檔轉換成RCG_ItemData)
    /// </summary>
    public class RCG_ItemDataService : UCL.Core.Game.UCL_GameService
    {
        static public RCG_ItemDataService ins = null;
        public int ItemCount
        {
            get { return m_ItemDatas.Count; }
        }
        public List<RCG_ItemData> m_ItemDatas = new List<RCG_ItemData>();
        public Dictionary<string, RCG_ItemData> m_ItemDataDic = new Dictionary<string, RCG_ItemData>();
        public Dictionary<string, Sprite> m_ItemSprites = new Dictionary<string, Sprite>();
        public override void Init()
        {
            base.Init();
            ins = this;
            LoadItemData();
        }
        public Sprite GetItemIcon(string iIconName)
        {
            if (m_ItemSprites.ContainsKey(iIconName))
            {
                return m_ItemSprites[iIconName];
            }
            Sprite aIcon = null;
            var aIconPath = Path.Combine(RCG_ItemData.ItemIconRelativePath, iIconName);
            if (BetterStreamingAssets.FileExists(aIconPath))
            {
                var aData = BetterStreamingAssets.ReadAllBytes(aIconPath);
                //var aData = File.ReadAllBytes(aIconPath);
                Texture2D aTexture = new Texture2D(1, 1);
                aTexture.LoadImage(aData);
                var aSprite = Sprite.Create(aTexture, new Rect(0.0f, 0.0f, aTexture.width, aTexture.height),
                    new Vector2(0.5f, 0.5f), 100.0f);
                m_ItemSprites.Add(iIconName, aSprite);
                aIcon = aSprite;
                var aImg = UCL.Core.GameObjectLib.Create<Image>(iIconName, transform);
                aImg.sprite = aIcon;
            }
            else
            {
                Debug.LogError("GetCardIcon:" + iIconName + ",File not found!!aIconPath:" + aIconPath);
            }

            return aIcon;
        }

        public RCG_ItemData GetItemData(int iID)
        {
            if (iID < 0 || iID >= m_ItemDatas.Count) return null;
            return m_ItemDatas[iID];
        }
        public RCG_ItemData GetItemData(string iItemName)
        {
            if (!m_ItemDataDic.ContainsKey(iItemName))
            {
                Debug.LogError("!m_ItemDataDic.ContainsKey:" + iItemName);
                return null;
            }
            return m_ItemDataDic[iItemName];
        }
        public RCG_Item CreateItem(string iItemName)
        {
            return new RCG_Item(GetItemData(iItemName));
        }
        public void LoadItemData()
        {
            Debug.LogWarning("LoadItemData()");
            m_ItemDatas.Clear();
            string[] aFiles = null;

            try
            {
                aFiles = BetterStreamingAssets.GetFiles(RCG_ItemData.ItemDataRelativePath, "*.json", SearchOption.AllDirectories);
            }
            catch (System.Exception e)
            {
                Debug.LogError("LoadItemData() Exception:" + e);
            }
            if (aFiles != null)
            {
                foreach (var aFile in aFiles)
                {
                    try
                    {
                        string aData = BetterStreamingAssets.ReadAllText(aFile);
                        RCG_ItemData aItemData = new RCG_ItemData(aData);
                        m_ItemDatas.Add(aItemData);
                        string aFileName = UCL.Core.FileLib.Lib.GetFileName(aFile);
                        m_ItemDataDic.Add(UCL.Core.FileLib.Lib.RemoveFileExtension(aFileName), aItemData);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError("LoadFile:" + aFile + " ,Exception:" + e);
                    }
                }
            }




            //var aFiles = UCL.Core.FileLib.Lib.GetFiles(RCG_CardData.CardDataPath);
            //foreach(var aFile in aFiles) {
            //    string aData = System.IO.File.ReadAllText(aFile);
            //    RCG_CardData aCardData = new RCG_CardData(aData);
            //    m_CardDatas.Add(aCardData);
            //    string aFileName = UCL.Core.FileLib.Lib.GetFileName(aFile);
            //    m_CardDataDic.Add(UCL.Core.FileLib.Lib.RemoveFileExtension(aFileName), aCardData);
            //}



            //Debug.LogError("m_CardDataDic:" + m_CardDataDic.UCL_ToString());
        }
    }
}
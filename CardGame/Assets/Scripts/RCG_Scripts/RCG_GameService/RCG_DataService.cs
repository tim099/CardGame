using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG
{
    /// <summary>
    /// 玩家牌組資料
    /// </summary>
    [System.Serializable]
    public class RCG_DeckData
    {

    }
    /// <summary>
    /// 用來管理遊戲開始後的所有資料
    /// </summary>
    public class RCG_DataService : UCL.Core.Game.UCL_GameService
    {
        static public RCG_DataService ins = null;
        public RCG_DeckData m_DeckData = new RCG_DeckData();
        public override void Init() {
            base.Init();
            ins = this;
        }
    }
}
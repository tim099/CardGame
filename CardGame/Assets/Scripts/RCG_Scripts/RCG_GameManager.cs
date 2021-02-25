using System.Collections;
using System.Collections.Generic;
using UCL.Core.DebugLib;
using UCL.SceneLib;
using UnityEngine;

namespace RCG
{
    public class RCG_GameManager : UCL.Core.Game.UCL_GameManager
    {
#if UNITY_EDITOR
        /// <summary>
        /// Refresh all game data
        /// </summary>
        [UCL.Core.ATTR.UCL_FunctionButton]
        override public void RefreshGamedata()
        {
            var aMonsterFileInspector = Resources.Load<UCL.Core.FileLib.UCL_FileInspector>(PathConst.MonsterResource + "/MonsterFileInspector");
            aMonsterFileInspector.RefreshFileInfos();//更新怪物資料
        }
#endif
        public static RCG_GameManager ins = null;
        public string m_LoadMapName = "";
        public UCL_SceneLoader m_SceneLoader = null;
        public UCL_DebugLog m_DebugLog = null;
        protected override void Init() {
            ins = this;
            m_DebugLog.Init();
            Debug.LogWarning("RCG_GameManager Init!!");
            try
            {
                BetterStreamingAssets.Initialize();
            }catch(System.Exception e)
            {
                Debug.LogError("BetterStreamingAssets.Initialize():" + e);
            }

#if UNITY_EDITOR
            RefreshGamedata();
#endif
            base.Init();
            if(m_SceneLoader != null) m_SceneLoader.Load();
        }
        //void OnApplicationPause(bool pauseStatus)
    }
}


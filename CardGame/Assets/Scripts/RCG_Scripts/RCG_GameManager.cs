using System.Collections;
using System.Collections.Generic;
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
        protected override void Init() {
            ins = this;
#if UNITY_EDITOR
            RefreshGamedata();
#endif
            base.Init();
        }
        //void OnApplicationPause(bool pauseStatus)
    }
}


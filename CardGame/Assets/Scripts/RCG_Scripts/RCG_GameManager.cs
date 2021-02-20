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

        public RCG_DataService GetDataService()
        {
            for(int i= m_GameServices.Count-1; i>=0; i--)
            {
                Debug.Log(m_GameServices[i].GetType().Name);
                if(m_GameServices[i].GetType().Name == "RCG_DataService")
                {
                    return (RCG_DataService)m_GameServices[i] ;
                }
            }
            return new RCG_DataService();
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG
{
    public class RCG_GameManager : UCL.Core.Game.UCL_GameManager
    {
        public static RCG_GameManager ins = null;
        public string m_LoadMapName = "";
        protected override void Init() {
            ins = this;
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


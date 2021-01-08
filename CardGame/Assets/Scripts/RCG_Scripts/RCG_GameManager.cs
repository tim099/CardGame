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
    }
}


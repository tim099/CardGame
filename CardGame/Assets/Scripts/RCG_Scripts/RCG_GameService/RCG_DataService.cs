using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG
{
    public class RCG_DeckData
    {

    }
    public class RCG_DataService : UCL.Core.Game.UCL_GameService
    {
        public RCG_DataService ins = null;
        public override void Init() {
            base.Init();
            ins = this;
        }
    }
}
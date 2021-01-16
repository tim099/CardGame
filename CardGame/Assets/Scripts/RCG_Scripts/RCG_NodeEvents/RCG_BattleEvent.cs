using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG {
    public class RCG_BattleEvent : RCG_NodeEvent {
        override public void StartEvent() {
            RCG_BattleManager.ins.EnterBattle();
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG {
    public class RCG_BattleEvent : RCG_NodeEvent {
        override public void StartEvent(RCG_MapNode iNode) {
            var iMap = iNode.gameObject.GetComponentInParent<RCG_Map>();
            var iMonsterSet = iMap.m_MonsterSets[iMap.m_MonsterSets.Count-1];
            RCG_BattleManager.ins.EnterBattle(iMonsterSet);
        }
    }
}


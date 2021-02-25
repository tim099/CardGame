using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG
{
    public class RCG_MonsterTuple
    {
        public string m_MonsterName;
        public UnitPos m_MonsterPos;

        public RCG_MonsterTuple(string iName, string iPos)
        {
            m_MonsterName = iName;
            m_MonsterPos = (iPos == "Front" ? UnitPos.Front : UnitPos.Back);
        }
    }

    public class RCG_MonsterSet
    {
        public List<RCG_MonsterTuple> m_Monsters = new List<RCG_MonsterTuple>();
    }
}

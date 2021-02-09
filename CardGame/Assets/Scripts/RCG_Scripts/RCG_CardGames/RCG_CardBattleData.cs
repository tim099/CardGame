using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG
{
    public class RCG_CardBattleData : RCG_CardData
    {
        public RCG_CardData m_CardData = null;
        public override int Atk { get => m_CardData.Atk; }
        public RCG_CardBattleData(RCG_CardData iData)
        {
            m_CardData = iData;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RCG
{
    /// <summary>
    /// 單位在戰鬥中的站位
    /// </summary>
    public class RCG_BattlePosition : MonoBehaviour
    {
        public RCG_Unit m_Unit = null;
        public Transform m_UnitPos = null;
        public void Init(RCG_Unit iUnit)
        {
            m_Unit = iUnit;
            m_Unit.transform.SetParent(m_UnitPos);
            m_Unit.transform.position = m_UnitPos.transform.position;
        }
    }
}
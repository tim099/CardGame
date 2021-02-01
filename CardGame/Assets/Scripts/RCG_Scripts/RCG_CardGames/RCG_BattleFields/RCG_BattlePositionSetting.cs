using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG
{
    /// <summary>
    /// 怪物站位
    /// </summary>
    public enum MonsterPos
    {
        Front = 0,//前排
        Back,//後排
    }
    /// <summary>
    /// 戰鬥站位組合 包含前排跟後排
    /// </summary>
    public class RCG_BattlePositionSetting : MonoBehaviour
    {
        /// <summary>
        /// 前排站位
        /// </summary>
        public List<RCG_BattlePosition> m_FrontPositions = new List<RCG_BattlePosition>();
        /// <summary>
        /// 後排站位
        /// </summary>
        public List<RCG_BattlePosition> m_BackPositions = new List<RCG_BattlePosition>();



        virtual public void Init()
        {
            foreach(var aPos in m_FrontPositions)
            {
                aPos.Init();
            }
            foreach (var aPos in m_BackPositions)
            {
                aPos.Init();
            }
        }
        public void SetAllSelection(bool iSelection)
        {
            foreach (var aPos in m_FrontPositions)
            {
                aPos.ShowSelection(iSelection);
            }
            foreach (var aPos in m_BackPositions)
            {
                aPos.ShowSelection(iSelection);
            }
        }
        public void SetFrontSelection(bool iSelection)
        {
            foreach (var aPos in m_FrontPositions)
            {
                aPos.ShowSelection(iSelection);
            }
        }
        public void SetBackSelection(bool iSelection)
        {
            foreach (var aPos in m_BackPositions)
            {
                aPos.ShowSelection(iSelection);
            }
        }
        /// <summary>
        /// 設定單位到目標位置
        /// </summary>
        /// <param name="iPosition"></param>
        /// <param name="iUnit"></param>
        virtual public void SetUnit(MonsterPos iMonsterPos, int iPosition, RCG_Unit iUnit)
        {
            var aPositions = iMonsterPos == MonsterPos.Front? m_FrontPositions : m_BackPositions;
            
            if (iPosition < 0 || iPosition >= aPositions.Count)
            {
                Debug.LogError("iPosition:" + iPosition + ",Out of range!!aPositions.Count:" + aPositions.Count);
                return;
            }
            var aPos = aPositions[iPosition];
            aPos.SetUnit(iUnit);
        }

    }
}


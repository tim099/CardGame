using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG
{
    /// <summary>
    /// 單位站位
    /// </summary>
    public enum UnitPos
    {
        Front = 0,//前排
        Back,//後排
    }
    /// <summary>
    /// 戰鬥站位組合 包含前排跟後排
    /// </summary>
    public class RCG_BattlePositionSetting : MonoBehaviour
    {
        virtual public bool IsFrontUnitAlive
        {
            get
            {
                foreach (var aPos in m_FrontPositions)
                {
                    if (aPos.IsAlive) return true;
                }
                return false;
            }
        }
        virtual public bool IsBackUnitAlive
        {
            get
            {
                foreach (var aPos in m_BackPositions)
                {
                    if (aPos.IsAlive) return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 前排站位
        /// </summary>
        public List<RCG_BattlePosition> m_FrontPositions = new List<RCG_BattlePosition>();
        /// <summary>
        /// 後排站位
        /// </summary>
        public List<RCG_BattlePosition> m_BackPositions = new List<RCG_BattlePosition>();

        public Transform m_SelectionRoot = null;

        virtual public void Init()
        {
            foreach(var aPos in m_FrontPositions)
            {
                aPos.Init(this);
            }
            foreach (var aPos in m_BackPositions)
            {
                aPos.Init(this);
            }
        }
        public List<RCG_Unit> GetFrontUntis()
        {
            List<RCG_Unit> aUnits = new List<RCG_Unit>();
            if (IsFrontUnitAlive)
            {
                foreach (var aPos in m_FrontPositions)
                {
                    if(aPos.m_Unit != null) aUnits.Add(aPos.m_Unit);
                }
            }
            else
            {
                foreach (var aPos in m_BackPositions)
                {
                    if (aPos.m_Unit != null) aUnits.Add(aPos.m_Unit);
                }
            }
            return aUnits;
        }
        public List<RCG_Unit> GetBackUntis()
        {
            List<RCG_Unit> aUnits = new List<RCG_Unit>();
            if (IsBackUnitAlive)
            {
                foreach (var aPos in m_BackPositions)
                {
                    if (aPos.m_Unit != null) aUnits.Add(aPos.m_Unit);
                }
            }
            else
            {
                foreach (var aPos in m_FrontPositions)
                {
                    if (aPos.m_Unit != null) aUnits.Add(aPos.m_Unit);
                }
            }
            return aUnits;
        }
        public List<RCG_Unit> GetAllUnits()
        {
            List<RCG_Unit> aUnits = new List<RCG_Unit>();
            foreach (var aPos in m_BackPositions)
            {
                if (aPos.m_Unit != null) aUnits.Add(aPos.m_Unit);
            }
            foreach (var aPos in m_FrontPositions)
            {
                if (aPos.m_Unit != null) aUnits.Add(aPos.m_Unit);
            }
            return aUnits;
        }
        public void SelectPlayer(HashSet<RCG_Unit> iActivatedPlayer)
        {
            foreach (var aPos in m_FrontPositions)
            {
                aPos.ShowSelection(!iActivatedPlayer.Contains(aPos.m_Unit));
            }
            foreach (var aPos in m_BackPositions)
            {
                aPos.ShowSelection(!iActivatedPlayer.Contains(aPos.m_Unit));
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
        public void SelectFront()
        {
            if (IsFrontUnitAlive)
            {
                SetFrontSelection(true);
                SetBackSelection(false);
            }
            else
            {
                SetFrontSelection(false);
                SetBackSelection(true);
            }

        }
        public void SelectBack()
        {
            if (IsBackUnitAlive)
            {
                SetFrontSelection(false);
                SetBackSelection(true);
            }
            else
            {
                SetFrontSelection(true);
                SetBackSelection(false);
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
        virtual public void SetUnit(UnitPos iMonsterPos, int iPosition, RCG_Unit iUnit)
        {
            var aPositions = iMonsterPos == UnitPos.Front? m_FrontPositions : m_BackPositions;
            
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


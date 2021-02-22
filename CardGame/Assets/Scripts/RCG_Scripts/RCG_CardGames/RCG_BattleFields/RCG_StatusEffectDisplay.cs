using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RCG
{
    public enum StatusEffectType
    {
        Buff = 0,
        Debuff
    }
    public enum StatusDecreaseType
    {
        /// <summary>
        /// 每回合減少1層
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 每回合重置到0
        /// </summary>
        Clear,
        /// <summary>
        /// 每回合減半
        /// </summary>
        Half,
    }
    public class RCG_StatusEffectDisplay : MonoBehaviour
    {
        public RCG_Unit p_Unit = null;
        public StatusType m_StatusType = StatusType.None;
        /// <summary>
        /// 效果類型
        /// </summary>
        public StatusEffectType m_StatusEffectType = StatusEffectType.Buff;
        /// <summary>
        /// 每回合減少的類型
        /// </summary>
        public StatusDecreaseType m_StatusDecreaseType = StatusDecreaseType.Normal;
        public Image m_StatusImg = null;
        public Text m_StatusLayerText = null;
        public int StatusLayer
        {
            get
            {
                return m_StatusLayer;
            }
            set
            {
                m_StatusLayer = value;
                m_StatusLayerText.text = m_StatusLayer.ToString();
            }
        }
        /// <summary>
        /// 狀態層數
        /// </summary>
        protected int m_StatusLayer = 0;
        virtual public void Init(RCG_Unit iUnit)
        {
            p_Unit = iUnit;
            gameObject.SetActive(true);
            StatusLayer = 0;
        }
        virtual public void AlterLayer(int iAmount)
        {
            int aVal = m_StatusLayer + iAmount;
            if (aVal < 0) aVal = 0;
            StatusLayer = aVal;
        }
        virtual public void TurnEnd()
        {
            switch (m_StatusType)
            {
                case StatusType.Bleed:
                    {
                        p_Unit.UnitHit(m_StatusLayer);
                        break;
                    }
            }
            switch (m_StatusDecreaseType)
            {
                case StatusDecreaseType.Normal:
                    {
                        AlterLayer(-1);
                        break;
                    }
                case StatusDecreaseType.Clear:
                    {
                        StatusLayer = 0;
                        break;
                    }
                case StatusDecreaseType.Half:
                    {
                        StatusLayer =  m_StatusLayer / 2;
                        break;
                    }
            }
        }
    }
}
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
    [UCL.Core.ATTR.EnableUCLEditor]
    public class RCG_StatusEffectDisplay : MonoBehaviour
    {
        public RCG_Unit p_Unit = null;
        virtual public StatusType StatusType { get{ return m_StatusType; } }
        [SerializeField] public StatusType m_StatusType = StatusType.None;
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
        public bool End
        {
            get
            {
                return StatusLayer <= 0;
            }
        }
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
        virtual public float GetAtkBuff()
        {
            return 0f;
        }
        virtual public int GetAtkAlter()
        {
            return 0;
        }
        virtual public void AlterLayer(int iAmount)
        {
            int aVal = m_StatusLayer + iAmount;
            if (aVal < 0) aVal = 0;
            StatusLayer = aVal;
        }
        virtual public void TurnEndDecreaseLayer()
        {
            switch (m_StatusDecreaseType)
            {
                case StatusDecreaseType.Normal:
                    {
                        RCG_Player.ins.AddPlayerAction(CreateAction.StatusAction(p_Unit, m_StatusType, -1));
                        //AlterLayer(-1);
                        break;
                    }
                case StatusDecreaseType.Clear:
                    {
                        RCG_Player.ins.AddPlayerAction(CreateAction.StatusAction(p_Unit, m_StatusType, -StatusLayer));
                        //StatusLayer = 0;
                        break;
                    }
                case StatusDecreaseType.Half:
                    {
                        int aDel = m_StatusLayer - m_StatusLayer / 2;
                        RCG_Player.ins.AddPlayerAction(CreateAction.StatusAction(p_Unit, m_StatusType, -aDel));

                        break;
                    }
            }

        }
        /// <summary>
        /// 回合結束時結算行為(etc. Bleed每回合扣血)
        /// </summary>
        virtual public void TurnEndAction() { }
        virtual public void TurnEnd()
        {
            TurnEndAction();
            TurnEndDecreaseLayer();
        }

        [UCL.Core.ATTR.UCL_FunctionButton]
        virtual public void AutoLinkData()
        {
            m_StatusLayerText = UCL.Core.GameObjectLib.SearchChild<Text>(transform, "StatusLayerText");
            m_StatusImg = UCL.Core.GameObjectLib.SearchChild<Image>(transform, "StatusImg");
        }
    }
}
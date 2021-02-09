﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RCG
{
    /// <summary>
    /// 單位在戰鬥中的站位
    /// </summary>
    public class RCG_BattlePosition : MonoBehaviour
    {
        virtual public bool IsAlive
        {
            get
            {
                if (m_Unit == null) return false;
                return !m_Unit.IsDead;
            }
        }
        public RCG_Unit m_Unit = null;
        public Transform m_UnitPos = null;
        public Button m_SelectButton = null;
        protected RCG_BattlePositionSetting p_BattlePositionSetting = null;
        virtual public void Init(RCG_BattlePositionSetting iBattlePositionSetting)
        {
            p_BattlePositionSetting = iBattlePositionSetting;
            m_SelectButton.transform.SetParent(iBattlePositionSetting.m_SelectionRoot);
            m_SelectButton.gameObject.SetActive(false);
            m_SelectButton.onClick.AddListener(SelectAction);
        }

        virtual public void ShowSelection(bool iShow)
        {
            if (m_Unit == null)
            {
                m_SelectButton.gameObject.SetActive(false);
                return;
            }
            m_SelectButton.GetComponent<RectTransform>().CopyValue(m_Unit.GetComponent<RectTransform>());
            m_SelectButton.gameObject.SetActive(iShow);
        }
        virtual public void SelectAction()
        {
            RCG_BattleField.ins.SelectUnit(m_Unit);
        }
        virtual public void SetUnit(RCG_Unit iUnit)
        {
            m_Unit = iUnit;
            m_Unit.transform.SetParent(m_UnitPos);
            m_Unit.transform.position = m_UnitPos.transform.position;
        }
    }
}
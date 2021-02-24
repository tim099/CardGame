using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RCG {
    public enum RCG_MonsterActionEnum
    {
        IdleAction,
        SimpleAttackAction
    }
 
    [RequireComponent(typeof(RCG_Unit))]
    public class RCG_Monster: MonoBehaviour
    {
        public List<RCG_MonsterActionEnum> m_ActionsNames;
        RCG_MonsterActionIndicator m_Indicator;
        protected List<RCG_MonsterAction> m_Actions = new List<RCG_MonsterAction>();
        RCG_MonsterAction m_CurrentAction;

        public void Init()
        {
            if (true)
            {
                m_Indicator = Resources.Load<RCG_MonsterActionIndicator>("PrefabsRes/UI/MonsterActionIndicator");
            }
            foreach (var s in m_ActionsNames)
            {
                Debug.Log(Enum.GetName(typeof(RCG_MonsterActionEnum), s));
                var action = RCG_MonsterActionCreator.Create(Enum.GetName(typeof(RCG_MonsterActionEnum), s));
                m_Actions.Add(action);
                Debug.Log(m_Actions.Count);
                Debug.Log(m_ActionsNames.Count);
            }
        }

        public void Act()
        {
            
            m_CurrentAction.TriggerAction(delegate () { });
        }

        public void PrepareToAct()
        {
            if (m_Actions.Count == 0)
            {
                Init();
                if (m_ActionsNames.Count == 0) //no action assigned
                {
                    m_Actions.Add(RCG_MonsterActionCreator.Create("SimpleAttackAction"));
                }
            }
            List<RCG_MonsterAction> aAvailableActions = new List<RCG_MonsterAction>();
            foreach (var action in m_Actions)
            {
                if (action.ActionAvailable())
                {
                    aAvailableActions.Add(action);
                }
            }
            m_CurrentAction = aAvailableActions[UCL.Core.MathLib.UCL_Random.Instance.Next(aAvailableActions.Count)];
            UpdateIndicator();
        }

        public void UpdateIndicator()
        {
            Debug.Log("update indicator");
            var iIndicator = gameObject.GetComponentInChildren<RCG_MonsterActionIndicator>();
            if (iIndicator)
            {

            }
            else
            {
                iIndicator = Instantiate<RCG_MonsterActionIndicator>(m_Indicator, transform);
            }
            iIndicator.Init(m_CurrentAction);
        }
    }
}

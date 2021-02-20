using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG {
    [RequireComponent(typeof(RCG_Unit))]
    public class RCG_Monster: MonoBehaviour
    {
        public List<string> m_ActionsNames;
        public RCG_MonsterActionIndicator m_Indicator;
        protected List<RCG_MonsterAction> m_Actions = new List<RCG_MonsterAction>();
        RCG_MonsterAction m_CurrentAction;

        public void Init()
        {
            foreach(var s in m_ActionsNames)
            {
                var action = RCG_MonsterActionCreator.Create(s);
                m_Actions.Add(action);
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
                if (m_ActionsNames.Count != 0)
                {
                    Init();
                }
                else
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
        }
    }
}

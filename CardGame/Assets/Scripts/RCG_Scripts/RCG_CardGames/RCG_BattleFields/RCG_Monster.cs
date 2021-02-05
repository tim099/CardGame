using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG {
    [RequireComponent(typeof(RCG_Unit))]
    public class RCG_Monster: MonoBehaviour
    {
        public List<string> m_ActionsNames;
        protected List<RCG_MonsterAction> m_Actions = new List<RCG_MonsterAction>();

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
            if(m_Actions.Count == 0)
            {
                if(m_ActionsNames.Count != 0)
                {
                    Init();
                }
                else
                {
                    m_Actions.Add(RCG_MonsterActionCreator.Create("SimpleAttackAction"));
                }
            }
            List<RCG_MonsterAction> aAvailableActions = new List<RCG_MonsterAction>();
            foreach(var action in m_Actions)
            {
                if (action.ActionAvailable())
                {
                    aAvailableActions.Add(action);
                }
            }
            var target_action = aAvailableActions[UCL.Core.MathLib.UCL_Random.Instance.Next(aAvailableActions.Count)];
            target_action.TriggerAction(delegate () { });
        }
    }
}

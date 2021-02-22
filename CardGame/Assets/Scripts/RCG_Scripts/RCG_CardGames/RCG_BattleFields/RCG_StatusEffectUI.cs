using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG
{
    public class RCG_StatusEffectUI : MonoBehaviour
    {
        protected RCG_Unit p_Unit;
        Dictionary<StatusType, RCG_StatusEffectDisplay> m_StatusEffects = new Dictionary<StatusType, RCG_StatusEffectDisplay>();
        virtual public void Init(RCG_Unit iUnit)
        {
            p_Unit = iUnit;
        }
        virtual public void AddStatusEffect(StatusType iStatusType, int iAmount)
        {
            if (!m_StatusEffects.ContainsKey(iStatusType))
            {
                var aStatus = RCG_StatusManager.ins.CreateStatus(iStatusType, transform);
                aStatus.Init(p_Unit);
                m_StatusEffects.Add(iStatusType, aStatus);
            }
            m_StatusEffects[iStatusType].AlterLayer(iAmount);
        }
        virtual public void TurnEnd()
        {
            List<StatusType> aEndEffect = new List<StatusType>();
            foreach(var aEffectKey in m_StatusEffects.Keys)
            {
                var aEffect = m_StatusEffects[aEffectKey];
                aEffect.TurnEnd();
                if (aEffect.StatusLayer <= 0)
                {
                    aEndEffect.Add(aEffectKey);
                }
            }
            foreach(var aKey in aEndEffect)
            {
                Destroy(m_StatusEffects[aKey].gameObject);
                m_StatusEffects.Remove(aKey);
            }
        }
    }
}
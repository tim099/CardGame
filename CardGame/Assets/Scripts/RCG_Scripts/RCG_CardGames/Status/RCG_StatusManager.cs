using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG
{
    public class RCG_StatusManager : MonoBehaviour
    {
        public static RCG_StatusManager ins = null;
        public List<RCG_StatusEffectDisplay> m_StatusTemplates = new List<RCG_StatusEffectDisplay>();
        protected Dictionary<StatusType, RCG_StatusEffectDisplay> m_StatusDic = new Dictionary<StatusType, RCG_StatusEffectDisplay>();
        virtual public void Init()
        {
            ins = this;
            for(int i = 0; i < m_StatusTemplates.Count; i++)
            {
                var aStatus = m_StatusTemplates[i];
                aStatus.gameObject.SetActive(false);
                if (m_StatusDic.ContainsKey(aStatus.StatusType))
                {
                    Debug.LogError("m_StatusDic.ContainsKey:" + aStatus.StatusType.ToString());
                }
                else
                {
                    m_StatusDic.Add(aStatus.StatusType, aStatus);
                }
            }
        }
        virtual public RCG_StatusEffectDisplay CreateStatus(StatusType iType, Transform iTarget)
        {
            if (m_StatusDic.ContainsKey(iType))
            {
                RCG_StatusEffectDisplay aStatus = Instantiate(m_StatusDic[iType], iTarget);
                return aStatus;
            }
            return null;
        }
    }
}
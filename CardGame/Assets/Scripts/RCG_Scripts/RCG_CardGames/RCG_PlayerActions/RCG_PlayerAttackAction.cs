using System;
using System.Collections;
using System.Collections.Generic;
using UCL.TweenLib;
using UnityEngine;

namespace RCG
{
    public class RCG_PlayerAttackAction : RCG_PlayerAction
    {
        List<RCG_Unit> m_Targets = null;
        RCG_Unit m_Attaker = null;
        int m_Atk = 0;
        public RCG_PlayerAttackAction(RCG_Unit iAttaker, List<RCG_Unit> iTargets, int iAtk)
        {
            m_Attaker = iAttaker;
            m_Targets = iTargets;
            m_Atk = iAtk;
        }
        public override void Trigger(Action iEndAction)
        {
            int aAtk = m_Atk;
            if (RCG_BattleField.ins.ActiveUnit != null)
            {
                aAtk = RCG_BattleField.ins.ActiveUnit.GetAtk(m_Atk);
            }
            if (m_Targets == null)
            {
                iEndAction.Invoke();
                return;
            }
            for (int i = m_Targets.Count - 1; i >= 0; i--)
            {
                if (m_Targets[i].IsDead)
                {
                    m_Targets.RemoveAt(i);
                }
            }
            if(m_Targets.Count == 0)
            {
                iEndAction.Invoke();
                return;
            }
            var aSeq = LibTween.Sequence();
            {
                aSeq.Append(delegate ()
                {
                    foreach (var aTarget in m_Targets)
                    {
                        if (!aTarget.IsDead)
                        {
                            aTarget.UnitHit(aAtk);
                        }
                    }
                });
                var aTweener = LibTween.Tweener(0.2f);//aSeq.AppendInterval(0.8f);
                aSeq.Append(aTweener);
                foreach (var aTarget in m_Targets)
                {
                    if (!aTarget.IsDead)
                    {
                        aTweener.AddComponent(aTarget.transform.TC_LocalShake(35, 20, true));
                    }
                }
                aSeq.AppendInterval(0.4f);
            }
            aSeq.OnComplete(iEndAction);
            aSeq.Start();
        }
    }
}
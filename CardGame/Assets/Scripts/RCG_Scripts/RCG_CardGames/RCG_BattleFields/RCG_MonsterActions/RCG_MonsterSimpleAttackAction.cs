using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UCL.TweenLib;

namespace RCG
{
    public class RCG_MonsterSimpleAttackAction: RCG_MonsterAction
    {
        int m_Atk;
        int m_AtkTimes;
        string m_AttackRange;
	    string m_AttackType;
        string m_ActionName;

        override public bool ActionAvailable()
        {
            return true;
        }

        public override void TriggerAction(Action iEndAction)
        {
            RCG_Unit aTarget = null;
            UnitPos target_pos = (m_AttackRange == "Front" ? UnitPos.Front : UnitPos.Back);
            var aTargets = RCG_BattleField.ins.GetPlayerUnits(target_pos);//抓取對應排的所有單位
            //UCL.Core.MathLib.UCL_Random.Instance.Shuffle(ref RCG_BattleField.ins.m_Characters);
            //foreach (var character in RCG_BattleField.ins.m_Characters)
            //{
            //    if(character.m_UnitPos == target_pos)
            //    {
            //        aTarget = character;
            //        break;
            //    }
            //}
            aTarget = UCL.Core.MathLib.UCL_Random.Instance.RandomPick(aTargets);
            RCG_BattleField.ins.AttackUnit(null, aTarget, m_Atk, m_AtkTimes, iEndAction);

            //if(aTarget != null)
            //{
            //    var aSeq = LibTween.Sequence();
            //    for (int i = 0; i < m_AtkTimes; i++)
            //    {
            //        aSeq.Append(delegate ()
            //        {
            //            if (!aTarget.IsDead)
            //            {
            //                aTarget.UnitHit(m_Atk);
            //            }
            //        });
            //        var aTweener = LibTween.Tweener(0.2f);//aSeq.AppendInterval(0.8f);
            //        aSeq.Append(aTweener);
            //        if (!aTarget.IsDead)
            //        {
            //            aTweener.AddComponent(aTarget.transform.TC_LocalShake(35, 20, true));
            //        }
            //        aSeq.AppendInterval(0.4f);
            //    }
            //    aSeq.OnComplete(iEndAction);
            //    aSeq.Start();
            //}
            //else
            //{
            //    iEndAction.Invoke();
            //}
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCL.TweenLib;
namespace RCG {
    public class RCG_CardAttackEffect : RCG_BattleEffect {
        [System.Serializable]
        public struct AttackData {
            public int m_Int;
            public float m_Float;
        }
        public enum AttackType {
            Normal = 0,
            Magic,
        }
        public int m_Atk = 0;
        public int m_AtkRange = 0;
        public int m_AtkTimes = 1;
        public AttackType m_AttackType = AttackType.Normal;
        //public AttackData m_AttackData;
        override public void OnGUI(int iID) {
            base.OnGUI(iID);
        }
        override public string Description {
            get {
                if(m_AtkTimes > 1) {
                    return UCL.Core.LocalizeLib.UCL_LocalizeManager.Get("Attack_Des", m_Atk, m_AtkTimes, m_AtkRange) + "\n";
                } else {
                    return UCL.Core.LocalizeLib.UCL_LocalizeManager.Get("Attack_DesSingle", m_Atk, m_AtkRange) + "\n";
                }
            }
        }
        public override void TriggerEffect(TriggerEffectData iTriggerEffectData, Action iEndAction)
        {
            var aTargets = iTriggerEffectData.m_Targets;
            if (aTargets != null)
            {
                var aSeq = LibTween.Sequence();
                for(int i = 0; i < m_AtkTimes; i++)
                {
                    aSeq.Append(delegate ()
                    {
                        foreach (var aTarget in aTargets)
                        {
                            aTarget.UnitHit();
                            aTarget.Hp -= m_Atk;
                        }
                    });
                    var aTweener = LibTween.Tweener(0.3f);//aSeq.AppendInterval(0.8f);
                    aSeq.Append(aTweener);
                    foreach (var aTarget in aTargets)
                    {
                        aTweener.AddComponent(aTarget.transform.TC_LocalShake(30, 20, true));
                    }
                    aSeq.AppendInterval(0.5f);
                }
                aSeq.OnComplete(iEndAction);
                aSeq.Start();
            }
            else//no attack targets!!
            {
                iEndAction.Invoke();
            }
        }
        //override public void LoadJson(UCL.Core.JsonLib.JsonData data) {
        //    UCL.Core.JsonLib.JsonConvert.LoadDataFromJson(this, data);
        //}
        //override public UCL.Core.JsonLib.JsonData ToJson() {
        //    UCL.Core.JsonLib.JsonData data = base.ToJson();
        //    UCL.Core.JsonLib.JsonConvert.SaveDataToJson(this, data);
        //    return data;
        //}
    }
}
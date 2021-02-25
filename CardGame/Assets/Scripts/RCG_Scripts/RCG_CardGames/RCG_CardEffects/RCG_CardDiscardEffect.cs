using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCL.TweenLib;
namespace RCG
{
    public class RCG_CardDiscardEffect : RCG_CardEffect
    {
        public int m_DiscardCardNum = 0;
        override public string Description
        {
            get
            {
                return UCL.Core.LocalizeLib.UCL_LocalizeManager.Get("DiscardCard_Des", m_DiscardCardNum) + System.Environment.NewLine;
            }
        }
        /// <summary>
        /// 觸發卡牌效果前執行 用來選擇棄牌或其他行動
        /// </summary>
        public override void PostTriggerAction()
        {
            RCG_Player.ins.AddPlayerAction(CreateAction.ActionTrigger(delegate (System.Action iEndAct)
            {
                iEndAct.Invoke();
                //var aTween = LibTween.Tweener(2f);
                //aTween.OnComplete(delegate ()
                //{
                //    iEndAct.Invoke();
                //});
                //aTween.Start();
            }));
        }
        public override void TriggerEffect(TriggerEffectData iTriggerEffectData, System.Action iEndAction)
        {
            iTriggerEffectData.p_Player.DrawCard(m_DiscardCardNum);
            iEndAction.Invoke();
        }
    }
}


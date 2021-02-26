using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCL.TweenLib;
namespace RCG
{
    public class RCG_CardDiscardEffect : RCG_CardEffect
    {
        public int m_DiscardCardNum = 0;
        private List<RCG_Card> m_DiscardCardList = null;
        override public string Description
        {
            get
            {
                return UCL.Core.LocalizeLib.UCL_LocalizeManager.Get("DiscardCard_Des", m_DiscardCardNum) + System.Environment.NewLine;
            }
        }
        override public void DeserializeFromJson(UCL.Core.JsonLib.JsonData data)
        {
            UCL.Core.JsonLib.JsonConvert.LoadDataFromJson(this, data);
        }

        override public UCL.Core.JsonLib.JsonData SerializeToJson()
        {
            UCL.Core.JsonLib.JsonData data = new UCL.Core.JsonLib.JsonData();
            data["EffectType"] = EffectType;
            data["m_DiscardCardNum"] = m_DiscardCardNum;
            return data;
        }
        /// <summary>
        /// 觸發卡牌效果前執行 用來選擇棄牌或其他行動
        /// </summary>
        public override void PostTriggerAction()
        {
            m_DiscardCardList = new List<RCG_Card>();
            for(int i = 0; i < m_DiscardCardNum; i++)
            {
                RCG_Player.ins.AddPlayerAction(CreateAction.ActionTrigger(delegate (System.Action iEndAct)
                {
                    RCG_Player.ins.StartSelectCardDontCheck(UCL.Core.LocalizeLib.UCL_LocalizeManager.Get("SelectDiscardCard"),
                        delegate (RCG_Card iCard)
                        {
                            if(iCard == null)
                            {
                                Debug.LogError("Discard card Fail!!No avaliable card!!");
                                iEndAct.Invoke();
                                return;
                            }
                            m_DiscardCardList.Add(iCard);
                            RCG_Player.ins.AddUsingCardAnim(iCard, iEndAct);
                            //Debug.LogWarning("Selected Card:" + iCard.Data.CardName);
                        });
                }));
            }

        }
        public override void TriggerEffect(TriggerEffectData iTriggerEffectData, System.Action iEndAction)
        {
            if(m_DiscardCardList.Count == 0)
            {
                iEndAction.Invoke();
            }
            System.Action<int> aDiscardAct = null;
            aDiscardAct = delegate (int iAt)
            {
                if (iAt < m_DiscardCardList.Count)
                {
                    iTriggerEffectData.p_Player.DiscardCard(m_DiscardCardList[iAt], delegate() {
                        aDiscardAct(iAt + 1);
                    });
                }
                else
                {
                    iEndAction.Invoke();
                }
                
            };
            aDiscardAct(0);
            
            //iTriggerEffectData.p_Player.DrawCard(m_DiscardCardNum);
            //iEndAction.Invoke();
        }
    }
}


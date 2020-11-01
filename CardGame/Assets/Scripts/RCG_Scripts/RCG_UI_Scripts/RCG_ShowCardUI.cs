﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCL.TweenLib;
using UnityEngine.UI;

namespace RCG
{
    public class RCG_ShowCardUI : MonoBehaviour
    {
        public RCG_CardRowsDisplayUI m_CardRowsDisplayUITmp;
        public Transform m_HidePos;
        public ScrollRect m_ScrollRect;
        Vector3 m_ShowPos = Vector3.zero;
        List<RCG_CardRowsDisplayUI> m_CardRowsDisplayUIs = new List<RCG_CardRowsDisplayUI>();
        public void Init() {
            m_CardRowsDisplayUITmp.Hide();
            m_CardRowsDisplayUIs.Add(m_CardRowsDisplayUITmp);
            m_ShowPos = transform.position;
        }
        public void Show(List<RCG_CardData> cards) {
            
            int card_num_in_row = m_CardRowsDisplayUITmp.m_CardDisplayer.Count;
            while(cards.Count > m_CardRowsDisplayUIs.Count * card_num_in_row) {
                var new_row = Instantiate(m_CardRowsDisplayUITmp, m_CardRowsDisplayUITmp.transform.parent);
                new_row.name = m_CardRowsDisplayUITmp.name + "_" + m_CardRowsDisplayUIs.Count;
                m_CardRowsDisplayUIs.Add(new_row);
            }
            for(int i = 0; i < m_CardRowsDisplayUIs.Count; i++) {
                var row = m_CardRowsDisplayUIs[i];
                int count = i * card_num_in_row;
                if(count < cards.Count - card_num_in_row) {
                    row.Show(cards, count, card_num_in_row);
                } else if(count < cards.Count) {
                    row.Show(cards, count, cards.Count - count);
                } else {
                    row.Hide();
                }
            }
            GetComponent<RectTransform>().ForceRebuildLayoutImmediate();

            gameObject.SetActive(true);
            transform.position = m_HidePos.position;
            transform.localScale = new Vector3(0.3f,0.3f,1f);
            m_ScrollRect.ToTop();
            var twn = transform.UCL_Scale(0.3f, 1f).SetEase(EaseType.OutElastic);
            twn.AddComponent(transform.TC_Move(m_ShowPos));
            //twn.OnComplete(() => {
            //    m_ScrollRect.ToTop();
            //});
            twn.Start();

        }
        public void Hide() {
            transform.position = m_ShowPos;
            transform.localScale = Vector3.one;
            var twn = transform.UCL_Scale(0.1f, 0f).SetEase(EaseType.OutQuint);
            twn.AddComponent(transform.TC_Move(m_HidePos));
            twn.OnComplete(() => {
                gameObject.SetActive(false);
                transform.position = m_ShowPos;
            });
            twn.Start();
        }
    }
}
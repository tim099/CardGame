﻿using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UCL.Core;
using UCL.Core.UI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RCG {

    public class RCG_Card : MonoBehaviour {
        public enum BlockingStatus {
            Cost = 1,
            DrawCardAnime,
        }
        public bool IsBlocking {
            get { return m_SelectionBlock.Count > 0; }
        }
        public GameObject m_BlockSelectionObject;
        public GameObject m_SelectedObject;
        public GameObject m_CardPanel;
        public UCL_Button m_Button;
        public UCL.TweenLib.UCL_TB_Tweener m_TB_Tweener;
        public RCG_CardDisplayer m_CardDisplayer;
        public bool m_Used = false;
        protected RCG_CardData m_Data;
        protected RCG_Player p_Player;
        public RCG_CardPos p_CardPos;
        HashSet<BlockingStatus> m_SelectionBlock = new HashSet<BlockingStatus>();
        virtual public RCG_CardData Data { get { return m_Data; } }
        virtual public bool IsSelected {
            get { return m_SelectedObject.activeSelf; }
        }
        virtual public bool IsCardDisplayerSelected {
            get { return m_CardDisplayer.IsSelected; }
        }
        /// <summary>
        /// target >=3 is enemy
        /// </summary>
        /// <param name="target"></param>
        virtual public bool TriggerCardEffect(int target) {
            Debug.LogWarning("target:" + target);
            if(p_Player == null) {
                Debug.LogError("p_Player == null");
                return false;
            }
            if(m_Data == null) {
                Debug.LogError("m_Data == null");
                return false;
            }
            //if(!m_Data.TargetCheck(target)) return false;
            if(!p_Player.AlterCost(-m_Data.Cost)) {
                ///費用不足
                return false;
            }
            m_Used = true;
            p_Player.IsUsingCard = true;
            m_Data.TriggerEffect(p_Player);
            if(RCG_BattleField.ins != null) {
                RCG_BattleField.ins.TriggerCardEffect(target, m_Data);
            }
            p_Player.IsUsingCard = false;
            return true;
        }
        /// <summary>
        /// 更新卡牌資訊 包含判斷玩家費用是否足夠等
        /// </summary>
        virtual public void UpdateCardStatus() {
            if(m_Data == null) return;
            if(m_Data.Cost > p_Player.Cost) {
                BlockSelection(BlockingStatus.Cost);
            } else {
                UnBlockSelection(BlockingStatus.Cost);
            }
        }
        /// <summary>
        /// Avaliable Cards
        /// </summary>
        virtual public bool IsEmpty {
            get {
                if(m_Used || m_Data == null) return true;
                return false;
            }
        }
        virtual public void SetCardPos(RCG_CardPos iCardPos) {
            p_CardPos = iCardPos;
        }
        virtual public void Init(RCG_Player _Player) {
            p_Player = _Player;
            m_SelectedObject.SetActive(false);
            m_Button.m_OnPointerUp.AddListener(p_Player.CardRelease);
            m_Button.m_OnClick.AddListener(delegate () {
                if(!IsBlocking) {
                    Debug.Log("SetSelected:" + name);
                    p_Player.SetSelectedCard(this);
                } else {
                    Debug.LogError("SetSelectedCard Fail Blocking:" + m_SelectionBlock.UCL_ToString());
                    p_Player.SetSelectedCard(null);
                }
            });
            //m_CardDisplayer.m_OnSelected.AddListener(() => { transform.SetAsLastSibling(); });
            m_CardDisplayer.OnPointerEnter(() => {
                //Debug.LogWarning("m_CardDisplayer.OnPointerEnter");
                m_CardDisplayer.Select();
            });
            m_CardDisplayer.OnPointerExit(() => {
                //Debug.LogWarning("m_CardDisplayer.OnPointerExit");
                if(!m_SelectedObject.activeSelf) {
                    m_CardDisplayer.DeSelect();
                }
            });
        }
        virtual public void TurnInit(RCG_CardData _Data) {
            SetCardData(_Data);
        }
        virtual public void Select() {
            m_SelectedObject.SetActive(true);
        }
        virtual public void Deselect() {
            m_SelectedObject.SetActive(false);
            m_CardDisplayer.DeSelect();
        }
        /// <summary>
        /// 設定顯示卡牌資料
        /// </summary>
        /// <param name="_Data"></param>
        virtual public void SetCardData(RCG_CardData _Data) {
            m_Data = _Data;
            m_CardDisplayer.Init(m_Data);
            m_TB_Tweener.Kill();
            if(m_Data != null) {
                m_CardPanel.transform.position = transform.position;
                m_CardPanel.SetActive(true);
            } else {
                m_CardPanel.transform.position = transform.position;
                m_CardPanel.SetActive(false);
            }
            m_Used = false;
            UpdateCardStatus();
        }
        virtual public void BlockSelection(BlockingStatus iBlock) {
            m_BlockSelectionObject.SetActive(true);            
            if(!m_SelectionBlock.Contains(iBlock))m_SelectionBlock.Add(iBlock);
        }
        virtual public void UnBlockSelection(BlockingStatus iBlock) {
            if(m_SelectionBlock.Contains(iBlock)) m_SelectionBlock.Remove(iBlock);
            if(IsBlocking) return;
            m_BlockSelectionObject.SetActive(false);
        }
        /// <summary>
        /// 抽牌演出
        /// </summary>
        /// <param name="iStartPos"></param>
        /// <param name="iEndAct"></param>
        virtual public void DrawCardAnime(Vector3 iStartPos, System.Action iEndAct) {
            m_CardPanel.transform.position = iStartPos;
            m_CardPanel.SetActive(true);
            m_CardDisplayer.BlockSelection();
            BlockSelection(BlockingStatus.DrawCardAnime);
            m_TB_Tweener.StartTween(()=> {
                m_CardDisplayer.UnBlockSelection();
                UnBlockSelection(BlockingStatus.DrawCardAnime);
                if(iEndAct != null) iEndAct.Invoke();
            });
        }
        virtual public void CardUsed() {
            SetCardData(null);
        }
        virtual protected void Update() {

        }
    }
}
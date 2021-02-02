using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UCL.Core;
using UCL.Core.UI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RCG {
    /// <summary>
    /// 觸發卡牌效果的資料 包含目標對象等...
    /// </summary>
    public class TriggerEffectData {
        public TriggerEffectData(RCG_Player iPlayer) {
            p_Player = iPlayer;
        }
        public RCG_Player p_Player = null;
        public List<RCG_Unit> m_Targets = null;
    }

    /// <summary>
    /// 對戰時的手牌(卡牌資料存在RCG_CardData中)
    /// </summary>
    public class RCG_Card : MonoBehaviour {
        /// <summary>
        /// 卡牌無法打出的狀態
        /// </summary>
        public enum BlockingStatus {
            Cost = 1,//費用不足
            DrawCardAnime,//抽牌演出中
            TriggerCardAnime,//出牌演出中
            Player,//玩家行動中
            NoSkill,//腳色技能不符合
        }
        /// <summary>
        /// Blocking狀態下無法出牌 選擇
        /// </summary>
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
            get; protected set;
        }
        virtual public bool IsCardDisplayerSelected {
            get { return m_CardDisplayer.IsSelected; }
        }
        /// <summary>
        /// target >=3 is enemy
        /// </summary>
        /// <param name="target"></param>
        virtual public bool TriggerCardEffect(TriggerEffectData iTriggerEffectData) {
            //Debug.LogWarning("target:" + target);
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
            m_Data.TriggerEffect(iTriggerEffectData);
            try
            {
                if (RCG_BattleField.ins != null)
                {
                    RCG_BattleField.ins.TriggerCardEffect(0, m_Data);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Exception:" + e);
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
            if(RCG_BattleField.ins.ActiveUnit == null || !m_Data.CheckRequireSkill(RCG_BattleField.ins.ActiveUnit.m_SkillSets))
            {
                BlockSelection(BlockingStatus.NoSkill);
            }
            else
            {
                UnBlockSelection(BlockingStatus.NoSkill);
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
            IsSelected = false;
            p_Player = _Player;
            m_SelectedObject.SetActive(false);
            //m_Button.m_OnPointerUp.AddListener(p_Player.CardRelease);
            m_Button.m_OnClick.AddListener(delegate () {
                if(!IsBlocking) {
                    Debug.Log("SetSelected:" + name);
                    p_Player.SetSelectedCard(this);
                } else {
                    Debug.LogError("SetSelectedCard Fail Blocking:" + m_SelectionBlock.UCL_ToString());
                    p_Player.ClearSelectedCard();
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
            IsSelected = true;
            m_SelectedObject.SetActive(true);
        }
        virtual public void Deselect() {
            IsSelected = false;
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
            m_CardPanel.transform.position = transform.position;
            if (m_Data != null) {
                m_CardPanel.SetActive(true);
            } else {
                m_CardPanel.SetActive(false);
            }
            m_Used = false;
            UpdateCardStatus();
        }
        virtual public void BlockSelection(BlockingStatus iBlock, bool iActiveBlockSelectionObject = true) {
            //Debug.LogWarning("BlockSelection");
            if (IsSelected) Deselect();
            //if(IsSelected) p_Player.ClearSelectedCard();//Deselect
            m_BlockSelectionObject.SetActive(iActiveBlockSelectionObject);
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
                transform.position = transform.parent.position;
                m_CardPanel.transform.position = transform.parent.position;
                transform.rotation = transform.parent.rotation;
                m_CardPanel.transform.rotation = transform.parent.rotation;
                m_CardDisplayer.UnBlockSelection();
                UnBlockSelection(BlockingStatus.DrawCardAnime);
                if(iEndAct != null) iEndAct.Invoke();
            });
        }
        /// <summary>
        /// 出牌演出
        /// </summary>
        /// <param name="iTargetPos"></param>
        /// <param name="iEndAct"></param>
        virtual public void TriggerCardAnime(Transform iTargetPos, System.Action iEndAct) {
            //Vector3 aPosO = transform.position;
            //Quaternion aRotO = transform.rotation;
            transform.position = iTargetPos.position;
            transform.rotation = iTargetPos.rotation;

            m_CardPanel.transform.position = transform.parent.position;
            m_CardPanel.SetActive(true);
            //m_CardDisplayer.BlockSelection();
            BlockSelection(BlockingStatus.TriggerCardAnime, false);
            m_TB_Tweener.StartTween(() => {
                //m_CardDisplayer.UnBlockSelection();
                UnBlockSelection(BlockingStatus.TriggerCardAnime);
                transform.position = transform.parent.position;
                m_CardPanel.transform.position = transform.parent.position;
                transform.rotation = transform.parent.rotation;
                m_CardPanel.transform.rotation = transform.parent.rotation;
                if(iEndAct != null) iEndAct.Invoke();
            });
        }
        virtual public void CardUsed() {
            SetCardData(null);
            p_Player.ClearSelectedCard();
        }
        virtual protected void Update() {

        }
    }
}
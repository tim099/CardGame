using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UCL.Core;
using UCL.Core.UI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RCG {

    public class RCG_Card : MonoBehaviour {
        public GameObject m_CardPanel;
        public UCL.Core.UI.UCL_Button m_Button;
        public UCL.TweenLib.UCL_TB_Tweener m_TB_Tweener;
        public RCG_CardDisplayer m_CardDisplayer;
        public bool m_Used = false;
        protected RCG_CardData m_Data;
        protected RCG_Player p_Player;
        virtual public RCG_CardData Data { get { return m_Data; } }
        virtual public bool IsDragging {
            get { return m_Button.m_Dragging; }
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
            if(!m_Data.TargetCheck(target)) return false;
            if(p_Player.m_Cost < m_Data.m_Cost) return false;
            m_Used = true;
            p_Player.m_Cost -= m_Data.m_Cost;
            m_Data.TriggerEffect(p_Player);
            return true;
        }
        virtual public bool IsEmpty {
            get {
                if(m_Used || m_Data == null) return true;
                return false;
            }
        }
        virtual public void Init(RCG_Player _Player) {
            p_Player = _Player;
            m_Button.m_OnPointerUp.AddListener(p_Player.CardRelease);
            m_CardDisplayer.m_OnSelected.AddListener(() => { transform.SetAsLastSibling(); });
        }
        virtual public void TurnInit(RCG_CardData _Data) {
            SetCardData(_Data);
        }
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
        }
        virtual public void DrawCardAnime(Vector3 start_pos, System.Action end_act) {
            m_CardPanel.transform.position = start_pos;
            m_CardPanel.SetActive(true);
            m_CardDisplayer.BlockSelection();
            m_TB_Tweener.StartTween(()=> {
                m_CardDisplayer.UnBlockSelection();
                if(end_act != null) end_act.Invoke();
            });
        }
        virtual public void CardUsed() {
            SetCardData(null);
        }
        virtual protected void Update() {

        }
    }
}
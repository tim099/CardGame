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
        public Image m_Image;
        public TextMeshProUGUI m_CostText;
        public Text m_NameText;
        public Text m_DescriptionText;
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
        }


        virtual public void SetData(RCG_CardData _Data) {
            m_Data = _Data;
            m_Image.sprite = m_Data.Icon;
            m_NameText.text = m_Data.CardName;
            m_CostText.text = m_Data.GetCost().ToString();
            m_DescriptionText.text = m_Data.Description;
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(gameObject);
#endif
        }
        virtual public void TurnInit(RCG_CardData _Data) {
            SetCardData(_Data);
        }
        virtual public void SetCardData(RCG_CardData _Data) {
            m_Data = _Data;
            m_CardDisplayer.Init(m_Data);
            m_TB_Tweener.Kill();
            if(m_Data != null) {
                m_Button.transform.position = transform.position;
                m_Button.gameObject.SetActive(true);
            } else {
                m_Button.transform.position = transform.position;
                m_Button.gameObject.SetActive(false);
            }
            m_Used = false;
        }
        virtual public void DrawCardAnime(Vector3 start_pos, System.Action end_act) {
            m_Button.transform.position = start_pos;
            m_Button.gameObject.SetActive(true);

            m_TB_Tweener.StartTween(end_act);
        }
        virtual public void CardUsed() {
            SetCardData(null);
        }
    }
}
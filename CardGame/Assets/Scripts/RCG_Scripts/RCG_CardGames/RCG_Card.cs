using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UCL.Core;
using UCL.Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace RCG {

    public class RCG_Card : MonoBehaviour {
        public RCG_Player p_Player;
        public Image m_Image;
        public Text m_NameText;
        public Text m_DescriptionText;
        public Text m_CostText;
        public UCL.Core.UI.UCL_Button m_Button;
        public bool m_Used = false;
        protected RCG_CardData m_Data;

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
            return true;
        }
        //void Onvalidate() {
        //    if(m_Button == null) m_Button = UCL.Core.GameObjectLib.SearchChild<UCL_Button>(transform);
        //}
        virtual public void Init(RCG_Player _Player) {
            p_Player = _Player;
            m_Button.m_OnPointerUp.AddListener(p_Player.CardRelease);

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(gameObject);
#endif
        }
        virtual public void TurnInit(RCG_CardData _Data) {
            m_Data = _Data;
            m_Image.SetSprite(m_Data.Icon);
            m_NameText.SetText(m_Data.CardName);
            m_DescriptionText.SetText(m_Data.Description);
            m_CostText.SetText(m_Data.m_Cost);

            m_Used = false;
            m_Button.transform.position = transform.position;
            m_Button.gameObject.SetActive(true);
        }
        virtual public void CardUsed() {
            m_Button.gameObject.SetActive(false);
        }
    }
}
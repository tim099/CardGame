using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RCG {
    //[RequireComponent(typeof(Slider))]
    public class RCG_UI_HPbar : MonoBehaviour
    {
        public Text m_ArmorText = null;
        public TextMeshProUGUI m_hp_text;
        [SerializeField] private Slider m_HpMoveSlider = null;
        [SerializeField] private Slider m_HPslider;
        private RCG_Unit m_unit;
        private int m_Timer = 0;
        private int m_DisplayHP = 0;
        private void Awake() {
            if(!m_HPslider){
                m_HPslider = gameObject.GetComponent<Slider>();
            }
            if(!m_unit){
                m_unit = gameObject.GetComponentInParent<RCG_Unit>();
            }
            if (!m_unit)
            {
                m_unit = gameObject.GetComponentInParent<RCG_Unit>();
            }
            // m_hp_text.text = m_hp_slider.value + "/" + m_hp_slider.maxValue;
            UpdateHUD();
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (++m_Timer > 2)
            {
                m_Timer = 0;
                if (m_DisplayHP != m_unit.Hp)
                {
                    int aDel = m_unit.Hp - m_DisplayHP;
                    //Debug.LogWarning("aDel:" + aDel);
                    int aMove = Mathf.RoundToInt(0.1f * aDel);
                    if (aMove == 0)
                    {
                        aMove = aDel > 0 ? 1 : -1;
                    }
                    m_DisplayHP += aMove;
                    m_HpMoveSlider.value = m_DisplayHP;
                }
            }

        }
        public void UpdateArmor(int iValue)
        {
            m_ArmorText.text = iValue.ToString();
        }
        public void UpdateHp()
        {
            m_HPslider.value = m_unit.Hp;
            m_HpMoveSlider.maxValue = m_HPslider.maxValue = m_unit.MaxHp;
            m_hp_text.text = m_unit.Hp + "/" + m_unit.MaxHp;
        }
        public void UpdateHUD(){
            m_DisplayHP = m_unit.Hp;
            m_HpMoveSlider.value = m_HPslider.value = m_unit.Hp;
            m_HpMoveSlider.maxValue = m_HPslider.maxValue = m_unit.MaxHp;
            m_hp_text.text = m_unit.Hp + "/" + m_unit.MaxHp;
        }
    }
}

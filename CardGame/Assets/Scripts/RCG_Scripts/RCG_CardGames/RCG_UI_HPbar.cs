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
        private float m_DisplayHP = 0;
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

        // Update is called once per frame
        void Update()
        {
            if (m_DisplayHP != m_unit.Hp)
            {
                const float aSpeed = 0.15f;
                float aDel = m_unit.Hp - m_DisplayHP;
                float aDelVal = Mathf.Abs(aDel);
                if (aDelVal > 0.1f)
                {
                    m_DisplayHP += (aDel > 0 ? aSpeed : -aSpeed) * Mathf.Sqrt(aDelVal);
                }
                else
                {
                    m_DisplayHP = m_unit.Hp;
                }
                //Debug.LogWarning("aDel:" + aDel);

                if (aDel < 0)
                {
                    m_HpMoveSlider.value = m_DisplayHP;
                }
                else
                {
                    m_HPslider.value = m_DisplayHP;
                }
            }
        }
        public void UpdateArmor(int iValue)
        {
            m_ArmorText.text = iValue.ToString();
        }
        public void UpdateHp()
        {
            if(m_unit.Hp <= m_HPslider.value)
            {
                m_HPslider.value = m_unit.Hp;
                m_HpMoveSlider.maxValue = m_HPslider.maxValue = m_unit.MaxHp;
            }
            else
            {
                m_HpMoveSlider.value = m_unit.Hp;
                m_HpMoveSlider.maxValue = m_HPslider.maxValue = m_unit.MaxHp;
            }

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

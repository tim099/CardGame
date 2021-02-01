using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RCG {
    [RequireComponent(typeof(Slider))]
    public class RCG_UI_HPbar : MonoBehaviour
    {

        public TextMeshProUGUI m_hp_text;
        private Slider m_hp_slider;
        private RCG_Unit m_unit;

        private void Awake() {
            if(!m_hp_slider){
                m_hp_slider = gameObject.GetComponent<Slider>();
            }
            if(!m_unit){
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
            
        }

        public void UpdateHUD(){
            m_hp_slider.value = m_unit.Hp;
            m_hp_slider.maxValue = m_unit.MaxHp;
            m_hp_text.text = m_unit.Hp + "/" + m_unit.MaxHp;
        }
    }
}

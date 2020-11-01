﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RCG{
    public class RCG_Status : MonoBehaviour
    {
        public RCG_Unit m_target;
        protected int m_amount = 0;
        public int _m_round  = 1;
        protected int m_round {
            get{return _m_round;}
            set{
                _m_round = value;
                m_counter_text.text = value.ToString();
            }
        }

        public TextMeshProUGUI m_counter_text;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void awake(){
            if(!m_counter_text){
                m_counter_text = GetComponentInChildren<TextMeshProUGUI>();
                m_counter_text.text = m_round.ToString();
            }
        }

        virtual public void StatusStart(){

        }

        virtual public void StatusStack(){
            
        }

        virtual public void StatusTurnEnd(){
            
        }

        virtual public void StatusTurnStart(){
            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RCG{
    public class RCG_Status : MonoBehaviour
    {
        protected int m_amount = 0;
        protected int m_round = 0;

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
                
            }
        }

        public void StatusStart(){

        }

        public void StatusStack(){
            
        }

        public void StatusTurnEnd(){
            
        }

        public void StatusTurnStart(){
            
        }
    }
}

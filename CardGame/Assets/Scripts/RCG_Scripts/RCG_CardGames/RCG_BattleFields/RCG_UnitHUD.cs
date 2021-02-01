using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG {
    [RequireComponent(typeof(RCG_Unit))]
    public class RCG_UnitHUD : MonoBehaviour
    {
        private RCG_UI_HPbar m_hp_bar;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void Awake(){
            if(!m_hp_bar){
                m_hp_bar = gameObject.GetComponentInChildren<RCG_UI_HPbar>();
            }
        }

        public void UpdateHUD(){
            m_hp_bar.UpdateHUD();
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCL.Core.UI;

namespace RCG {
    [UCL.Core.ATTR.EnableUCLEditor]
    public class RCG_BattleField : MonoBehaviour
    {
        static public RCG_BattleField ins = null;
        public List<RCG_Unit> m_units;
        bool m_Entered = false;
        // Start is called before the first frame update
        void Start(){}

        // Update is called once per frame
        void Update(){}

        private void Awake() {
            //Init();
        }
        virtual public void EnterBattle() {
            if(m_Entered) {
                Debug.LogError("EnterBattle() Fail!! Already Entered!!");
                return;
            }
            m_Entered = true;
            gameObject.SetActive(true);
        }
        public void ExitBattle() {
            if(!m_Entered) {
                Debug.LogError("ExitBattle() Fail!! Not Entered!!");
                return;
            }
            m_Entered = false;
            gameObject.SetActive(false);
        }
        virtual public void Init() {
            ins = this;
            // RCG_Unit[] units = GetComponentsInChildren<RCG_Unit>();
            UCL_RectTransformCollider[] colliders = GetComponentsInChildren<UCL_RectTransformCollider>();
            // Debug.Log(units.Length);
            for(int i=0; i<colliders.Length; i++){
                UCL_RectTransformCollider c = colliders[i];
                RCG_Unit u = c.gameObject.GetComponent<RCG_Unit>();
                if(u){
                    m_units.Add(u);
                }
                else{
                    m_units.Add(null);
                }
            }
        }

        public void Test() {
            
        }

        public void CreateUnits() {

        }

        public void TurnEnd() {
            foreach(RCG_Unit u in m_units){
                if(u == null){
                    continue;
                }
                u.EndTurn();
            }
        }

        public void TriggerCardEffect(int target, RCG_CardData card_data){
            Debug.Log("TriggerCardEffect");
            RCG_CardEffectHandler.TriggerCardEffectOnUnits(m_units ,target, card_data);
        }
    }
}

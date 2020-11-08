using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG {
    [UCL.Core.ATTR.EnableUCLEditor]
    public class RCG_Unit : MonoBehaviour {
        public int _m_Hp = 0;
        public int _m_MaxHp = 0;
        private Queue<RCG_UnitAction> m_action_queue;
        private RCG_UnitAction m_action = null;

        public int m_Hp{
            get{return _m_Hp;}
            set{
                _m_Hp = value;
                m_unit_HUD.UpdateHUD();
            }
        }
        public int m_MaxHp{
            get{return _m_MaxHp;}
            set{
                _m_MaxHp = value;
                m_unit_HUD.UpdateHUD();
            }
        }
        public List<RCG_Status> m_status_list;
        private RCG_UnitHUD m_unit_HUD;

        private void Awake() {
            if(!m_unit_HUD){
                m_unit_HUD = gameObject.GetComponent<RCG_UnitHUD>();
                //QWQ
            }
        }

        public int RestoreHP(int amount){
            m_Hp += amount;
            return 0;
        }

        public int DamageHP(int amount){
            m_Hp -= amount;
            return 0;
        }

        public void Die(){
            
        }

        public void EndTurn(){
            for(int i = 0; i < m_status_list.Count; i++) {
                if(m_status_list[i] != null){
                    m_status_list[i].StatusTurnEnd();
                }
                else{
                    m_status_list.RemoveAt(i);
                }
            }
        }

        [UCL.Core.ATTR.UCL_FunctionButton]
        public void Test(){
            DamageHP(1);
        }

        public void AddAction(RCG_UnitAction action){
            m_action_queue.Enqueue(action);
        }

        public void TakeAction(){
            m_action.TakeAction(this);
        }

        public void Start()
        {
            m_action_queue = new Queue<RCG_UnitAction>();
        }

        public void Update(){
            if(m_action_queue.Count <= 0){
                return;
            }
            else if(m_action == null){
                m_action = m_action_queue.Dequeue();
                TakeAction();
            }
            else{
                m_action.m_duration -= Time.deltaTime;
                Debug.Log(m_action.m_duration);
                if(m_action.m_duration < 0){
                    m_action = null;
                }
            }
        }

    }
}
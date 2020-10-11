using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG {
    [UCL.Core.ATTR.EnableUCLEditor]
    public class RCG_Unit : MonoBehaviour {
        public int _m_Hp = 0;
        public int _m_MaxHp = 0;
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

        [UCL.Core.ATTR.UCL_FunctionButton]
        public void Test(){
            DamageHP(1);
        }

    }
}
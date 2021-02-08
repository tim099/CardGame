using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RCG {
    public class RCG_CardGameTest : MonoBehaviour {
        public RCG_BattleManager m_BattleManager;
        public RCG_CardGame m_Game;
        public bool m_AutoInit = true;
        bool m_Inited = false;
        //int m_Timer = 0;
        private void Start() {
            if(m_AutoInit) Init();
        }
        public void Init() {
            if(m_Inited) return;
            m_Inited = true;
            m_BattleManager.Init();
            m_BattleManager.EnterBattle();
        }
        private void Update()
        {
            //m_Timer++;
            //if(m_Timer == 3 && m_AutoInit)
            //{
            //    Init();
            //}
        }
    }
}
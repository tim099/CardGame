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
            //for (int i = 0; i < 256; i++)
            //{
            //    Debug.LogWarning("i:" + i + ",char:" + System.Convert.ToChar(i));
            //}
            //byte[] aArr = new byte[256];
            //for (int i = 0; i < aArr.Length; i++)
            //{
            //    aArr[i] = (byte)i;
            //    string aHex = UCL.Core.MarshalLib.Lib.ByteToHexString(aArr[i]);
            //    Debug.LogError("aHex:" + aHex+",Val:"+ UCL.Core.MarshalLib.Lib.HexToByte(aHex));
            //}
            //string aHexStr = UCL.Core.MarshalLib.Lib.ByteArrayToHexString(aArr);
            //Debug.LogError("aHexStr:" + aHexStr);
            //Debug.LogError("aHexaArr:" + UCL.Core.MarshalLib.Lib.HexStringToByteArray(aHexStr).UCL_ToString());
            if (m_AutoInit) Init();
        }
        public void Init() {
            if(m_Inited) return;
            m_Inited = true;
            m_BattleManager.Init();
            m_BattleManager.EnterBattle(new RCG_MonsterSet());
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
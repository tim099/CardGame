using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG
{
    public class RCG_CharacterManager : MonoBehaviour
    {
        public static RCG_CharacterManager ins = null;
        public RCG_CharacterUI m_CharacterUI;
        public RCG_CharacterPanel m_CharacterPanel;

        List<RCG_CharacterData> m_CharacterDatas;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void Awake()
        {
            ins = this;
            m_CharacterDatas = RCG_DataService.ins.m_CharacterDatas;
        }
    }
}

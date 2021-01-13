using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RCG {
    public class RCG_MapManager : MonoBehaviour {
        public RCG_BattleManager m_BattleManager;

        public RectTransform m_MapRoot = null;
        public ScrollRect m_MapScrollRect = null;
        public string m_LoadMapName = "";
        public RCG_MapItems m_MapItems = null;


        public RCG_Map m_CurMap = null;
        private void Awake() {
            Init();
        }
        virtual public void Init() {
            m_BattleManager.Init();
            m_LoadMapName = RCG_GameManager.ins.m_LoadMapName;
            LoadMap(m_LoadMapName);
        }
        virtual public void LoadMap(string map_name) {
            if(m_CurMap != null) {
                m_MapItems.transform.SetParent(transform);
                Destroy(m_CurMap.gameObject);
                m_CurMap = null;
            }
            var map = Resources.Load<RCG_Map>(PathConst.MapResource + "/" + map_name);
            m_CurMap = Instantiate(map, m_MapRoot);
            m_CurMap.name = map_name;
            m_CurMap.Init(m_MapItems);
            m_MapScrollRect.content = m_CurMap.GetComponent<RectTransform>();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RCG {
    public class RCG_MapManager : MonoBehaviour {
        public RCG_Map[] m_Maps = null;
        public RectTransform m_MapRoot = null;
        public ScrollRect m_MapScrollRect = null;
        public string m_LoadMapName = "";

        public RCG_Map m_CurMap = null;
        private void Awake() {
            Init();
        }
        virtual public void Init() {
            LoadMap(m_LoadMapName);
        }
        virtual public void LoadMap(string map_name) {
            if(m_CurMap != null) {
                Destroy(m_CurMap.gameObject);
                m_CurMap = null;
            }
            for(int i = 0; i < m_Maps.Length; i++) {
                var map = m_Maps[i];
                if(map.name == map_name) {
                    m_CurMap = Instantiate(map, m_MapRoot);
                    m_CurMap.name = map_name;
                    m_CurMap.Init();
                    m_MapScrollRect.content = m_CurMap.GetComponent<RectTransform>();
                    return;
                }
            }
        }
    }
}
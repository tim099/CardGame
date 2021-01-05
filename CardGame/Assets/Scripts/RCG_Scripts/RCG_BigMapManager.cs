using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RCG
{
    public class RCG_BigMapManager : MonoBehaviour
    {
        public RCG_BigMap[] m_BigMaps;
        public RCG_BigMap m_CurBigMap;
        public RCG_EmbarkUI m_EmbarkUI;
        public RectTransform m_BigMapRoot;
        public ScrollRect m_BigMapScrollRect;
        public string m_LoadMapName = "";
        private void Awake() {
            Init();
        }
        virtual public void Init() {
            m_EmbarkUI.Init();

            LoadBigMap(m_LoadMapName);
        }
        virtual public void LoadBigMap(string map_name) {
            if(m_CurBigMap != null) {
                Destroy(m_CurBigMap.gameObject);
                m_CurBigMap = null;
            }
            for(int i = 0; i < m_BigMaps.Length; i++) {
                var map = m_BigMaps[i];
                if(map.name == map_name) {
                    m_CurBigMap = Instantiate(map, m_BigMapRoot);
                    m_CurBigMap.name = map_name;
                    m_BigMapScrollRect.content = m_CurBigMap.GetComponent<RectTransform>();
                    return;
                }
            }
        }
    }
}
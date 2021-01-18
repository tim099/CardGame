using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG {
    public class RCG_CardEditor : MonoBehaviour {
        const string FolderPathKey = "RCG_CardEditor_FolderPath";
        public string m_FolderPath = "";
        public string m_CardSetting = "";
        Vector2 m_ScrollPos = default;
        Rect m_WindowRect = default;
        private void Awake() {
            Init();
        }
        virtual public void Init() {
            if(PlayerPrefs.HasKey(FolderPathKey)) {
                m_FolderPath = PlayerPrefs.GetString(FolderPathKey);
            }
        }
        virtual protected void EditWindow(int id) {
            m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos);
            GUILayout.BeginVertical();
            using(var scope = new GUILayout.HorizontalScope("box")) {
                if(UCL.Core.UI.UCL_GUILayout.ButtonAutoSize("Explore folder", 22)) {
#if UNITY_EDITOR
                    m_FolderPath = UCL.Core.FileLib.EditorLib.OpenAssetsFolderExplorer(m_FolderPath);
#endif
                }
            }
            GUILayout.BeginHorizontal();
            using(var scope = new GUILayout.HorizontalScope("box")) {
                //DrawRoot();
            }

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }
        private void OnGUI() {
            const int edge = 5;//5 pixel
            m_WindowRect = new Rect(edge, edge, Screen.width - 2 * edge, Screen.height - 2 * edge);
            m_WindowRect = GUILayout.Window(133125, m_WindowRect, EditWindow, "CardEditor");
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
namespace RCG {
    public class RCG_CardEditorWindow : EditorWindow {
        public static RCG_CardEditorWindow ShowWindow(RCG_CardEditor target) {
            Debug.LogWarning("UCL_SceneSwitcherWindow ShowWindow() !!");
            var window = EditorWindow.GetWindow<RCG_CardEditorWindow>("CardEditor");
            window.Init(target);
            return window;
        }
        Rect m_GridRegion = new Rect();
        RCG_CardEditor m_CardEditor;
        public void Init(RCG_CardEditor _CardEditor) {
            m_CardEditor = _CardEditor;
            m_CardEditor.Init();
        }
        private void OnGUI() {
            if(m_CardEditor == null) return;
            m_CardEditor.EditWindow(0);
            if(Event.current.type == EventType.Repaint) {
                var newRgn = GUILayoutUtility.GetLastRect();
                if(newRgn != m_GridRegion) {
                    m_GridRegion = newRgn;
                    Repaint();
                }
            }
        }
    }
}

#endif
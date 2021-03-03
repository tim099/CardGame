using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
namespace RCG
{
    public class RCG_ItemEditorWindow : EditorWindow
    {
        public static RCG_ItemEditorWindow ShowWindow(RCG_ItemEditor target)
        {
            var window = EditorWindow.GetWindow<RCG_ItemEditorWindow>("ItemEditor");
            window.Init(target);
            return window;
        }
        Rect m_GridRegion = new Rect();
        public RCG_ItemEditor m_ItemEditor;
        public void Init(RCG_ItemEditor _CardEditor)
        {
            m_ItemEditor = _CardEditor;
        }
        private void OnGUI()
        {
            if (m_ItemEditor == null) return;
            m_ItemEditor.Init();
            m_ItemEditor.EditWindow(0);
            if (Event.current.type == EventType.Repaint)
            {
                var newRgn = GUILayoutUtility.GetLastRect();
                if (newRgn != m_GridRegion)
                {
                    m_GridRegion = newRgn;
                    Repaint();
                }
            }
        }
    }
}
#endif
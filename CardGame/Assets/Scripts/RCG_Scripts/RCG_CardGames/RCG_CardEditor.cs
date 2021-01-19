using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
namespace RCG {
    public class RCG_CardEditor : MonoBehaviour {
        public class CardEditData {
            public CardEditData(RCG_CardData _CardData,string _FilePath) {
                m_CardData = _CardData;
                m_FilePath = _FilePath;
            }
            public string m_FilePath = "";
            public RCG_CardData m_CardData = null;
        }
        const string FolderPathKey = "RCG_CardEditor_FolderPath";
        public string m_FolderPath = "";
        public string m_CardSetting = "";
        public string ImagePath { get {
                return Path.Combine(UCL.Core.FileLib.Lib.RemoveFolderPath(m_FolderPath,1), "Images");
            } }
        Vector2 m_ScrollPos_SelectCard = default;
        Vector2 m_ScrollPos_EditCard = default;
        Rect m_WindowRect = default;
        List<string> m_CardDataPaths = null;
        CardEditData m_EditingData = null;
        int m_CreateEffectID = 0;
        bool m_CreateEffectOpened = false;
        private void Awake() {
            Init();
        }
        private void OnDestroy() {
            PlayerPrefs.SetString(FolderPathKey, m_FolderPath);
        }
        virtual public void Init() {
            if(PlayerPrefs.HasKey(FolderPathKey)) {
                m_FolderPath = PlayerPrefs.GetString(FolderPathKey);
            }
#if UNITY_EDITOR
            if(m_FolderPath == null) m_FolderPath = Application.dataPath;
#endif
            if(!Directory.Exists(m_FolderPath)) {
                Directory.CreateDirectory(m_FolderPath);
            }
            RefreshCardDataPaths();
        }
        virtual public void RefreshCardDataPaths() {
            var files = UCL.Core.FileLib.Lib.GetFiles(m_FolderPath);
            if(m_CardDataPaths == null) {
                m_CardDataPaths = new List<string>();
            } else {
                m_CardDataPaths.Clear();
            }
            if(files != null) {
                int discard_len = m_FolderPath.Length + 1;
                for(int i = 0; i < files.Length; i++) {
                    var card_path = files[i];
                    m_CardDataPaths.Add(card_path.Substring(discard_len,card_path.Length - discard_len));
                }
                //m_CardDataNames = files.ToList();
            }
        }
        virtual protected void SelectCardWindow() {
            m_ScrollPos_SelectCard = GUILayout.BeginScrollView(m_ScrollPos_SelectCard);
            GUILayout.BeginVertical();
            using(var scope = new GUILayout.VerticalScope("box")) {
                GUILayout.BeginHorizontal();
#if UNITY_EDITOR
                if(UCL.Core.UI.UCL_GUILayout.ButtonAutoSize("Explore folder", 22)) {
                    m_FolderPath = UCL.Core.FileLib.EditorLib.OpenFolderExplorer(m_FolderPath);
                    //bool flag = UnityEditor.EditorUtility.DisplayDialog("Test", "HiHi", "Ok", "QAQ");
                    //Debug.LogError("flag:" + flag);
                }
#endif
                if(UCL.Core.UI.UCL_GUILayout.ButtonAutoSize("Refresh folder", 22)) {
                    RefreshCardDataPaths();
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                UCL.Core.UI.UCL_GUILayout.LabelAutoSize("Folder Path", 18);
                m_FolderPath = GUILayout.TextField(m_FolderPath);//, GUILayout.Height(26)
                GUILayout.EndHorizontal();
            }
            GUILayout.BeginHorizontal();
            using(var scope = new GUILayout.VerticalScope("box")) {
                for(int i = 0; i < m_CardDataPaths.Count; i++) {
                    string card_path = m_CardDataPaths[i];
                    if(GUILayout.Button(card_path)) {
                        string data = File.ReadAllText(Path.Combine(m_FolderPath, card_path));
                        var json = UCL.Core.JsonLib.JsonData.ParseJson(data);
                        m_EditingData = new CardEditData(new RCG_CardData(json), card_path);
                    }
                }
            }

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }
        virtual protected void EditCardWindow() {
            if(m_EditingData == null) return;
            var card_data = m_EditingData.m_CardData;
            m_ScrollPos_EditCard = GUILayout.BeginScrollView(m_ScrollPos_EditCard);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            UCL.Core.UI.UCL_GUILayout.LabelAutoSize("Edit Card", 22);
            if(UCL.Core.UI.UCL_GUILayout.ButtonAutoSize("Back", 22)) {
                m_EditingData = null;
                return;
            }
            if(UCL.Core.UI.UCL_GUILayout.ButtonAutoSize("Save", 22)) {
                var path = Path.Combine(m_FolderPath, m_EditingData.m_FilePath);
                var data = card_data.ToJson();
                string save_data = data.ToJsonBeautify();
                Debug.LogWarning("path:" + path);
                Debug.LogWarning("save_data:" + save_data);
                File.WriteAllText(path, save_data);
                RefreshCardDataPaths();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            using(var scope = new GUILayout.VerticalScope("box", GUILayout.Width(300))) {
                m_EditingData.m_FilePath = UCL.Core.UI.UCL_GUILayout.TextField("SaveName", m_EditingData.m_FilePath);

                card_data.CardName = UCL.Core.UI.UCL_GUILayout.TextField("CardName", card_data.CardName);
                card_data.CardName = UCL.Core.UI.UCL_GUILayout.TextField("IconName", card_data.IconName);     
                card_data.Cost = UCL.Core.UI.UCL_GUILayout.IntField("Cost", card_data.Cost);


            }
            using(var scope = new GUILayout.VerticalScope("box")) {
                GUILayout.BeginHorizontal();
                m_CreateEffectID = UCL.Core.UI.UCL_GUILayout.
                    Popup(m_CreateEffectID, RCG_CardEffectCreator.m_EffectNameList.ToArray(), ref m_CreateEffectOpened);
                if(GUILayout.Button("Add", GUILayout.Width(60))) {
                    card_data.AddCardEffect(RCG_CardEffectCreator.Create(m_CreateEffectID));
                }
                GUILayout.EndHorizontal();
                card_data.OnGUI();

            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }
        virtual protected void EditWindow(int id) {
            if(m_EditingData == null) {
                SelectCardWindow();
            } else {
                EditCardWindow();
            }
            
        }
        private void OnGUI() {
            const int edge = 5;//5 pixel
            m_WindowRect = new Rect(edge, edge, Screen.width - 2 * edge, Screen.height - 2 * edge);
            m_WindowRect = GUILayout.Window(133125, m_WindowRect, EditWindow, "CardEditor");
        }
        private void Update() {

        }
    }
}


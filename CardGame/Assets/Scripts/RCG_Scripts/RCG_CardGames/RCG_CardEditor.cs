using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
namespace RCG {
    [UCL.Core.ATTR.EnableUCLEditor]
    public class RCG_CardEditor : MonoBehaviour {
        static protected Dictionary<string, object> m_CardEditTmpDatas = new Dictionary<string, object>();
        public static RCG_CardEditor ins = null;
        public class CardEditData {
            public CardEditData(RCG_CardData _CardData,string _FilePath) {
                m_CardData = _CardData;
                m_FilePath = _FilePath;
            }
            public string m_FilePath = "";
            public RCG_CardData m_CardData = null;
            public Texture2D m_IconTexture = null;
        }
        const string FolderPathKey = "RCG_CardEditor_FolderPath";
        public string m_FolderPath = "";
        public string m_CardSetting = "";
        public string IconPath { get {
                return Path.Combine(UCL.Core.FileLib.Lib.RemoveFolderPath(m_FolderPath,1), "Icons");
            } }
        protected Vector2 m_ScrollPos_SelectCard = default;
        protected Vector2 m_ScrollPos_EditCard = default;
        protected Rect m_WindowRect = default;
        protected List<string> m_CardDataPaths = null;
        protected List<string> m_CardIconPaths = new List<string>();
        protected CardEditData m_EditingData = null;
        protected int m_CreateEffectID = 0;
        protected bool m_CreateEffectOpened = false;
        protected bool m_Inited = false;
        
        private void Awake() {
            Init();
        }
        private void OnDestroy() {
            PlayerPrefs.SetString(FolderPathKey, m_FolderPath);
        }
        virtual public void Init() {
            if(m_Inited) return;
            m_Inited = true;
            ins = this;
            if(PlayerPrefs.HasKey(FolderPathKey)) {
                m_FolderPath = PlayerPrefs.GetString(FolderPathKey);
            }
            if(string.IsNullOrEmpty(m_FolderPath) || !Directory.Exists(m_FolderPath)) ResetFolderPath();

            //if(!string.IsNullOrEmpty(m_FolderPath) && !Directory.Exists(m_FolderPath)) {
            //    Directory.CreateDirectory(m_FolderPath);
            //}
            RefreshCardDataPaths();
        }
        virtual public void RefreshCardDataPaths() {
            if(m_CardDataPaths == null) {
                m_CardDataPaths = new List<string>();
            } else {
                m_CardDataPaths.Clear();
            }
            if(string.IsNullOrEmpty(m_FolderPath)) return;
            var files = UCL.Core.FileLib.Lib.GetFiles(m_FolderPath);

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

                if(UCL.Core.UI.UCL_GUILayout.ButtonAutoSize("Explore folder", 22)) {
#if UNITY_EDITOR
                    m_FolderPath = UCL.Core.FileLib.EditorLib.OpenFolderExplorer(m_FolderPath);
                    RefreshCardDataPaths();
#endif
                    //bool flag = UnityEditor.EditorUtility.DisplayDialog("Test", "HiHi", "Ok", "QAQ");
                    //Debug.LogError("flag:" + flag);
                }
                if(UCL.Core.UI.UCL_GUILayout.ButtonAutoSize("Reset folder", 22)) {
                    ResetFolderPath();
                }
                if(UCL.Core.UI.UCL_GUILayout.ButtonAutoSize("Refresh CardDatas", 22)) {
                    RefreshCardDataPaths();
                }
                if(UCL.Core.UI.UCL_GUILayout.ButtonAutoSize("Create CardData", 22)) {
                    SetEditCardData(new CardEditData(new RCG_CardData(), "NewCard.json"));
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
                    GUILayout.BeginHorizontal();
                    if(GUILayout.Button(card_path)) {
                        string data = File.ReadAllText(Path.Combine(m_FolderPath, card_path));
                        var json = UCL.Core.JsonLib.JsonData.ParseJson(data);
                        SetEditCardData(new CardEditData(new RCG_CardData(json), card_path));
                    }
                    if(UCL.Core.UI.UCL_GUILayout.ButtonAutoSize("Delete", 18)) {
                        File.Delete(Path.Combine(m_FolderPath, card_path));
                        RefreshCardDataPaths();
                    }
                    GUILayout.EndHorizontal();
                }
            }

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }
        virtual protected void ResetFolderPath() {
            m_FolderPath = RCG_CardData.CardDataPath;
            Debug.LogWarning("System.Environment.CurrentDirectory:" + System.Environment.CurrentDirectory);
            var aCurrentDirectory = System.Environment.CurrentDirectory.Replace('\\', '/')+"/";
//#if UNITY_EDITOR
            if(m_FolderPath.Contains(aCurrentDirectory)) {
                m_FolderPath = m_FolderPath.Replace(aCurrentDirectory, string.Empty);
            }
//#endif
            RefreshCardDataPaths();
        }
        //static public object GetCardEditTmpData(string iKey, object iDefaultValue) {
        //    if(ins == null) return iDefaultValue;
        //    if(!ins.m_CardEditTmpDatas.ContainsKey(iKey)) ins.m_CardEditTmpDatas.Add(iKey, iDefaultValue);
        //    return ins.m_CardEditTmpDatas[iKey];
        //}
        static public T GetCardEditTmpData<T>(string iKey, T iDefaultValue) {
            if(m_CardEditTmpDatas == null) return iDefaultValue;
            if(!m_CardEditTmpDatas.ContainsKey(iKey)) m_CardEditTmpDatas.Add(iKey, iDefaultValue);
            return (T)m_CardEditTmpDatas[iKey];
        }
        static public void SetCardEditTmpData<T>(string iKey, T iValue) {
            if(m_CardEditTmpDatas == null) return;
            m_CardEditTmpDatas[iKey] = iValue;
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
            m_EditingData.m_FilePath = UCL.Core.UI.UCL_GUILayout.TextField("SaveName", m_EditingData.m_FilePath);
            if(UCL.Core.UI.UCL_GUILayout.ButtonAutoSize("Save", 22)) {
                var path = Path.Combine(m_FolderPath, m_EditingData.m_FilePath);
                var data = card_data.ToJson();
                string save_data = data.ToJsonBeautify();
                Debug.LogWarning("path:" + path);
                Debug.LogWarning("save_data:" + save_data);
                File.WriteAllText(path, save_data);
                RefreshCardDataPaths();
            }
#if UNITY_STANDALONE_WIN
            if(UCL.Core.UI.UCL_GUILayout.ButtonAutoSize("OpenFile", 22)) {
                var path = Path.Combine(m_FolderPath, m_EditingData.m_FilePath);
                UCL.Core.FileLib.WindowsLib.OpenExplorer(path);
            }
#endif

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            using(var scope = new GUILayout.VerticalScope("box", GUILayout.Width(300))) {

                card_data.CardName = UCL.Core.UI.UCL_GUILayout.TextField("CardName", card_data.CardName);
                //card_data.IconName = UCL.Core.UI.UCL_GUILayout.TextField("IconName", card_data.IconName);

                {
                    string aFieldName = "CardType";
                    GUILayout.BeginHorizontal();
                    UCL.Core.UI.UCL_GUILayout.LabelAutoSize(aFieldName);
                    bool flag = GetCardEditTmpData(aFieldName, false);
                    card_data.CardType = UCL.Core.UI.UCL_GUILayout.Popup(card_data.CardType, ref flag);
                    SetCardEditTmpData(aFieldName, flag);
                    GUILayout.EndHorizontal();
                }
                {
                    string aFieldName = "TargetType";
                    GUILayout.BeginHorizontal();
                    UCL.Core.UI.UCL_GUILayout.LabelAutoSize(aFieldName);
                    bool flag = GetCardEditTmpData(aFieldName, false);
                    card_data.TargetType = UCL.Core.UI.UCL_GUILayout.Popup(card_data.TargetType, ref flag);
                    SetCardEditTmpData(aFieldName, flag);
                    GUILayout.EndHorizontal();
                }
                //TargetType
                {
                    bool flag = GetCardEditTmpData("CardIconPath", false);
                    int index = m_CardIconPaths.FindIndex(a => a == card_data.IconName);
                    GUILayout.BeginHorizontal();
                    UCL.Core.UI.UCL_GUILayout.LabelAutoSize("IconName");
                    int new_index = UCL.Core.UI.UCL_GUILayout.Popup(index, m_CardIconPaths, ref flag);
                    GUILayout.EndHorizontal();
                    SetCardEditTmpData("CardIconPath", flag);
                    if(new_index != index) {
                        card_data.IconName = m_CardIconPaths[new_index];
                        UpdateCardIcon();
                    }
                }
                card_data.Cost = UCL.Core.UI.UCL_GUILayout.IntField("Cost", card_data.Cost);
                if(m_EditingData.m_IconTexture != null) GUILayout.Box(m_EditingData.m_IconTexture,
                    GUILayout.Width(64), GUILayout.Height(64));

            }
            using(var scope = new GUILayout.VerticalScope("box")) {
                GUILayout.BeginHorizontal();
                m_CreateEffectID = UCL.Core.UI.UCL_GUILayout.
                    Popup(m_CreateEffectID, RCG_CardEffectCreator.m_EffectNameList, ref m_CreateEffectOpened);
                if(GUILayout.Button("Add", GUILayout.Width(60))) {
                    card_data.AddCardEffect(RCG_CardEffectCreator.Create(m_CreateEffectID));
                }
                GUILayout.EndHorizontal();
                card_data.DrawCardEffects();

            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }
        virtual protected void SetEditCardData(CardEditData data) {
            if(m_EditingData != null) {
                if(m_EditingData.m_IconTexture != null) {
                    DestroyImmediate(m_EditingData.m_IconTexture);
                }
            }

            m_EditingData = data;
            m_CardEditTmpDatas.Clear();
            m_CardIconPaths.Clear();
            var icon_path = IconPath;
            if(string.IsNullOrEmpty(icon_path)) return;
            var files = UCL.Core.FileLib.Lib.GetFiles(icon_path);

            if(files != null) {
                int discard_len = icon_path.Length + 1;
                for(int i = 0; i < files.Length; i++) {
                    var card_path = files[i];
                    m_CardIconPaths.Add(card_path.Substring(discard_len, card_path.Length - discard_len));
                }
            }

            UpdateCardIcon();
        }
        virtual protected void UpdateCardIcon() {
            if(m_EditingData == null) return;
            var card_data = m_EditingData.m_CardData;
            if(string.IsNullOrEmpty(card_data.IconName)) return;
            string icon_path = Path.Combine(IconPath, card_data.IconName);
            Debug.LogWarning("icon_path:" + icon_path);
            if(File.Exists(icon_path)) {
                var fileData = File.ReadAllBytes(icon_path);
                var tex = new Texture2D(2, 2);
                tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
                if(m_EditingData.m_IconTexture != null) {
                    DestroyImmediate(m_EditingData.m_IconTexture);
                }
                m_EditingData.m_IconTexture = tex;
            } else {
                Debug.LogError("icon_path:" + icon_path + ",not exist!!");
            }
        }
#if UNITY_EDITOR
        [UCL.Core.ATTR.UCL_FunctionButton]
        public void ShowEditWindow() {
            RCG_CardEditorWindow.ShowWindow(this);
        }
#endif

        virtual public void EditWindow(int id) {
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


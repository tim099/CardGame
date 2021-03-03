using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using UCL.Core.JsonLib;
namespace RCG
{
    [UCL.Core.ATTR.EnableUCLEditor]
    public class RCG_ItemEditor : MonoBehaviour
    {
        static protected Dictionary<string, object> m_CardEditTmpDatas = new Dictionary<string, object>();
        public static RCG_ItemEditor ins = null;
        public class ItemEditData
        {
            public ItemEditData(RCG_ItemData _CardData, string _FilePath)
            {
                m_CardData = _CardData;
                m_FilePath = _FilePath;
            }
            public string m_FilePath = "";
            public RCG_ItemData m_CardData = null;
            public Texture2D m_IconTexture = null;
        }
        const string FolderPathKey = "RCG_ItemEditor_FolderPath";
        public string m_FolderPath = "";
        public string m_CardSetting = "";
        public string IconPath
        {
            get
            {
                return Path.Combine(UCL.Core.FileLib.Lib.RemoveFolderPath(m_FolderPath, 1), "Icons");
            }
        }
        //public string DeckPath
        //{
        //    get
        //    {
        //        return Path.Combine(UCL.Core.FileLib.Lib.RemoveFolderPath(m_FolderPath, 1), "Deck.json");
        //    }
        //}
        protected Vector2 m_ScrollPos_SelectCard = default;
        protected Vector2 m_ScrollPos_EditCard = default;
        protected Rect m_WindowRect = default;
        protected List<string> m_CardDataPaths = null;
        protected List<string> m_CardIconPaths = new List<string>();
        protected ItemEditData m_EditingData = null;
        protected DeckData m_EditingDeckData = null;
        protected int m_CreateEffectID = 0;
        protected bool m_CreateEffectOpened = false;
        protected bool m_Inited = false;
        private void Awake()
        {
            Init();
        }
        private void OnDestroy()
        {
            PlayerPrefs.SetString(FolderPathKey, m_FolderPath);
        }
        virtual public void Init()
        {
            if (m_Inited) return;
            m_Inited = true;
            ins = this;
            if (PlayerPrefs.HasKey(FolderPathKey))
            {
                m_FolderPath = PlayerPrefs.GetString(FolderPathKey);
            }
            if (string.IsNullOrEmpty(m_FolderPath) || !Directory.Exists(m_FolderPath)) ResetFolderPath();

            //if(!string.IsNullOrEmpty(m_FolderPath) && !Directory.Exists(m_FolderPath)) {
            //    Directory.CreateDirectory(m_FolderPath);
            //}
            RefreshCardDataPaths();
        }
        virtual public void RefreshCardDataPaths()
        {
            if (m_CardDataPaths == null)
            {
                m_CardDataPaths = new List<string>();
            }
            else
            {
                m_CardDataPaths.Clear();
            }
            if (string.IsNullOrEmpty(m_FolderPath)) return;
            var files = UCL.Core.FileLib.Lib.GetFiles(m_FolderPath, "*.json");

            if (files != null)
            {
                int discard_len = m_FolderPath.Length + 1;
                for (int i = 0; i < files.Length; i++)
                {
                    var card_path = files[i];
                    m_CardDataPaths.Add(card_path.Substring(discard_len, card_path.Length - discard_len));
                }
                //m_CardDataNames = files.ToList();
            }
        }
        #region SelectItem
        virtual protected void SelectItemWindow()
        {
            m_ScrollPos_SelectCard = GUILayout.BeginScrollView(m_ScrollPos_SelectCard);
            GUILayout.BeginVertical();
            using (var scope = new GUILayout.VerticalScope("box"))
            {
                GUILayout.BeginHorizontal();

                if (UCL.Core.UI.UCL_GUILayout.ButtonAutoSize("Explore folder", 22))
                {
#if UNITY_EDITOR
                    m_FolderPath = UCL.Core.FileLib.EditorLib.OpenFolderExplorer(m_FolderPath);
                    RefreshCardDataPaths();
#endif
                    //bool flag = UnityEditor.EditorUtility.DisplayDialog("Test", "HiHi", "Ok", "QAQ");
                    //Debug.LogError("flag:" + flag);
                }
                if (UCL.Core.UI.UCL_GUILayout.ButtonAutoSize("Reset folder", 22))
                {
                    ResetFolderPath();
                }
                if (UCL.Core.UI.UCL_GUILayout.ButtonAutoSize("Refresh ItemDatas", 22))
                {
                    RefreshCardDataPaths();
                }
                if (UCL.Core.UI.UCL_GUILayout.ButtonAutoSize("Create ItemData", 22))
                {
                    SetEditCardData(new ItemEditData(new RCG_ItemData(), "NewItem.json"));
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                UCL.Core.UI.UCL_GUILayout.LabelAutoSize("Folder Path", 18);
                m_FolderPath = GUILayout.TextField(m_FolderPath);//, GUILayout.Height(26)
                GUILayout.EndHorizontal();
            }
            GUILayout.BeginHorizontal();
            using (var scope = new GUILayout.VerticalScope("box"))
            {
                for (int i = 0; i < m_CardDataPaths.Count; i++)
                {
                    string card_path = m_CardDataPaths[i];
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button(card_path))
                    {
                        string data = File.ReadAllText(Path.Combine(m_FolderPath, card_path));
                        var json = UCL.Core.JsonLib.JsonData.ParseJson(data);
                        SetEditCardData(new ItemEditData(new RCG_ItemData(json), card_path));
                    }
                    if (UCL.Core.UI.UCL_GUILayout.ButtonAutoSize("Delete", 18))
                    {
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
        #endregion
        virtual protected void ResetFolderPath()
        {
            m_FolderPath = RCG_ItemData.ItemDataPath;
            Debug.LogWarning("System.Environment.CurrentDirectory:" + System.Environment.CurrentDirectory);
            var aCurrentDirectory = System.Environment.CurrentDirectory.Replace('\\', '/') + "/";
            if (m_FolderPath.Contains(aCurrentDirectory))
            {
                m_FolderPath = m_FolderPath.Replace(aCurrentDirectory, string.Empty);
            }
            RefreshCardDataPaths();
        }
        static public bool IsCardEditTmpDataContainsKey(string iKey)
        {
            if (m_CardEditTmpDatas == null) return false;
            return m_CardEditTmpDatas.ContainsKey(iKey);
        }
        static public T GetEditTmpData<T>(string iKey, T iDefaultValue)
        {
            if (m_CardEditTmpDatas == null) return iDefaultValue;
            if (!m_CardEditTmpDatas.ContainsKey(iKey)) m_CardEditTmpDatas.Add(iKey, iDefaultValue);
            return (T)m_CardEditTmpDatas[iKey];
        }
        static public void SetCardEditTmpData<T>(string iKey, T iValue)
        {
            if (m_CardEditTmpDatas == null) return;
            m_CardEditTmpDatas[iKey] = iValue;
        }
        #region EditCard
        virtual protected void EditItemWindow()
        {
            if (m_EditingData == null) return;
            var card_data = m_EditingData.m_CardData;
            m_ScrollPos_EditCard = GUILayout.BeginScrollView(m_ScrollPos_EditCard);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            UCL.Core.UI.UCL_GUILayout.LabelAutoSize("Edit Item", 22);

            if (UCL.Core.UI.UCL_GUILayout.ButtonAutoSize("Back", 22))
            {
                m_EditingData = null;
                return;
            }
            m_EditingData.m_FilePath = UCL.Core.UI.UCL_GUILayout.TextField("SaveName", m_EditingData.m_FilePath);
            if (UCL.Core.UI.UCL_GUILayout.ButtonAutoSize("Save", 22))
            {
                var path = Path.Combine(m_FolderPath, m_EditingData.m_FilePath);
                var data = card_data.ToJson();
                string save_data = data.ToJsonBeautify();
                //Debug.LogWarning("path:" + path);
                //Debug.LogWarning("save_data:" + save_data);
                File.WriteAllText(path, save_data);
                RefreshCardDataPaths();
            }
#if UNITY_STANDALONE_WIN
            if (UCL.Core.UI.UCL_GUILayout.ButtonAutoSize("OpenFile", 22))
            {
                var path = Path.Combine(m_FolderPath, m_EditingData.m_FilePath);
                UCL.Core.FileLib.WindowsLib.OpenExplorer(path);
            }
#endif

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            using (var scope = new GUILayout.VerticalScope("box", GUILayout.Width(300)))
            {
                card_data.OnGUICardDatas();

                {//Draw Icon
                    bool flag = GetEditTmpData("CardIconPath", false);
                    int index = m_CardIconPaths.FindIndex(a => a == card_data.IconName);
                    GUILayout.BeginHorizontal();
                    UCL.Core.UI.UCL_GUILayout.LabelAutoSize("IconName");
                    int new_index = UCL.Core.UI.UCL_GUILayout.Popup(index, m_CardIconPaths, ref flag);
                    GUILayout.EndHorizontal();
                    SetCardEditTmpData("CardIconPath", flag);
                    if (new_index != index)
                    {
                        card_data.IconName = m_CardIconPaths[new_index];
                        UpdateCardIcon();
                    }
                }
                if (m_EditingData.m_IconTexture != null) GUILayout.Box(m_EditingData.m_IconTexture,
                     GUILayout.Width(64), GUILayout.Height(64));

            }
            using (var scope = new GUILayout.VerticalScope("box"))
            {
                GUILayout.BeginHorizontal();
                m_CreateEffectID = UCL.Core.UI.UCL_GUILayout.
                    Popup(m_CreateEffectID, RCG_CardEffectCreator.EffectNameList, ref m_CreateEffectOpened);
                if (GUILayout.Button("Add", GUILayout.Width(60)))
                {
                    card_data.AddItemEffect(RCG_CardEffectCreator.Create(m_CreateEffectID));
                }
                GUILayout.EndHorizontal();
                card_data.OnGUICardEffects();

            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }
        #endregion
        virtual protected void SetEditCardData(ItemEditData data)
        {
            if (m_EditingData != null)
            {
                if (m_EditingData.m_IconTexture != null)
                {
                    DestroyImmediate(m_EditingData.m_IconTexture);
                }
            }

            m_EditingData = data;
            m_CardEditTmpDatas.Clear();
            m_CardIconPaths.Clear();
            var icon_path = IconPath;
            if (string.IsNullOrEmpty(icon_path)) return;
            var files = UCL.Core.FileLib.Lib.GetFiles(icon_path, "*.png");

            if (files != null)
            {
                int discard_len = icon_path.Length + 1;
                for (int i = 0; i < files.Length; i++)
                {
                    var card_path = files[i];
                    m_CardIconPaths.Add(card_path.Substring(discard_len, card_path.Length - discard_len));
                }
            }

            UpdateCardIcon();
        }
        virtual protected void UpdateCardIcon()
        {
            if (m_EditingData == null) return;
            var card_data = m_EditingData.m_CardData;
            if (string.IsNullOrEmpty(card_data.IconName)) return;
            string icon_path = Path.Combine(IconPath, card_data.IconName);
            //Debug.LogWarning("icon_path:" + icon_path);
            if (File.Exists(icon_path))
            {
                var fileData = File.ReadAllBytes(icon_path);
                var tex = new Texture2D(2, 2);
                tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
                if (m_EditingData.m_IconTexture != null)
                {
                    DestroyImmediate(m_EditingData.m_IconTexture);
                }
                m_EditingData.m_IconTexture = tex;
            }
            else
            {
                Debug.LogError("icon_path:" + icon_path + ",not exist!!");
            }
        }
#if UNITY_EDITOR
        [UCL.Core.ATTR.UCL_FunctionButton]
        public void ShowEditWindow()
        {
            RCG_ItemEditorWindow.ShowWindow(this);
        }
#endif

        virtual public void EditWindow(int id)
        {
            if (m_EditingData == null)
            {
                SelectItemWindow();
            }
            else
            {
                EditItemWindow();
            }
        }
        private void OnGUI()
        {
            const int edge = 5;//5 pixel
            m_WindowRect = new Rect(edge, edge, Screen.width - 2 * edge, Screen.height - 2 * edge);
            m_WindowRect = GUILayout.Window(133125, m_WindowRect, EditWindow, "ItemEditor");
        }
    }
}
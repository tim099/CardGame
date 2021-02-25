using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Test_AtlasChange : EditorWindow
{
    private static Test_AtlasChange mWindows = null;
    private Sprite mOldSprite;
    private Sprite mNewSprite;

    /// <summary>
    /// 緩存起來的Sprites
    /// </summary>
    private Dictionary<string, List<Sprite>> mTempSprites = new Dictionary<string, List<Sprite>>();
    private string mChangeAtlasCSVPath = string.Empty;
    private Dictionary<string, ChangeSpriteMember> mChangeSpriteLoader = new Dictionary<string, ChangeSpriteMember>();
    private Dictionary<string, ChangeSpriteMember> mChangeSpriteLoader_NewToOld = new Dictionary<string, ChangeSpriteMember>();
    private string mFormatStr = "{0}_{1}";
    private Vector2 mSelectSpriteThumb = Vector2.zero;
    private Vector2 mSelectObjThumb = Vector2.zero;
    /// <summary>
    /// 交換使用
    /// </summary>
    public class ChangeSpriteMember
    {
        /// <summary>
        /// 組合 路徑+ICON
        /// </summary>
        public string OldSpriteKey = "";
        public string NewSpriteKey = "";

        public string OldAllSpritePath = "";
        public string OldSpriteName = "";
        public string NewAllSpritePath = "";
        public string NewSpriteName = "";
    }

    [MenuItem("Tool/DUAN/TestAtlas")]
    public static void ShowWindow()
    {
        mWindows = EditorWindow.GetWindow(typeof(Test_AtlasChange)) as Test_AtlasChange;

        const int width = 400;
        const int height = 320;

        var x = (Screen.currentResolution.width - width) / 4;
        var y = (Screen.currentResolution.height - height) / 2;

        mWindows.position = new Rect(x, y, width, height);
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.TextField(mChangeAtlasCSVPath);
        if (GUILayout.Button("Open", GUILayout.Width(80f)))
        {
            string _path = EditorUtility.OpenFilePanel("Overwrite with png", "", "csv");
            mChangeAtlasCSVPath = _path;
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginVertical();

        ShowInitBtn();
        ShowAtlasObj();
        ShowChangeToNewBtn();
        ShowBackToOld();
        ShowSelectObj();
        GUILayout.EndVertical();
    }

    private void ShowInitBtn()
    {
        if (GUILayout.Button("Init"))
        {
            mChangeSpriteLoader = new Dictionary<string, ChangeSpriteMember>();
            mChangeSpriteLoader_NewToOld = new Dictionary<string, ChangeSpriteMember>();
            mTempSprites = new Dictionary<string, List<Sprite>>();

            if (System.IO.File.Exists(mChangeAtlasCSVPath) == false)
            {
                Debug.LogError("CSV 檔案不存在請檢察 ~!! \n路徑為 : " + mChangeAtlasCSVPath);
                return;
            }

            string _allText = System.IO.File.ReadAllText(mChangeAtlasCSVPath);
            string[] _lines = _allText.Split("\n"[0]);

            for (int i = 0; i < _lines.Length; i++)
            {
                if (i == 0)
                    continue;

                string[] _contents = (_lines[i].Trim()).Split(',');
                if (_contents.Length < 4)
                {
                    Debug.LogError("格式錯誤請檢察~!!");
                    continue;
                }

                string _oldAllSpritePath = _contents[0];
                string _oldSpriteName = _contents[1];
                string _newAllSpritePath = _contents[2];
                string _newSpriteName = _contents[3];

                ChangeSpriteMember _changeSprite = new ChangeSpriteMember();
                _changeSprite.OldSpriteKey = string.Format(mFormatStr, _oldAllSpritePath, _oldSpriteName);
                _changeSprite.NewSpriteKey = string.Format(mFormatStr, _newAllSpritePath, _newSpriteName);
                _changeSprite.OldAllSpritePath = _oldAllSpritePath;
                _changeSprite.OldSpriteName = _oldSpriteName;
                _changeSprite.NewAllSpritePath = _newAllSpritePath;
                _changeSprite.NewSpriteName = _newSpriteName;

                if (mChangeSpriteLoader.ContainsKey(_changeSprite.OldSpriteKey) == false)
                    mChangeSpriteLoader.Add(_changeSprite.OldSpriteKey, _changeSprite);
                else
                    Debug.LogError(_changeSprite.OldSpriteKey + " ==> 名稱重複請檢察~!!");

                if (mChangeSpriteLoader_NewToOld.ContainsKey(_changeSprite.NewSpriteKey) == false)
                    mChangeSpriteLoader_NewToOld.Add(_changeSprite.NewSpriteKey, _changeSprite);

                ///先緩存 所有可能會用到的Sprite
                if (mTempSprites.ContainsKey(_oldAllSpritePath) == false)
                {
                    mTempSprites.Add(_oldAllSpritePath, new List<Sprite>());
                    mTempSprites[_oldAllSpritePath] = GetAllSprite(_oldAllSpritePath);
                }

                ///先緩存 所有可能會用到的Sprite
                if (mTempSprites.ContainsKey(_newAllSpritePath) == false)
                {
                    mTempSprites.Add(_newAllSpritePath, new List<Sprite>());
                    mTempSprites[_newAllSpritePath] = GetAllSprite(_newAllSpritePath);
                }
            }
        }
    }

    public void ShowAtlasObj()
    {
        if (mTempSprites == null)
            return;

        if (mTempSprites.Count == 0)
            return;

        GUILayout.Space(5f);
        GUILayout.Label("Using Sprite : ");
        mSelectSpriteThumb = GUILayout.BeginScrollView(mSelectSpriteThumb, GUILayout.Height(100));
        foreach (var _spritePath in mTempSprites.Keys)
        {
            GUILayout.Label(_spritePath);
        }
        GUILayout.EndScrollView();
    }

    private void ShowChangeToNewBtn()
    {
        if (GUILayout.Button("Change To New"))
        {
            if (Selection.activeObject == null)
                return;

            List<UnityEngine.UI.Image> _Imgs = CheckSelectObjs();
            foreach (var _i in _Imgs)
            {
                if (_i.sprite == null)
                    continue;

                string _path = AssetDatabase.GetAssetPath(_i.sprite);
                string _name = _i.sprite.name;
                string _key = string.Format(mFormatStr, _path, _name);
                ///有更換資料
                if (mChangeSpriteLoader.ContainsKey(_key))
                {
                    ChangeSpriteMember _changeSprite = mChangeSpriteLoader[_key];
                    if (_changeSprite == null)
                        return;

                    if (mTempSprites.ContainsKey(_changeSprite.NewAllSpritePath))
                    {
                        Sprite _targetSpite = mTempSprites[_changeSprite.NewAllSpritePath].Find(x => x.name == _changeSprite.NewSpriteName);
                        if (_targetSpite != null)
                            _i.sprite = _targetSpite;
                        else
                            Debug.LogError("找不到這個Sprite : " + _changeSprite.NewAllSpritePath + " ==> " + _changeSprite.NewSpriteName);
                    }
                    else
                        Debug.LogError("找不到這個路徑的Sprite : " + _changeSprite.NewAllSpritePath);
                }
            }
            AssetDatabase.Refresh();
        }
    }

    private void ShowBackToOld()
    {
        if (GUILayout.Button("Back To Old"))
        {
            if (Selection.activeObject == null)
                return;

            List<UnityEngine.UI.Image> _Imgs = CheckSelectObjs();
            foreach (var _i in _Imgs)
            {
                if (_i.sprite == null)
                    continue;

                string _path = AssetDatabase.GetAssetPath(_i.sprite);
                string _name = _i.sprite.name;
                string _key = string.Format(mFormatStr, _path, _name);
                ///有更換資料
                if (mChangeSpriteLoader_NewToOld.ContainsKey(_key))
                {
                    ChangeSpriteMember _changeSprite = mChangeSpriteLoader_NewToOld[_key];
                    if (_changeSprite == null)
                        return;

                    if (mTempSprites.ContainsKey(_changeSprite.OldAllSpritePath))
                    {
                        Sprite _targetSpite = mTempSprites[_changeSprite.OldAllSpritePath].Find(x => x.name == _changeSprite.OldSpriteName);
                        if (_targetSpite != null)
                            _i.sprite = _targetSpite;
                        else
                            Debug.LogError("找不到這個Sprite : " + _changeSprite.OldAllSpritePath + " ==> " + _changeSprite.OldSpriteName);
                    }
                    else
                        Debug.LogError("找不到這個路徑的Sprite : " + _changeSprite.OldAllSpritePath);
                }
            }

            AssetDatabase.Refresh();
        }
    }

    private void ShowSelectObj()
    {
        mSelectObjThumb = GUILayout.BeginScrollView(mSelectObjThumb, GUILayout.Height(100));
        foreach (var _o in Selection.objects)
        {
            if (_o != null)
                GUILayout.Label(_o.name);
        }
        GUILayout.EndScrollView();
    }


    Dictionary<int, GameObject> mSelectionHash = new Dictionary<int, GameObject>();
    private List<UnityEngine.UI.Image> CheckSelectObjs()
    {
        List<UnityEngine.UI.Image> _targetImgs = new List<UnityEngine.UI.Image>();
        mSelectionHash = new Dictionary<int, GameObject>();

        for (int i = 0; i < Selection.objects.Length; i++)
        {
            GameObject _Obj = Selection.objects[i] as GameObject;
            if (_Obj.transform == null)
                continue;

            _targetImgs.AddRange(CheckSelectObjs(_Obj.transform));
        }

        return _targetImgs;
    }

    private List<UnityEngine.UI.Image> CheckSelectObjs(Transform iTrans)
    {
        List<UnityEngine.UI.Image> _images = new List<UnityEngine.UI.Image>();

        if (iTrans == null)
            return _images;
        
        if (mSelectionHash.ContainsKey(iTrans.GetHashCode()))
            return _images;

        mSelectionHash.Add(iTrans.GetHashCode(), null);
        UnityEngine.UI.Image _i = iTrans.GetComponent<UnityEngine.UI.Image>();
        if (_i != null)
            _images.Add(_i);

        if (iTrans.childCount > 0)
        {
            for (int i = 0; i < iTrans.childCount; i++)
            {
                _images.AddRange(CheckSelectObjs(iTrans.GetChild(i)));
            }
        }

        return _images;
    }

    /// <summary>
    /// 獲得該路徑下擁有的檔案
    /// </summary>
    /// <param name="iPath"></param>
    /// <returns></returns>
    private List<Sprite> GetAllSprite(string iPath)
    {
        List<Sprite> _allSprite = new List<Sprite>();
        Object[] _allData = AssetDatabase.LoadAllAssetsAtPath(iPath);
        foreach (Object _o in _allData)
        {
            if (_o is Sprite)
            {
                _allSprite.Add(_o as Sprite);
            }
        }

        return _allSprite;
    }
}

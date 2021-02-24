using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace RCG
{
    public class RCG_MonsterActionIndicatorIcons
    {
        public static Dictionary<string, Sprite> m_Icons = new Dictionary<string, Sprite>();
    }
    public class RCG_MonsterActionIndicator : MonoBehaviour
    {
        public Image m_ActionIcon;
        public Image m_Indicator;
        public Text m_Description;
        public Text m_DescriptionShort;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Init(RCG_MonsterAction iAction)
        {
            var aIconPath = Path.Combine(RCG_MonsterAction.MonsterActionIconPath, iAction.GetIcon());
            Debug.Log(iAction.ActionType);
            if (RCG_MonsterActionIndicatorIcons.m_Icons.ContainsKey(iAction.GetIcon()))
            {
                m_ActionIcon.sprite = RCG_MonsterActionIndicatorIcons.m_Icons[iAction.GetIcon()];
            }
            else if (File.Exists(aIconPath))
            {
                var aData = File.ReadAllBytes(aIconPath);
                Texture2D aTexture = new Texture2D(1, 1);
                aTexture.LoadImage(aData);
                var aSprite = Sprite.Create(aTexture, new Rect(0.0f, 0.0f, aTexture.width, aTexture.height),
                    new Vector2(0.5f, 0.5f), 100.0f);
                // QWQ m_CardSprites.Add(iIconName, aSprite);
                RCG_MonsterActionIndicatorIcons.m_Icons.Add(iAction.GetIcon(), aSprite);
                m_ActionIcon.sprite = aSprite;
            }

            m_Description.text = iAction.GetDescription();
            m_DescriptionShort.text = iAction.GetDescriptionShort();
        }
    }
}

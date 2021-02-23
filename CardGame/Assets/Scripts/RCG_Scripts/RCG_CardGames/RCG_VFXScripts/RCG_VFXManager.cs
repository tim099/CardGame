using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG
{
    public class RCG_VFXManager : MonoBehaviour
    {
        public static RCG_VFXManager ins = null;
        [SerializeField] protected Transform m_VFXRoot = null;
        [SerializeField] protected Transform m_VFXTemplatesRoot = null;

        protected List<RCG_VFX> m_VFXTemplates = null;
        protected Dictionary<string, RCG_VFX> m_VFXTemplatesDic = null;
        protected Dictionary<string, Queue<RCG_VFX> > m_VFXDic = null;
        virtual public void Init()
        {
            ins = this;
            m_VFXTemplates = new List<RCG_VFX>();
            UCL.Core.GameObjectLib.SearchChild(m_VFXTemplatesRoot, m_VFXTemplates);
            m_VFXTemplatesDic = new Dictionary<string, RCG_VFX>();
            m_VFXDic = new Dictionary<string, Queue<RCG_VFX>>();
            foreach(var aVFX in m_VFXTemplates)
            {
                aVFX.gameObject.SetActive(false);
                m_VFXTemplatesDic.Add(aVFX.name, aVFX);
                
                //aVFX.Init();
                m_VFXDic.Add(aVFX.name, new Queue<RCG_VFX>());
            }
        }
        /// <summary>
        /// 照類別生成VFX
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T CreateVFX<T>() where T : RCG_VFX
        {
            return CreateVFX(typeof(T).Name.Replace("RCG_",string.Empty)) as T;
        }

        public T CreateVFX<T>(string iName) where T : RCG_VFX
        {
            return CreateVFX(iName) as T;
        }
        /// <summary>
        /// 照名稱生成VFX
        /// </summary>
        /// <param name="iName"></param>
        /// <returns></returns>
        public RCG_VFX CreateVFX(string iName)
        {
            if (!m_VFXDic.ContainsKey(iName))
            {
                Debug.LogError("CreateVFX Fail!!iName:" + iName + ",Not Exist!!");
                return null;
            }
            var aVFXs = m_VFXDic[iName];
            RCG_VFX aVFX = null;
            if (aVFXs.Count > 0)
            {
                aVFX = aVFXs.Dequeue();
            }
            if(aVFX == null)
            {
                var aTemp = m_VFXTemplatesDic[iName];
                aVFX = Instantiate(aTemp, m_VFXRoot);
                aVFX.name = aTemp.name;
                aVFX.Init();
            }
            aVFX.ShowVFX();
            return aVFX;
        }
        public void DeleteVFX(RCG_VFX iVFX)
        {
            string aName = iVFX.name;
            if (!m_VFXDic.ContainsKey(aName))
            {
                Debug.LogError("DeleteVFX Fail!!iName:" + aName + ",Not Exist!!");
                return;
            }
            iVFX.HideVFX();
            m_VFXDic[aName].Enqueue(iVFX);
        }
    }
}
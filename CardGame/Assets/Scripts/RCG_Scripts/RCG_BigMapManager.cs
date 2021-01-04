using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG
{
    public class RCG_BigMapManager : MonoBehaviour
    {
        public RCG_EmbarkUI m_EmbarkUI;
        private void Awake() {
            Init();
        }
        virtual public void Init() {
            m_EmbarkUI.Init();
        }
    }
}
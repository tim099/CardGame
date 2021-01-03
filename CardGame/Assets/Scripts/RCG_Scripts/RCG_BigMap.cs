using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG
{
    public class RCG_BigMap : MonoBehaviour
    {
        protected List<RCG_CityButton> m_CityButs = new List<RCG_CityButton>();
        private void Awake() {
            Init();
        }
        virtual public void Init() {
            UCL.Core.GameObjectLib.SearchChild<RCG_CityButton>(transform, m_CityButs);
            foreach(var city_but in m_CityButs) {
                city_but.Init(this);
            }
        }
        virtual public void SelectCity(string city_name) {
            Debug.LogWarning("SelectCity:" + city_name);
        }
    }
}
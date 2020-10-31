using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RCG
{
    [CreateAssetMenu(fileName = "New CardBeginSet", menuName = "RCG/CardBeginSet")]
    public class RCG_CardBeginSet : ScriptableObject
    {
        public List<RCG_CardSettings> m_Settings;
    }
}
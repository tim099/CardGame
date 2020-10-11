using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG {
    [UCL.Core.ATTR.EnableUCLEditor]
    public class RCG_BattleField : MonoBehaviour
    {
        public List<RCG_Unit> m_units;

        // Start is called before the first frame update
        void Start(){}

        // Update is called once per frame
        void Update(){}

        private void Awake() {
            Test();
        }

        public void Test() {
            
        }

        public void CreateUnits() {

        }
    }
}

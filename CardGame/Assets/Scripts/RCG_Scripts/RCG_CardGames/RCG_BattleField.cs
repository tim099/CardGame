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
            RCG_Unit[] units = GetComponentsInChildren<RCG_Unit>();
            foreach(RCG_Unit u in units){
                m_units.Add(u);
            }
        }

        public void Test() {
            
        }

        public void CreateUnits() {

        }

        public void TurnEnd() {
            foreach(RCG_Unit u in m_units){
                u.EndTurn();
            }
        }
    }
}

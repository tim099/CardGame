using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCL.Core.UI;

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
            //Init();
        }

        virtual public void Init() {
            // RCG_Unit[] units = GetComponentsInChildren<RCG_Unit>();
            UCL_RectTransformCollider[] colliders = GetComponentsInChildren<UCL_RectTransformCollider>();
            // Debug.Log(units.Length);
            for(int i=0; i<colliders.Length; i++){
                UCL_RectTransformCollider c = colliders[i];
                RCG_Unit u = c.gameObject.GetComponent<RCG_Unit>();
                if(u){
                    m_units.Add(u);
                }
                else{
                    m_units.Add(null);
                }
            }
        }

        public void Test() {
            
        }

        public void CreateUnits() {

        }

        public void TurnEnd() {
            foreach(RCG_Unit u in m_units){
                if(u == null){
                    continue;
                }
                u.EndTurn();
            }
        }

        public void TriggerCardEffect(int target, RCG_CardData card_data){
            Debug.Log("TriggerCardEffect");
            RCG_CardEffectHandler.TriggerCardEffectOnUnits(m_units ,target, card_data);
        }
    }
}

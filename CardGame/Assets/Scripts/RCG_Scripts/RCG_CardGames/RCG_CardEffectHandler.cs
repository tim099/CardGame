using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG {
    [UCL.Core.ATTR.EnableUCLEditor]
    public class RCG_CardEffectHandler
    {
        public static void TriggerCardEffectOnUnits(List<RCG_Unit> units, RCG_CardData card_data){
            if(card_data.Atk > 0){
                if(card_data.AtkTimes > 1){
                    for(int i=0; i<units.Count; i++){
                        units[i].DamageHP(card_data.Atk * card_data.AtkTimes);
                    }
                }
                else{
                    for(int i=0; i<units.Count; i++){
                        units[i].DamageHP(card_data.Atk);
                    }
                }
            }
            if(card_data.Defense > 0){

            }

        }

        public static void TriggerCardEffectOnUnits(List<RCG_Unit> all_units, int target, RCG_CardData card_data){
            List<RCG_Unit> units = FilterTargetUnits(all_units, target);
            TriggerCardEffectOnUnits(units, card_data);
        }

        public static List<RCG_Unit> FilterTargetUnits(List<RCG_Unit> all_units, int target){
            List<RCG_Unit> units = new List<RCG_Unit>();

            units.Add(all_units[target]);

            return units;
        }
    }
}

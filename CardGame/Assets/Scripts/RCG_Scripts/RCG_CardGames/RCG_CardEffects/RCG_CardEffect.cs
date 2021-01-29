using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RCG {
    public class RCG_CardEffect : UCL.Core.JsonLib.IJsonSerializable {
        public int ID { get; private set; } = 0;
        virtual public void Init(int iID) {
            ID = iID;
        }
        virtual public string EffectType { get { return this.GetType().Name.Replace("RCG_Card", ""); } }
        virtual public string Description { get { return string.Empty; } }

        virtual public void DeserializeFromJson(UCL.Core.JsonLib.JsonData data) {
            UCL.Core.JsonLib.JsonConvert.LoadDataFromJson(this, data);
        }
        
        virtual public UCL.Core.JsonLib.JsonData SerializeToJson() {
            UCL.Core.JsonLib.JsonData data = new UCL.Core.JsonLib.JsonData();
            data["EffectType"] = EffectType;
            UCL.Core.JsonLib.JsonConvert.SaveDataToJson(this, data);
            return data;
        }
        virtual public void TriggerEffect(TriggerEffectData iTriggerEffectData) {

        }
        virtual public void DrawFieldData(object obj) {
            using(var scope = new GUILayout.VerticalScope("box")) {
                Type type = obj.GetType();
                var fields = type.GetAllFieldsUntil(typeof(RCG_CardEffect), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                foreach(var field in fields) {
                    var data = field.GetValue(obj);
                    string dp_name = field.Name;

                    if(dp_name[0] == 'm' && dp_name[1] == '_') {
                        dp_name = dp_name.Substring(2, dp_name.Length - 2);
                    }
                    if(data == null) {

                    } else if(data.IsNumber()) {
                        var result = UCL.Core.UI.UCL_GUILayout.NumField(dp_name, data);
                        field.SetValue(obj, result);
                    } else if(field.FieldType == typeof(string)) {
                        var result = UCL.Core.UI.UCL_GUILayout.TextField(dp_name, (string)data);
                        field.SetValue(obj, result);
                    } else if(field.FieldType.IsEnum) {
                        GUILayout.BeginHorizontal();
                        UCL.Core.UI.UCL_GUILayout.LabelAutoSize(dp_name);
                        string aKey = ID.ToString() + dp_name;
                        bool flag = RCG_CardEditor.GetCardEditTmpData(aKey, false);
                        field.SetValue(obj, UCL.Core.UI.UCL_GUILayout.Popup((System.Enum)data, ref flag));
                        RCG_CardEditor.SetCardEditTmpData(aKey, flag);
                        GUILayout.EndHorizontal();
                    } else if(field.FieldType.IsStructOrClass()) {
                        UCL.Core.UI.UCL_GUILayout.LabelAutoSize(dp_name);
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        DrawFieldData(data);
                        field.SetValue(obj, data);
                        GUILayout.EndHorizontal();
                    }

                }
            }
        }
        virtual public void OnGUI() {
            using(var scope = new GUILayout.VerticalScope("box")) {
                DrawFieldData(this);
            }  
        }

    }
}


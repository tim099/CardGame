using System;
using System.Reflection;
using UnityEngine;

namespace RCG
{
    public class RCG_CardEffect : UCL.Core.JsonLib.IJsonSerializable {
        virtual public void Init(int iID) {

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
        virtual public void TriggerEffect(TriggerEffectData iTriggerEffectData, Action iEndAction)
        {
            iEndAction.Invoke();
        }
        virtual public void DrawFieldData(object obj, int iID) {
            using(var scope = new GUILayout.VerticalScope("box")) {
                Type type = obj.GetType();
                var fields = type.GetAllFieldsUntil(typeof(RCG_CardEffect), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                foreach(var field in fields) {
                    var aData = field.GetValue(obj);
                    string dp_name = field.Name;

                    if(dp_name[0] == 'm' && dp_name[1] == '_') {
                        dp_name = dp_name.Substring(2, dp_name.Length - 2);
                    }
                    if(aData == null) {

                    } else if(aData.IsNumber()) {
                        string aKey = iID.ToString() + "_" + dp_name;
                        if (!RCG_CardEditor.IsCardEditTmpDataContainsKey(aKey))
                        {
                            RCG_CardEditor.SetCardEditTmpData(aKey, aData.ToString());
                        }
                        string aNum = RCG_CardEditor.GetCardEditTmpData(aKey, string.Empty);
                        var aResult = UCL.Core.UI.UCL_GUILayout.TextField(dp_name, (string)aNum);
                        RCG_CardEditor.SetCardEditTmpData(aKey, aResult);
                        object res_val;
                        if (UCL.Core.MathLib.Num.TryParse(aResult, aData.GetType(), out res_val)) {
                            field.SetValue(obj, res_val);
                        }
                        //var result = UCL.Core.UI.UCL_GUILayout.NumField(dp_name, data);
                        //field.SetValue(obj, result);
                    }
                    else if (aData is bool)
                    {
                        field.SetValue(obj, UCL.Core.UI.UCL_GUILayout.BoolField(dp_name, (bool)aData));
                    }
                    else if(field.FieldType == typeof(string)) {
                        var result = UCL.Core.UI.UCL_GUILayout.TextField(dp_name, (string)aData);
                        field.SetValue(obj, result);
                    } else if(field.FieldType.IsEnum) {
                        GUILayout.BeginHorizontal();
                        UCL.Core.UI.UCL_GUILayout.LabelAutoSize(dp_name);
                        string aKey = iID.ToString() + dp_name;
                        bool flag = RCG_CardEditor.GetCardEditTmpData(aKey, false);
                        field.SetValue(obj, UCL.Core.UI.UCL_GUILayout.Popup((System.Enum)aData, ref flag));
                        RCG_CardEditor.SetCardEditTmpData(aKey, flag);
                        GUILayout.EndHorizontal();
                    } else if(field.FieldType.IsStructOrClass()) {
                        UCL.Core.UI.UCL_GUILayout.LabelAutoSize(dp_name);
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        DrawFieldData(aData, iID);
                        field.SetValue(obj, aData);
                        GUILayout.EndHorizontal();
                    }

                }
            }
        }
        virtual public void OnGUI(int iID) {
            using(var scope = new GUILayout.VerticalScope("box")) {
                DrawFieldData(this, iID);
            }  
        }

    }
}


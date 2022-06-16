using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;

public class GameData : ScriptableObject
{
#if UNITY_EDITOR
    public virtual void Parse(System.Object[] objList)
    {

    }

    protected void ParseObject(object obj, object csvObj)
    {
        Dictionary<string, System.Object> dict = csvObj as Dictionary<string, System.Object>;

        //해당 타입이 가진 모든 필드 정보를 가져옴
        FieldInfo[] infos = obj.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

        string fieldName = "";

        for (int i = 0; i < infos.Length; i++)
        {
            FieldInfo fi = infos[i];
            fieldName = fi.Name;
            if (dict.ContainsKey(fieldName))
            {
                object valueObj = dict[fieldName];

                if (fi.FieldType == typeof(string))
                {
                    fi.SetValue(obj, valueObj.ToString());
                }
                else if (fi.FieldType == typeof(int))
                {
                    fi.SetValue(obj, Convert.ToInt32(valueObj));
                }
                else if (fi.FieldType == typeof(float))
                {
                    fi.SetValue(obj, (float)Convert.ToDouble(valueObj));
                }
                else
                {
                    Debug.LogError("Unspported Type!!");
                }
            }
        }
    }
#endif
}

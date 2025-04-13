using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

public class Utils
{
    public static ElementType GetElementType(string element_type_name)
    {
        return element_type_name switch
        {
            "炎" => ElementType.Pyro,
            "水" => ElementType.Hydro,
            "氷" => ElementType.Cryo,
            "雷" => ElementType.Electro,
            "岩" => ElementType.Geo,
            "風" => ElementType.Anemo,
            "草" => ElementType.Dendro,
            "物理" => ElementType.Physics,
            _ => ElementType.None,
        };
    }

    public static ReferenceStatus GetReferenceStatus(string referenceStatusName)
    {
        return referenceStatusName switch
        {
            "攻撃%" => ReferenceStatus.Atk,
            "防御%" => ReferenceStatus.Def,
            "HP%" => ReferenceStatus.Hp,
            "元素熟知" => ReferenceStatus.Em,
            _ => ReferenceStatus.None,
        };
    }

    public static WeaponType GetWeaponType(string weapon_type_name)
    {
        return weapon_type_name switch
        {
            "片手剣" => WeaponType.Sword,
            "両手剣" => WeaponType.Claymore,
            "弓" => WeaponType.Bow,
            "法器" => WeaponType.Catalyst,
            "槍" => WeaponType.Polearm,
            _ => WeaponType.None,
        };
    }

    public static void LogJson(object value)
    {
        Debug.Log(JsonConvert.SerializeObject(value, Formatting.Indented));
    }

    public static T AddInstances<T>(T[] instances) where T : new()
    {
        Type type = typeof(T);
        T newInstance = new(); // デフォルトの値をセットする

        foreach (FieldInfo fieldInfo in type.GetFields())
        {
            if (fieldInfo.FieldType == typeof(int))
            {
                int sum = 0;
                foreach (T instance in instances)
                {
                    sum += (int)fieldInfo.GetValue(instance);
                }
                fieldInfo.SetValue(newInstance, sum);
            }
            if (fieldInfo.FieldType == typeof(float))
            {
                float sum = 0;
                foreach (T instance in instances)
                {
                    sum += (float)fieldInfo.GetValue(instance);
                }
                fieldInfo.SetValue(newInstance, sum);
            }
            if (fieldInfo.FieldType == typeof(bool))
            {
                bool flag = false;
                foreach (T instance in instances)
                {
                    flag = (bool)fieldInfo.GetValue(instance);
                    if (flag) break;
                }
                fieldInfo.SetValue(newInstance, flag);
            }
            if (fieldInfo.FieldType == typeof(string))
            {
                string name = "";
                foreach (T instance in instances)
                {
                    name += (string)fieldInfo.GetValue(instance) + "+";
                }
                fieldInfo.SetValue(newInstance, name.TrimEnd('+'));
            }
        }
        return newInstance;
    }


    public static void CopyBaseFields<T>(T source, T destination)
    {
        var type = typeof(T);
        var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);

        foreach (var field in fields)
        {
            var value = field.GetValue(source);
            field.SetValue(destination, value);
        }
    }

}

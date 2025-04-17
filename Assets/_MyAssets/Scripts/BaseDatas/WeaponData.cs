using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponData : BaseData
{
    public string type_name;
    public int refinement = 1;
    public string option = "";
    public float base_atk;
    public float homa;
    public float sekisha;
    public float sekikaku;
    public float suikou;
    public float seiken_hp;
    public float seiken_em;
    public float kusanagi;
    public float kariudo;
    public float cinnabar;

    public WeaponType WeaponType => Utils.GetWeaponType(type_name);
    public string Id => $"{name}+{type_name}+R{refinement}" + ((option == "") ? "" : $"+{option}");

    public string DisplayName
    {
        get
        {
            List<string> infos = new();
            if (refinement > 1) infos.Add($"R{refinement}");
            if (string.IsNullOrEmpty(option) == false) infos.Add(option);
            string combinedInfo = string.Join("、", infos);
            if (string.IsNullOrEmpty(combinedInfo)) return name;
            return $"{name}({combinedInfo})";
        }
    }
}
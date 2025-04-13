using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponData : BaseData
{
    public string type_name;
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
}
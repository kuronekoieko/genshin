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

    public WeaponType WeaponType
    {
        get
        {
            return type_name switch
            {
                "片手剣" => WeaponType.Sword,
                "両手剣" => WeaponType.Claymore,
                "弓" => WeaponType.Bow,
                "法器" => WeaponType.Catalyst,
                "槍" => WeaponType.Polearm,
                _ => WeaponType.None,
            };
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyGame/Create " + nameof(CharacterSO), fileName = nameof(CharacterSO))]
public class CharacterSO : ScriptableObject
{
    public Status status;
    public Ascend ascend;
}

[Serializable]
public class Status
{
    public string name = "八重神子";
    public WeaponType weaponType = WeaponType.Catalyst;
    public ElementType elementType = ElementType.Electro;
    // 基礎ステータス Lv90
    public float baseAtk = 340;
    public float baseDef = 569;
    public float baseHp = 10372;
    public readonly float baseCritRate = 0.05f;
    public readonly float baseCritDmg = 0.5f;
    public string WeaponTypeName
    {
        get
        {
            return weaponType switch
            {
                WeaponType.Sword => "片手剣",
                WeaponType.Claymore => "両手剣",
                WeaponType.Bow => "弓",
                WeaponType.Catalyst => "法器",
                WeaponType.Polearm => "槍",
                _ => "",
            };
        }
    }
}


[Serializable]
public class Ascend
{
    public float critRate = 0.192f;
    public float critDmg = 0;
    public float dmgBonus = 0;
    public float atkPer = 0;
    public float energyRecharge = 0;
    public float hpPer = 0;
    public float defPer = 0;
}

public enum WeaponType
{
    Sword,
    Claymore,
    Bow,
    Catalyst,
    Polearm,
}

public enum ElementType
{
    Pyro,
    Hydro,
    Electro,
    Cryo,
    Geo,
    Anemo,
    Dendro,
    Physics,
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour
{
    public Status status;
    public Ascend ascend;
    public string Name => gameObject.name;
    public string WeaponType => status.WeaponTypeName;

    public abstract Dictionary<string, string> CalcDmg(Datas datas);

    protected float ElementalDmgBonus(Datas datas)
    {
        float pyro_bonus = datas.pyro_bonus();
        float hydro_bonus = datas.hydro_bonus();
        float electro_bonus = datas.electro_bonus();
        float cryo_bonus = datas.cryo_bonus();
        float geo_bonus = datas.geo_bonus();
        float anemo_bonus = datas.anemo_bonus();
        float dendro_bonus = datas.dendro_bonus();
        float physics_bonus = datas.physics_bonus();

        return status.elementType switch
        {
            ElementType.Pyro => pyro_bonus,
            ElementType.Hydro => hydro_bonus,
            ElementType.Electro => electro_bonus,
            ElementType.Cryo => cryo_bonus,
            ElementType.Geo => geo_bonus,
            ElementType.Anemo => anemo_bonus,
            ElementType.Dendro => dendro_bonus,
            ElementType.Physics => physics_bonus,
            _ => 0,
        };
    }

    protected float GetExpectedDamage(float atk, float talentRate, float dmgAdd, float dmgBonus, float expectedCritDmg, float res, float elementalReaction)
    {
        float dmg = 0;

        dmg += (atk * talentRate + dmgAdd) * (1 + dmgBonus) * expectedCritDmg * res * elementalReaction;

        return dmg;
    }

    protected float GetExpectedDamageSum(float atk, float[] talentRates, float dmgAdd, float dmgBonus, float expectedCritDmg, float res, float elementalReaction)
    {
        float dmg = 0;

        for (int i = 0; i < talentRates.Length; i++)
        {
            dmg += (atk * talentRates[i] + dmgAdd) * (1 + dmgBonus) * expectedCritDmg * res * elementalReaction;
        }

        return dmg;
    }

    protected float GetElementalRes(float decreasingRes)
    {
        float enemyElementalRes = 0.1f + decreasingRes;
        float elementalRes = 1 / (4 * enemyElementalRes + 1);
        if (enemyElementalRes < 0.75f)
            elementalRes = 1 - enemyElementalRes;
        if (enemyElementalRes < 0)
            elementalRes = 1 - enemyElementalRes / 2;
        return elementalRes;
    }

}

[Serializable]
public class Status
{
    public WeaponType weaponType = WeaponType.Catalyst;
    public ElementType elementType = ElementType.Electro;
    // 基礎ステータス Lv90
    public float baseAtk = 340;
    public float baseDef = 569;
    public float baseHp = 10372;
    public bool hasSelfHarm = false;
    public readonly float defaultCritRate = 0.05f;
    public readonly float defaultCritDmg = 0.5f;
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
    public float heal_bonus = 0;
    public float elemental_mastery = 0;

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
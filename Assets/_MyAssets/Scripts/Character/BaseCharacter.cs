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

    public abstract Dictionary<string, string> CalcDmg(Data data);


    protected CharaData GetCharaData(Data data)
    {
        CharaData charaData = new(data);

        charaData.hp = status.baseHp * (1 + charaData.hp_rate) + data.hp();
        charaData.def = status.baseDef * (1 + charaData.def_rate) + data.def();

        var dmgAdd_sekikaku = charaData.def * data.weapon.sekikaku;
        charaData.add_normal_atk += dmgAdd_sekikaku;
        charaData.add_charged_atk += dmgAdd_sekikaku;

        var dmgAdd_cinnabar = charaData.def * data.weapon.cinnabar;
        charaData.add_skill += dmgAdd_cinnabar;

        var homa_atkAdd = charaData.hp * data.weapon.homa;
        var sekisa_atkAdd = charaData.elemental_mastery * data.weapon.sekisha;
        var kusanagi_atkAdd = (charaData.energy_recharge - 1) * data.weapon.kusanagi;

        charaData.atk
            = data.base_atk() * (1 + charaData.atk_rate)
            + data.atk()
            + homa_atkAdd
            + sekisa_atkAdd
            + kusanagi_atkAdd;

        return charaData;
    }

    protected (float, Crit) ExpectedDmg(AttackType attackType, ElementType elementType, CharaData charaData, Data data, float[] talentRates, float elementalReaction = 1)
    {
        ExpectedDamage expectedDamage = new(attackType, charaData, data.artSub);
        charaData.dmg_bonus += data.ElementalDmgBonus(elementType);

        float result = expectedDamage.GetExpectedDamageSum(talentRates, elementalReaction, 0);

        return (result, expectedDamage.Crit);
    }

    protected (float, Crit) ExpectedDmg(AttackType attackType, ElementType elementType, CharaData charaData, Data data, float talentRate, float elementalReaction = 1)
    {
        return ExpectedDmg(attackType, elementType, charaData, data, new[] { talentRate }, elementalReaction);
    }

    protected (float, Crit) ExpectedDmg(AttackType attackType, CharaData charaData, Data data, float[] talentRates, float elementalReaction = 1)
    {
        return ExpectedDmg(attackType, status.elementType, charaData, data, talentRates, elementalReaction);
    }

    protected (float, Crit) ExpectedDmg(AttackType attackType, CharaData charaData, Data data, float talentRate, float elementalReaction = 1)
    {
        return ExpectedDmg(attackType, status.elementType, charaData, data, new[] { talentRate }, elementalReaction);
    }

    protected (float, Crit) ExpectedDmg(AttackType attackType, CharaData charaData, Data data, float atkRate = 0, float defRate = 0, float hpRate = 0, float emRate = 0, float elementalReaction = 1, float addTalentRate = 0)
    {
        ExpectedDamage expectedDamage = new(attackType, charaData, data.artSub);
        charaData.dmg_bonus += data.ElementalDmgBonus(status.elementType);

        float result = expectedDamage.GetExpectedDamage_multi(atkRate, defRate, hpRate, emRate, elementalReaction, 0);
        return (result, expectedDamage.Crit);
    }

}

[Serializable]
public class Status
{
    public WeaponType weaponType = WeaponType.Catalyst;
    public ElementType elementType = ElementType.Electro;
    // 基礎ステータス Lv90
    public float baseAtk;
    public float baseDef;
    public int baseHp;
    public bool hasSelfHarm = false;
    public bool notUseShimenawa = false;
    public bool isFront = true;
    public bool isNightSoul = false;


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
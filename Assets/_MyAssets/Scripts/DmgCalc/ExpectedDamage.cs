using System;
using UnityEngine;

public class ExpectedDamage
{
    readonly float atk;
    readonly float def;
    readonly float hp;
    readonly float em;
    public float DmgAdd { get; private set; }
    public float DmgBonus { get; private set; }
    public float Res { get; private set; }
    public Crit Crit { get; private set; }
    public float Result { get; private set; }


    ExpectedDamage(AttackType attackType, ElementType elementType, Data data, ArtSubData artSub)
    {
        float dmgAdd = data.add;
        float dmgBonus = data.dmg_bonus + data.ElementalDmgBonus(elementType);
        float critRate = data.crit_rate;
        float critDmg = data.crit_dmg;
        float elementalRes = data.res + data.GetElementalRes(elementType);

        switch (attackType)
        {
            case AttackType.Normal:
                dmgAdd += data.add_normal_atk;
                dmgBonus += data.normal_atk_bonus;
                critRate += data.crit_rate_normal_atk;
                //  critDmg += data.crit_dmg_normal_atk();
                break;
            case AttackType.Charged:
                dmgAdd += data.add_charged_atk;
                dmgBonus += data.charged_atk_bonus;
                critRate += data.crit_rate_charged_atk;
                //critDmg += data.crit_dmg_charged_atk();
                break;
            case AttackType.Plugged:
                dmgAdd += data.add_plugged_atk;
                dmgBonus += data.plugged_atk_bonus;
                critRate += data.crit_rate_plugged_atk;
                critDmg += data.crit_dmg_plugged;
                break;
            case AttackType.Skill:
                dmgAdd += data.add_skill;
                dmgBonus += data.skill_bonus;
                critRate += data.crit_rate_skill;
                //  critDmg += data.crit_dmg_skill();
                break;
            case AttackType.Burst:
                dmgAdd += data.add_burst;
                dmgBonus += data.burst_bonus;
                critRate += data.crit_rate_burst;
                critDmg += data.crit_dmg_burst;
                break;
            default:
                break;
        }
        Crit = new Crit(critRate, critDmg, artSub);

        this.atk = data.atk;
        this.def = data.def;
        this.hp = data.hp;
        this.em = data.elemental_mastery;
        this.DmgAdd = dmgAdd;
        this.DmgBonus = dmgBonus;
        this.Res = GetElementalRes(elementalRes) * 0.5f;
    }


    public static ExpectedDamage Sum(
        Data data,
        AttackType attackType,
        float[] atkRates,
        ElementType elementType = ElementType.None,
        ReferenceStatus referenceStatus = ReferenceStatus.Atk,
        ElementalReaction elementalReaction = null)
    {
        if (elementType == ElementType.None) elementType = data.Status.elementType;
        ExpectedDamage expectedDamage = new(attackType, elementType, data, data.ArtSubData);
        expectedDamage.Result = expectedDamage.GetExpectedDamageSum(referenceStatus, atkRates, elementalReaction);
        return expectedDamage;
    }

    public static ExpectedDamage Single(
        Data data,
        AttackType attackType,
        float atkRate,
        ElementType elementType = ElementType.None,
        ReferenceStatus referenceStatus = ReferenceStatus.Atk,
        ElementalReaction elementalReaction = null)
    {
        if (elementType == ElementType.None) elementType = data.Status.elementType;

        ExpectedDamage expectedDamage = new(attackType, elementType, data, data.ArtSubData);

        expectedDamage.Result = expectedDamage.GetExpectedDamage(referenceStatus, atkRate, elementalReaction);
        return expectedDamage;
    }



    float GetExpectedDamage(
        ReferenceStatus referenceStatus,
        float atkRate,
        ElementalReaction elementalReaction)
    {

        float baseDamage = referenceStatus switch
        {
            ReferenceStatus.Atk => atk * atkRate,
            ReferenceStatus.Def => def * atkRate,
            ReferenceStatus.Hp => hp * atkRate,
            ReferenceStatus.Em => em * atkRate,
            _ => 0,
        };

        elementalReaction ??= new();

        baseDamage += DmgAdd + elementalReaction.result_er_add;
        float result = baseDamage * (1 + DmgBonus) * Crit.ExpectedCritDmg * Res * elementalReaction.result_er_multi;
        return result;
    }

    float GetExpectedDamageSum(
        ReferenceStatus referenceStatus,
        float[] atkRates,
        ElementalReaction elementalReaction)
    {
        float sum = 0;

        for (int i = 0; i < atkRates.Length; i++)
        {
            sum += GetExpectedDamage(referenceStatus, atkRates[i], elementalReaction);
        }
        return sum;
    }


    static float GetElementalRes(float decreasingRes)
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

public enum AttackType
{
    Normal,
    Charged,
    Plugged,
    Skill,
    Burst,
}

[System.Flags]
public enum ReferenceStatus
{
    None = -1,
    Atk = 0,
    Def = 1,
    Hp = 2,
    Em = 3,
}
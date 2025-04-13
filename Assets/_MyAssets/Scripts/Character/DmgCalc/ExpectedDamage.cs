using System;

public class ExpectedDamage
{
    readonly float atk;
    readonly float def;
    readonly float hp;
    readonly float em;
    readonly float dmgAdd;
    readonly float dmgBonus;
    readonly float expectedCritDmg;
    readonly float res;
    public Crit Crit { get; private set; }

    public ExpectedDamage(AttackType attackType, ElementType elementType, Data data, ArtSubData artSub)
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
        this.dmgAdd = dmgAdd;
        this.dmgBonus = dmgBonus;
        this.expectedCritDmg = Crit.ExpectedCritDmg;
        this.res = GetElementalRes(elementalRes) * 0.5f;
    }

    public float GetExpectedDamage(
        ReferenceStatus referenceStatus,
        float atkRate,
        float er_multi,
        float er_add)
    {

        float baseDamage = referenceStatus switch
        {
            ReferenceStatus.Atk => atk * atkRate,
            ReferenceStatus.Def => def * atkRate,
            ReferenceStatus.Hp => hp * atkRate,
            ReferenceStatus.Em => em * atkRate,
            _ => 0,
        };

        baseDamage += dmgAdd + er_add;
        float dmg = baseDamage * (1 + dmgBonus) * expectedCritDmg * res * er_multi;
        return dmg;
    }

    public float GetExpectedDamageSum(
        ReferenceStatus referenceStatus,
        float[] atkRates,
        float er_multi = 1,
        float er_add = 0)
    {
        float dmg = 0;

        for (int i = 0; i < atkRates.Length; i++)
        {
            dmg += GetExpectedDamage(referenceStatus, atkRates[i], er_multi, er_add);
        }
        return dmg;
    }


    float GetElementalRes(float decreasingRes)
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
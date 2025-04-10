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

    public ExpectedDamage(AttackType attackType, ElementType elementType, CharaData charaData, ArtSubData artSub)
    {
        float dmgAdd = charaData.add;
        float dmgBonus = charaData.dmg_bonus + charaData.ElementalDmgBonus(elementType);
        float critRate = charaData.crit_rate;
        float critDmg = charaData.crit_dmg;


        switch (attackType)
        {
            case AttackType.Normal:
                dmgAdd += charaData.add_normal_atk;
                dmgBonus += charaData.normal_atk_bonus;
                critRate += charaData.crit_rate_normal_atk;
                //  critDmg += data.crit_dmg_normal_atk();
                break;
            case AttackType.Charged:
                dmgAdd += charaData.add_charged_atk;
                dmgBonus += charaData.charged_atk_bonus;
                critRate += charaData.crit_rate_charged_atk;
                //critDmg += data.crit_dmg_charged_atk();
                break;
            case AttackType.Plugged:
                dmgAdd += charaData.add_plugged_atk;
                dmgBonus += charaData.plugged_atk_bonus;
                critRate += charaData.crit_rate_plugged_atk;
                critDmg += charaData.crit_dmg_plugged;
                break;
            case AttackType.Skill:
                dmgAdd += charaData.add_skill;
                dmgBonus += charaData.skill_bonus;
                critRate += charaData.crit_rate_skill;
                //  critDmg += data.crit_dmg_skill();
                break;
            case AttackType.Burst:
                dmgAdd += charaData.add_burst;
                dmgBonus += charaData.burst_bonus;
                critRate += charaData.crit_rate_burst;
                critDmg += charaData.crit_dmg_burst;
                break;
            default:
                break;
        }
        Crit = new Crit(critRate, critDmg, artSub);

        this.atk = charaData.atk;
        this.def = charaData.def;
        this.hp = charaData.hp;
        this.em = charaData.elemental_mastery;
        this.dmgAdd = dmgAdd;
        this.dmgBonus = dmgBonus;
        this.expectedCritDmg = Crit.ExpectedCritDmg;
        this.res = GetElementalRes(charaData.res) * 0.5f;
    }

    /*
        public float GetExpectedDamage(float talentRate, float er_multi = 1, float er_add = 0)
        {
            float dmg = (atk * talentRate + dmgAdd + er_add) * (1 + dmgBonus) * expectedCritDmg * res * er_multi;
            return dmg;
        }
   
    public float GetExpectedDamage(
        float atkRate,
        float defRate,
        float hpRate,
        float emRate,
        float er_multi,
        float er_add)
    {
        float adkDmg = atk * atkRate;
        float defDmg = def * defRate;
        float hpDmg = hp * hpRate;
        float emDmg = em * emRate;
        float baseDamage = adkDmg + defDmg + hpDmg + emDmg + dmgAdd + er_add;
        float dmg = baseDamage * (1 + dmgBonus) * expectedCritDmg * res * er_multi;
        return dmg;
    }
 */
    public float GetExpectedDamage(
        ReferenceStatus referenceStatus,
        float atkRate,
        float er_multi,
        float er_add)
    {

        float baseDamage = referenceStatus switch
        {
            ReferenceStatus.Atk => atk * atkRate,
            ReferenceStatus.Def => atk * atkRate,
            ReferenceStatus.Hp => atk * atkRate,
            ReferenceStatus.Em => atk * atkRate,
            _ => 0,
        };

        baseDamage += er_add;
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
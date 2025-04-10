public class ExpectedDamage
{
    readonly float atk;
    readonly float dmgAdd;
    readonly float dmgBonus;
    readonly float expectedCritDmg;
    readonly float res;
    public Crit Crit { get; private set; }

    public ExpectedDamage(float atk, float dmgAdd, float dmgBonus, float expectedCritDmg, float res)
    {
        this.atk = atk;
        this.dmgAdd = dmgAdd;
        this.dmgBonus = dmgBonus;
        this.expectedCritDmg = expectedCritDmg;
        this.res = res;
    }


    public ExpectedDamage(AttackType attackType, CharaData charaData, ArtSubData artSub)
    {
        float dmgAdd = charaData.add;
        float dmgBonus = charaData.dmg_bonus;
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
        this.dmgAdd = dmgAdd;
        this.dmgBonus = dmgBonus;
        this.expectedCritDmg = Crit.ExpectedCritDmg;
        this.res = GetElementalRes(charaData.res) * 0.5f;
    }

    public float GetExpectedDamage(float talentRate, float elementalReaction = 1, float addTalentRate = 0)
    {
        float dmg = (atk * (talentRate + addTalentRate) + dmgAdd) * (1 + dmgBonus) * expectedCritDmg * res * elementalReaction;
        return dmg;
    }

    public float GetExpectedDamageSum(float[] talentRates, float elementalReaction = 1, float addTalentRate = 0)
    {
        float dmg = 0;

        for (int i = 0; i < talentRates.Length; i++)
        {
            dmg += GetExpectedDamage(talentRates[i], elementalReaction, addTalentRate);
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
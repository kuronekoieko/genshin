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


    public ExpectedDamage(AttackType attackType, BaseData baseData, Data data)
    {
        float dmgAdd = baseData.add;
        float dmgBonus = baseData.dmg_bonus;
        float critRate = baseData.crit_rate;
        float critDmg = baseData.crit_dmg;

        switch (attackType)
        {
            case AttackType.Normal:
                dmgAdd += data.add_normal_atk();
                dmgBonus += data.normal_atk_bonus();
                critRate += data.crit_rate_normal_atk();
                //  critDmg += data.crit_dmg_normal_atk();
                break;
            case AttackType.Charged:
                dmgAdd += data.add_charged_atk();
                dmgBonus += data.charged_atk_bonus();
                critRate += data.crit_rate_charged_atk();
                //critDmg += data.crit_dmg_charged_atk();
                break;
            case AttackType.Plugged:
                dmgAdd += data.add_plugged_atk();
                dmgBonus += data.plugged_atk_bonus();
                critRate += data.crit_rate_plugged_atk();
                critDmg += data.crit_dmg_plugged();
                break;
            case AttackType.Skill:
                dmgAdd += data.add_skill();
                dmgBonus += data.skill_bonus();
                critRate += data.crit_rate_skill();
                //  critDmg += data.crit_dmg_skill();
                break;
            case AttackType.Burst:
                dmgAdd += data.add_burst();
                dmgBonus += data.burst_bonus();
                critRate += data.crit_rate_burst();
                critDmg += data.crit_dmg_burst();
                break;
            default:
                break;
        }
        Crit = new Crit(critRate, critDmg, data.artSub);

        this.atk = baseData.atk;
        this.dmgAdd = dmgAdd;
        this.dmgBonus = dmgBonus;
        this.expectedCritDmg = Crit.ExpectedCritDmg;
        this.res = baseData.res;
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
}


public enum AttackType
{
    Normal,
    Charged,
    Plugged,
    Skill,
    Burst,
}
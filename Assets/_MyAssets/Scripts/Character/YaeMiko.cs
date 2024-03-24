using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class YaeMiko
{
    // 基礎ステータス Lv90
    static float baseAtk = 340;
    static float baseDef = 569;
    static float baseCritRate = 0.05f;
    static float baseCritDmg = 0.5f;
    static float baseHp = 10372;
    static Ascend ascend = new();
    public static string weaponType = "法器";

    //突破ステータス
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

    // スキル Lv9
    static float[] skillPerArray = { 1.031f, 1.289f, 1.612f };

    // 固有天賦
    static float passive_dmgBonusPerEM = 0.15f * 0.01f;

    public static Dictionary<string, string> CalcDmg(WeaponData weapon, ArtMainData artMain, ArtSetData artSets, PartyData partyData, ArtSubData artSub)
    {

        float healPerSum
        = weapon.heal_bonus
        + artMain.heal_bonus
        + artSets.heal_bonus
        + partyData.heal_bonus
        + artSub.heal_bonus;

        float hpPerSum
        = weapon.hp_rate
        + artMain.hp_rate
        + artSets.hp_rate
        + partyData.hp_rate
        + artSub.hp_rate
        + ascend.hpPer;

        var hpSum
        = baseHp * (1 + hpPerSum)
        + weapon.hp
        + artMain.hp
        + artSets.hp
        + partyData.hp
        + artSub.hp;


        float energyRecharge
        = 1 + weapon.energy_recharge
        + artMain.energy_recharge
        + artSets.energy_recharge
        + partyData.energy_recharge
        + artSub.energy_recharge
        + ascend.energyRecharge;

        float elementalMastery
        = weapon.elemental_mastery
        + artMain.elemental_mastery
        + artSets.elemental_mastery
        + partyData.elemental_mastery
        + artSub.elemental_mastery;

        float defPerSum
        = weapon.def_rate
        + artMain.def_rate
        + artSets.def_rate
        + partyData.def_rate
        + artSub.def_rate;

        var def
        = baseDef * (1 + defPerSum)
        + weapon.def
        + artMain.def
        + artSets.def
        + partyData.def
        + artSub.def;

        float atkPerSum
        = weapon.atk_rate
        + artMain.atk_rate
        + artSets.atk_rate
        + partyData.atk_rate
        + artSub.atk_rate;


        var homa_atkAdd = hpSum * weapon.homa;
        var sekisa_atkAdd = elementalMastery * weapon.sekisha;

        var atk = (baseAtk + weapon.base_atk)
        * (1 + atkPerSum)
        + weapon.atk
        + artMain.atk
        + artSets.atk
        + partyData.atk
        + artSub.atk
        + homa_atkAdd
        + sekisa_atkAdd;

        float dmgBonus
        = weapon.dmg_bonus
        + artMain.dmg_bonus
        + artSets.dmg_bonus
        + partyData.dmg_bonus
        + artSub.dmg_bonus
        + ascend.dmgBonus;

        float normalAtkDmgBonus
        = weapon.normal_atk_bonus
        + artMain.normal_atk_bonus
        + artSets.normal_atk_bonus
        + partyData.normal_atk_bonus
        + artSub.normal_atk_bonus;

        float chargedAtkDmgBonus
              = weapon.charged_atk_bonus
              + artMain.charged_atk_bonus
              + artSets.charged_atk_bonus
              + partyData.charged_atk_bonus
              + artSub.charged_atk_bonus;

        float skillDmgBonus
              = weapon.skill_bonus
              + artMain.skill_bonus
              + artSets.skill_bonus
              + partyData.skill_bonus
              + artSub.skill_bonus
              + passive_dmgBonusPerEM * elementalMastery;

        float burstDmgBonus
              = weapon.burst_bonus
              + artMain.burst_bonus
              + artSets.burst_bonus
              + partyData.burst_bonus
              + artSub.burst_bonus;

        float attackSpeed
              = weapon.atk_speed
              + artMain.atk_speed
              + artSets.atk_speed
              + partyData.atk_speed
              + artSub.atk_speed;

        float critRate
              = weapon.crit_rate
              + artMain.crit_rate
              + artSets.crit_rate
              + partyData.crit_rate
              + artSub.crit_rate
              + ascend.critRate
              + baseCritRate;


        var critRate_skill
            = critRate
            + weapon.crit_rate_skill
            + artMain.crit_rate_skill
            + artSets.crit_rate_skill
            + partyData.crit_rate_skill
            + artSub.crit_rate_skill;

        var ritRate_burst
            = critRate
            + weapon.crit_rate_burst
            + artMain.crit_rate_burst
            + artSets.crit_rate_burst
            + partyData.crit_rate_burst
            + artSub.crit_rate_burst;

        float critDmg
                    = weapon.crit_dmg
                    + artMain.crit_dmg
                    + artSets.crit_dmg
                    + partyData.crit_dmg
                    + artSub.crit_dmg
                    + baseCritDmg
                    + ascend.critDmg;

        float dmgAdd
                    = weapon.add
                    + artMain.add
                    + artSets.add
                    + partyData.add
                    + artSub.add;

        var dmgAdd_sekikaku
            = def * weapon.sekikaku;

        var dmgAdd_talent = 0;
        //= hpSum * (burst_addDmgPerHp + passive_addDmgPerHeal * healPerSum)

        var dmgAdd_normalAttack
        = weapon.add_normal_atk
        + artMain.add_normal_atk
        + artSets.add_normal_atk
        + partyData.add_normal_atk
        + artSub.add_normal_atk
        + dmgAdd_sekikaku
        + dmgAdd_talent;

        var dmgAdd_chargedAttack
            = dmgAdd_sekikaku;
        // = getNum(weapon, "狩人ダメージアップ")
        // * elementalMastery;

        var dmgAdd_skill
        = weapon.add_skill
        + artMain.add_skill
        + artSets.add_skill
        + partyData.add_skill
        + artSub.add_skill;

        var crit_skill = Crit.GetCrit(critRate_skill, critDmg, artSub);
        // var crit_ChargedAttack = Crit.GetCrit(critRate, critDmg, artSub);
        //var crit_normalAttack = Crit.GetCrit(critRate, critDmg, artSub);

        var melt = ElementalReaction.MeltForPyro(elementalMastery, 0);
        var vaporize = ElementalReaction.VaporizeForPyro(elementalMastery, artSets.er_rate);

        var addAggravate
          = ElementalReaction.Aggravate(elementalMastery, artSets.er_aggravate);

        var enemyRES = GetElementalRes(partyData.res) * 0.5f;

        /*
                var expectedDmg
                  = GetExpectedDamageSum(
                    atk,
                    skillPerArray,
                    dmgAdd + dmgAdd_skill,
                    dmgBonus + skillDmgBonus,
                    crit_skill.ExpectedCritDmg,
                    enemyRES,
                    1);
        */
        var expectedDmg_gekika
          = GetExpectedDamage(
            atk,
            skillPerArray[0],
            dmgAdd + dmgAdd_skill + addAggravate,
            dmgBonus + skillDmgBonus,
            crit_skill.ExpectedCritDmg,
            enemyRES,
            1);
        expectedDmg_gekika
          += GetExpectedDamage(
            atk,
            skillPerArray[1],
            dmgAdd + dmgAdd_skill,
            dmgBonus + skillDmgBonus,
            crit_skill.ExpectedCritDmg,
            enemyRES,
            1);

        expectedDmg_gekika
          += GetExpectedDamage(
            atk,
            skillPerArray[2],
            dmgAdd + dmgAdd_skill,
            dmgBonus + skillDmgBonus,
            crit_skill.ExpectedCritDmg,
            enemyRES,
            1);

        Dictionary<string, string> result = new()
        {
            ["武器"] = weapon.name,
            ["聖遺物セット"] = artSets.name,
            ["聖遺物メイン"] = artMain.name,
            ["バフキャラ"] = partyData.name,
            ["スキル期待値"] = expectedDmg_gekika.ToString(),
            ["攻撃力"] = atk.ToString(),
            ["HP"] = hpSum.ToString(),
            ["バフ"] = dmgBonus.ToString(),
            ["会心ダメ期待値"] = crit_skill.ExpectedCritDmg.ToString(),
            ["熟知"] = elementalMastery.ToString(),
            ["率ダメ"] = crit_skill.RateDmg,
            ["会心ダメ比率"] = crit_skill.CritProportion,
            ["聖遺物組み合わせ"] = artSub.name,
            ["サブステ"] = crit_skill.SubCritRate.ToString(),
            ["サブHP%"] = artSub.hp_rate.ToString(),
            ["サブHP"] = artSub.hp.ToString(),
            ["スコア"] = artSub.score.ToString()
        };

        //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));

        return result;
    }

    static float GetExpectedDamage(float atk, float talentRate, float dmgAdd, float dmgBonus, float expectedCritDmg, float res, float elementalReaction)
    {
        float dmg = 0;

        dmg += (atk * talentRate + dmgAdd) * (1 + dmgBonus) * expectedCritDmg * res * elementalReaction;

        return dmg;
    }

    static float GetExpectedDamageSum(float atk, float[] talentRates, float dmgAdd, float dmgBonus, float expectedCritDmg, float res, float elementalReaction)
    {
        float dmg = 0;

        for (int i = 0; i < talentRates.Length; i++)
        {
            dmg += (atk * talentRates[i] + dmgAdd) * (1 + dmgBonus) * expectedCritDmg * res * elementalReaction;
        }

        return dmg;
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

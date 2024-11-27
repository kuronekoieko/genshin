using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class YaeMiko : BaseCharacter
{
    // スキル Lv9
    float[] skillPerArray = { 1.031f, 1.289f, 1.612f };

    // 固有天賦
    float passive_dmgBonusPerEM = 0.15f * 0.01f;


    public override Dictionary<string, string> CalcDmg(Data data)
    {
        float healPerSum = data.heal_bonus();

        float hpPerSum
            = data.hpPerSum();

        var hpSum
            = status.baseHp * (1 + hpPerSum)
            + data.hp();


        float energyRecharge
            = 1 + data.energy_recharge();

        float elementalMastery
            = data.elemental_mastery();

        float defPerSum
            = data.def_rate();

        var def
            = status.baseDef * (1 + defPerSum)
            + data.def();

        float atkPerSum
            = data.atk_rate();

        var homa_atkAdd = hpSum * data.weapon.homa;
        var sekisa_atkAdd = elementalMastery * data.weapon.sekisha;

        var atk
            = (status.baseAtk + data.weapon.base_atk)
            * (1 + atkPerSum)
            + data.atk()
            + homa_atkAdd
            + sekisa_atkAdd;

        float dmgBonus
            = data.dmg_bonus() + data.ElementalDmgBonus();

        float normalAtkDmgBonus
            = data.normal_atk_bonus();

        float chargedAtkDmgBonus
            = data.charged_atk_bonus();

        float skillDmgBonus
            = data.skill_bonus()
            + passive_dmgBonusPerEM * elementalMastery;

        float burstDmgBonus
            = data.burst_bonus();

        float attackSpeed
            = data.atk_speed();

        float critRate
            = data.crit_rate();


        var critRate_skill
            = critRate
            + data.crit_rate_skill();

        var ritRate_burst
            = critRate
            + data.crit_rate_burst();

        float critDmg
            = data.crit_dmg();

        float dmgAdd = data.add();

        var dmgAdd_sekikaku
            = def * data.weapon.sekikaku;

        var dmgAdd_talent = 0;
        //= hpSum * (burst_addDmgPerHp + passive_addDmgPerHeal * healPerSum)

        var dmgAdd_normalAttack
        = data.add_normal_atk()
        + dmgAdd_sekikaku
        + dmgAdd_talent;

        var dmgAdd_chargedAttack = dmgAdd_sekikaku;
        // = getNum(weapon, "狩人ダメージアップ")
        // * elementalMastery;

        var dmgAdd_skill = data.add_skill();

        var crit_skill = new Crit(critRate_skill, critDmg, data.artSub);
        // var crit_ChargedAttack = new Crit(critRate, critDmg, artSub);
        //var crit_normalAttack = new Crit(critRate, critDmg, artSub);

        var melt = ElementalReaction.MeltForPyro(elementalMastery, 0);
        var vaporize = ElementalReaction.VaporizeForPyro(elementalMastery, data.artSetData.er_rate);

        var addAggravate
          = ElementalReaction.Aggravate(elementalMastery, data.artSetData.er_aggravate);

        var enemyRES = GetElementalRes(data.partyData.res) * 0.5f;

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
            ["武器"] = data.weapon.name,
            ["聖遺物セット"] = data.artSetData.name,
            ["聖遺物メイン"] = data.artMainData.name,
            ["バフキャラ"] = data.partyData.name,
            ["スキル期待値"] = expectedDmg_gekika.ToString(),
            ["攻撃力"] = atk.ToString(),
            ["HP"] = hpSum.ToString(),
            ["バフ"] = dmgBonus.ToString(),
            ["会心ダメ期待値"] = crit_skill.ExpectedCritDmg.ToString(),
            ["熟知"] = elementalMastery.ToString(),
            ["率ダメ"] = crit_skill.RateDmg,
            ["会心ダメ比率"] = crit_skill.CritProportion,
            ["聖遺物組み合わせ"] = data.artSub.name,
            ["サブステ"] = crit_skill.SubCrit.ToString(),
            ["サブHP%"] = data.artSub.hp_rate.ToString(),
            ["サブHP"] = data.artSub.hp.ToString(),
            ["スコア"] = data.artSub.Score.ToString()
        };

        //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));

        return result;
    }



}

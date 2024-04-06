using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chiori : BaseCharacter
{
    // スキル Lv9
    // float[] normalAtkPerArray_atk = { 0.908f, 0.860f, 0.559f, 0.559f, 1.38f };
    float[] normalAtkPerArray_atk = { 0.908f };

    float[] skillPerArray = { 1.40f };
    float skillAddPerDef = 1.74f;
    // 固有天賦
    //  float passive_dmgBonusPerEM = 0.15f * 0.01f;
    float constellation_addNormalAtkPerDef = 2.35f;


    public override Dictionary<string, string> CalcDmg(Data data)
    {
        float healPerSum = data.heal_bonus();
        float hpPerSum = data.hpPerSum();


        var hpSum
            = status.baseHp * (1 + hpPerSum)
            + data.hp();


        float energyRecharge = 1 + data.energy_recharge();

        float elementalMastery = data.elemental_mastery();

        float defPerSum = data.def_rate();

        var def = status.baseDef * (1 + defPerSum) + data.def();

        float atkPerSum = data.atk_rate();

        var homa_atkAdd = hpSum * data.weapon.homa;
        var sekisa_atkAdd = elementalMastery * data.weapon.sekisha;

        var atk
            = data.base_atk() * (1 + atkPerSum)
            + data.atk()
            + homa_atkAdd
            + sekisa_atkAdd;

        float dmgBonus
            = data.dmg_bonus()
            + ElementalDmgBonus(data);

        float normalAtkDmgBonus = data.normal_atk_bonus();

        float chargedAtkDmgBonus = data.charged_atk_bonus();

        float skillDmgBonus = data.skill_bonus();

        float burstDmgBonus = data.burst_bonus();

        float attackSpeed = data.atk_speed();

        float critRate = data.crit_rate();


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
        + dmgAdd_talent
        + def * constellation_addNormalAtkPerDef;

        var dmgAdd_chargedAttack = dmgAdd_sekikaku;
        // = getNum(weapon, "狩人ダメージアップ")
        // * elementalMastery;

        var dmgAdd_skill
        = data.add_skill()
        + def * data.weapon.cinnabar
        + def * skillAddPerDef;

        var crit_skill = Crit.GetCrit(critRate_skill, critDmg, data.artSub);
        // var crit_ChargedAttack = Crit.GetCrit(critRate, critDmg, artSub);
        var crit_normalAttack = Crit.GetCrit(critRate, critDmg, data.artSub);

        var melt = ElementalReaction.MeltForPyro(elementalMastery, 0);
        var vaporize = ElementalReaction.VaporizeForPyro(elementalMastery, data.artSets.er_rate);

        var addAggravate
          = ElementalReaction.Aggravate(elementalMastery, data.artSets.er_aggravate);

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
        var expectedDmg_skill
          = GetExpectedDamage(
            def,
            skillPerArray[0],
            dmgAdd + dmgAdd_skill,
            dmgBonus + skillDmgBonus,
            crit_skill.ExpectedCritDmg,
            enemyRES,
            1);

        var expectedDmg_normalAtk
            = GetExpectedDamageSum(
atk,
normalAtkPerArray_atk,
dmgAdd + dmgAdd_normalAttack,
dmgBonus + normalAtkDmgBonus,
crit_normalAttack.ExpectedCritDmg,
enemyRES,
1);

        Dictionary<string, string> result = new()
        {
            ["武器"] = data.weapon.name,
            ["聖遺物セット"] = data.artSets.name,
            ["聖遺物メイン"] = data.artMain.name,
            ["バフキャラ"] = data.partyData.name,
            ["通常期待値"] = expectedDmg_normalAtk.ToString(),
            ["スキル期待値"] = expectedDmg_skill.ToString(),
            ["攻撃力"] = atk.ToString(),
            ["防御力"] = def.ToString(),
            ["HP"] = hpSum.ToString(),
            ["バフ"] = dmgBonus.ToString(),
            ["会心ダメ期待値"] = crit_normalAttack.ExpectedCritDmg.ToString(),
            ["熟知"] = elementalMastery.ToString(),
            ["率ダメ"] = crit_normalAttack.RateDmg,
            ["会心ダメ比率"] = crit_normalAttack.CritProportion,
            ["聖遺物組み合わせ"] = data.artSub.name,
            ["サブステ"] = crit_normalAttack.SubCrit.ToString(),
            ["サブHP%"] = data.artSub.hp_rate.ToString(),
            ["サブHP"] = data.artSub.hp.ToString(),
            ["スコア"] = data.artSub.score.ToString(),
        };

        //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));

        return result;
    }
}

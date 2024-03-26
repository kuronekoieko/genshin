using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Albedo : BaseCharacter
{
    // スキル Lv9
    float[] skillPerArray = { 2.27f };

    // 固有天賦
    //  float passive_dmgBonusPerEM = 0.15f * 0.01f;


    public override Dictionary<string, string> CalcDmg(Datas datas)
    {
        float healPerSum = datas.heal_bonus();

        float hpPerSum
            = datas.hpPerSum()
            + datas.artSub.hp_rate
            + characterSO.ascend.hpPer;

        var hpSum
            = characterSO.status.baseHp * (1 + hpPerSum)
            + datas.hp();


        float energyRecharge
            = 1 + datas.energy_recharge()
            + characterSO.ascend.energyRecharge;

        float elementalMastery
            = datas.elemental_mastery();

        float defPerSum
            = datas.def_rate();

        var def
            = characterSO.status.baseDef * (1 + defPerSum)
            + datas.def();

        float atkPerSum
            = datas.atk_rate()
            + characterSO.ascend.atkPer;

        var homa_atkAdd = hpSum * datas.weapon.homa;
        var sekisa_atkAdd = elementalMastery * datas.weapon.sekisha;

        var atk
            = (characterSO.status.baseAtk + datas.weapon.base_atk)
            * (1 + atkPerSum)
            + datas.atk()
            + homa_atkAdd
            + sekisa_atkAdd;

        float dmgBonus
            = datas.dmg_bonus()
            + characterSO.ascend.dmgBonus;

        float normalAtkDmgBonus
            = datas.normal_atk_bonus();

        float chargedAtkDmgBonus
            = datas.charged_atk_bonus();

        float skillDmgBonus
            = datas.skill_bonus();

        float burstDmgBonus
            = datas.burst_bonus();

        float attackSpeed
            = datas.atk_speed();

        float critRate
            = datas.crit_rate()
            + characterSO.ascend.critRate
            + characterSO.status.baseCritRate;


        var critRate_skill
            = critRate
            + datas.crit_rate_skill();

        var ritRate_burst
            = critRate
            + datas.crit_rate_burst();

        float critDmg
            = datas.crit_dmg()
            + characterSO.ascend.critDmg
            + characterSO.status.baseCritDmg;

        float dmgAdd = datas.add();

        var dmgAdd_sekikaku
            = def * datas.weapon.sekikaku;

        var dmgAdd_talent = 0;
        //= hpSum * (burst_addDmgPerHp + passive_addDmgPerHeal * healPerSum)

        var dmgAdd_normalAttack
        = datas.add_normal_atk()
        + dmgAdd_sekikaku
        + dmgAdd_talent;

        var dmgAdd_chargedAttack = dmgAdd_sekikaku;
        // = getNum(weapon, "狩人ダメージアップ")
        // * elementalMastery;

        var dmgAdd_skill
        = datas.add_skill()
        + def * datas.weapon.cinnabar;

        var crit_skill = Crit.GetCrit(critRate_skill, critDmg, datas.artSub);
        // var crit_ChargedAttack = Crit.GetCrit(critRate, critDmg, artSub);
        //var crit_normalAttack = Crit.GetCrit(critRate, critDmg, artSub);

        var melt = ElementalReaction.MeltForPyro(elementalMastery, 0);
        var vaporize = ElementalReaction.VaporizeForPyro(elementalMastery, datas.artSets.er_rate);

        var addAggravate
          = ElementalReaction.Aggravate(elementalMastery, datas.artSets.er_aggravate);

        var enemyRES = GetElementalRes(datas.partyData.res) * 0.5f;

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
            def,
            skillPerArray[0],
            dmgAdd + dmgAdd_skill,
            dmgBonus + skillDmgBonus,
            crit_skill.ExpectedCritDmg,
            enemyRES,
            1);

        Dictionary<string, string> result = new()
        {
            ["武器"] = datas.weapon.name,
            ["聖遺物セット"] = datas.artSets.name,
            ["聖遺物メイン"] = datas.artMain.name,
            ["バフキャラ"] = datas.partyData.name,
            ["スキル期待値"] = expectedDmg_gekika.ToString(),
            ["攻撃力"] = atk.ToString(),
            ["HP"] = hpSum.ToString(),
            ["バフ"] = dmgBonus.ToString(),
            ["会心ダメ期待値"] = crit_skill.ExpectedCritDmg.ToString(),
            ["熟知"] = elementalMastery.ToString(),
            ["率ダメ"] = crit_skill.RateDmg,
            ["会心ダメ比率"] = crit_skill.CritProportion,
            ["聖遺物組み合わせ"] = datas.artSub.name,
            ["サブステ"] = crit_skill.SubCritRate.ToString(),
            ["サブHP%"] = datas.artSub.hp_rate.ToString(),
            ["サブHP"] = datas.artSub.hp.ToString(),
            ["スコア"] = datas.artSub.score.ToString()
        };

        //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));

        return result;
    }
}

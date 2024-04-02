using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Xiao : BaseCharacter
{
    // スキル Lv9
    //float[] normalAtkPerArray = { 1.456f, 1.403f, 1.684f, 2.154f, };
    // float[] chargedAtkPerArray = { 4.04f, };
    //float[] chargedAtkPerArray = { 1.675f };
    float[] pluggedAtkPerArray = { 4.04f, };

    float burst_bonus = 0.952f + 0.175f;
    // float talent_addDmg_chargedAtk_PerDef = 0.35f;


    public override Dictionary<string, string> CalcDmg(Datas datas)
    {
        // if (datas.energy_recharge() < 0.5f) return null;
        // if (datas.weapon.name != "草薙の稲光") return null;
        // if (datas.weapon.name != "和璞鳶") return null;

        float healPerSum = datas.heal_bonus();
        float hpPerSum = datas.hpPerSum();


        var hpSum
            = status.baseHp * (1 + hpPerSum)
            + datas.hp();


        float energyRecharge = 1 + datas.energy_recharge();

        float elementalMastery = datas.elemental_mastery();

        float defPerSum = datas.def_rate();

        var def = status.baseDef * (1 + defPerSum) + datas.def();

        float atkPerSum = datas.atk_rate();

        var homa_atkAdd = hpSum * datas.weapon.homa;
        var sekisa_atkAdd = elementalMastery * datas.weapon.sekisha;
        var kusanagi_atkAdd = (energyRecharge - 1) * datas.weapon.kusanagi;

        var atk
            = datas.base_atk() * (1 + atkPerSum)
            + datas.atk()
            + homa_atkAdd
            + sekisa_atkAdd
            + kusanagi_atkAdd;


        float dmgBonus
            = datas.dmg_bonus()
            + ElementalDmgBonus(datas);

        float normalAtkDmgBonus = datas.normal_atk_bonus();

        float chargedAtkDmgBonus = datas.charged_atk_bonus();

        float pluggedAtkDmgBonus = datas.plugged_atk_bonus() + burst_bonus;

        float skillDmgBonus = datas.skill_bonus();

        float burstDmgBonus = datas.burst_bonus();

        float attackSpeed = datas.atk_speed();

        float critRate = datas.crit_rate();

        var critRate_normalAtk = critRate + datas.crit_rate_normal_atk();
        var critRate_chargedAtk = critRate + datas.crit_rate_charged_atk();
        var critRate_pluggedAtk = critRate + datas.crit_rate_plugged_atk();
        var critRate_skill = critRate + datas.crit_rate_skill();
        var ritRate_burst = critRate + datas.crit_rate_burst();

        float critDmg
            = datas.crit_dmg();

        float dmgAdd = datas.add();

        var dmgAdd_sekikaku = def * datas.weapon.sekikaku;


        var dmgAdd_normalAttack
        = datas.add_normal_atk()
        + dmgAdd_sekikaku;

        var dmgAdd_chargedAttack
        = dmgAdd_sekikaku;

        var dmgAdd_pluggedAttack
        = datas.add_plugged_atk();


        // = getNum(weapon, "狩人ダメージアップ")
        // * elementalMastery;

        var dmgAdd_skill
        = datas.add_skill()
        + def * datas.weapon.cinnabar;

        var crit_ChargedAttack = Crit.GetCrit(critRate_chargedAtk, critDmg, datas.artSub);
        var crit_normalAttack = Crit.GetCrit(critRate_normalAtk, critDmg, datas.artSub);
        var crit_pluggedAttack = Crit.GetCrit(critRate_pluggedAtk, critDmg, datas.artSub);
        var crit_skill = Crit.GetCrit(critRate_skill, critDmg, datas.artSub);

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
        var expectedDmg_pluggedAtk
          = GetExpectedDamageSum(
            atk,
            pluggedAtkPerArray,
            dmgAdd + dmgAdd_pluggedAttack,
            dmgBonus + pluggedAtkDmgBonus,
            crit_pluggedAttack.ExpectedCritDmg,
            enemyRES,
            1);

        /*        var expectedDmg_chargedAtk
      = GetExpectedDamageSum(
        atk,
        chargedAtkPerArray,
        dmgAdd + dmgAdd_chargedAttack,
        dmgBonus + chargedAtkDmgBonus,
        crit_ChargedAttack.ExpectedCritDmg,
        enemyRES,
        1);
*/

        /*      var expectedDmg_normalAtk
                    = GetExpectedDamageSum(
        atk,
        normalAtkPerArray,
        dmgAdd + dmgAdd_normalAttack,
        dmgBonus + normalAtkDmgBonus,
        crit_normalAttack.ExpectedCritDmg,
        enemyRES,
        1);
        */

        var sum = expectedDmg_pluggedAtk;

        Dictionary<string, string> result = new()
        {
            ["武器"] = datas.weapon.name,
            ["聖遺物セット"] = datas.artSets.name,
            ["聖遺物メイン"] = datas.artMain.name,
            ["バフキャラ"] = datas.partyData.name,
            ["合計期待値"] = sum.ToString(),
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

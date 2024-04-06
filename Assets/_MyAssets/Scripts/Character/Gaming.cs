using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaming : BaseCharacter
{
    float[] pluggedAtkPerArray = { 3.91f, };

    float talent_addDmgBonusChargedAtk = 0.2f;

    float constellation_atkRate = 0.2f;
    float constellation_critRate = 0.2f;
    float constellation_critDmg = 0.4f;


    public override Dictionary<string, string> CalcDmg(Data data)
    {
        if (data.partyData.name.Contains("フリーナ") == false) return null;
        // if (data.energy_recharge() < 0.5f) return null;
        // if (data.weapon.name != "草薙の稲光") return null;
        // if (data.weapon.name != "和璞鳶") return null;

        float healPerSum = data.heal_bonus();
        float hpPerSum = data.hpPerSum();


        var hpSum = status.baseHp * (1 + hpPerSum) + data.hp();


        float energyRecharge = 1 + data.energy_recharge();

        float elementalMastery = data.elemental_mastery();

        float defPerSum = data.def_rate();

        var def = status.baseDef * (1 + defPerSum) + data.def();

        float atkPerSum = data.atk_rate() + constellation_atkRate;

        var homa_atkAdd = hpSum * data.weapon.homa;
        var sekisa_atkAdd = elementalMastery * data.weapon.sekisha;
        var kusanagi_atkAdd = (energyRecharge - 1) * data.weapon.kusanagi;

        var atk
            = data.base_atk() * (1 + atkPerSum)
            + data.atk()
            + homa_atkAdd
            + sekisa_atkAdd
            + kusanagi_atkAdd;


        float dmgBonus
            = data.dmg_bonus()
            + ElementalDmgBonus(data);

        float normalAtkDmgBonus = data.normal_atk_bonus();

        float chargedAtkDmgBonus = data.charged_atk_bonus();

        float pluggedAtkDmgBonus = data.plugged_atk_bonus() + talent_addDmgBonusChargedAtk;

        float skillDmgBonus = data.skill_bonus();

        float burstDmgBonus = data.burst_bonus();

        float attackSpeed = data.atk_speed();

        float critRate = data.crit_rate();

        var critRate_normalAtk = critRate + data.crit_rate_normal_atk();
        var critRate_chargedAtk = critRate + data.crit_rate_charged_atk();
        var critRate_pluggedAtk = critRate + data.crit_rate_plugged_atk() + constellation_critRate;
        var critRate_skill = critRate + data.crit_rate_skill();
        var ritRate_burst = critRate + data.crit_rate_burst();

        float critDmg = data.crit_dmg();
        float critDmg_pluggedAtk = critDmg + constellation_critDmg;


        float dmgAdd = data.add();

        var dmgAdd_sekikaku = def * data.weapon.sekikaku;


        var dmgAdd_normalAttack
        = data.add_normal_atk()
        + dmgAdd_sekikaku;

        var dmgAdd_chargedAttack
        = dmgAdd_sekikaku;

        var dmgAdd_pluggedAttack
        = data.add_plugged_atk();


        // = getNum(weapon, "狩人ダメージアップ")
        // * elementalMastery;

        var dmgAdd_skill
        = data.add_skill()
        + def * data.weapon.cinnabar;

        var crit_ChargedAttack = Crit.GetCrit(critRate_chargedAtk, critDmg, data.artSub);
        var crit_normalAttack = Crit.GetCrit(critRate_normalAtk, critDmg, data.artSub);
        var crit_pluggedAttack = Crit.GetCrit(critRate_pluggedAtk, critDmg_pluggedAtk, data.artSub);
        var crit_skill = Crit.GetCrit(critRate_skill, critDmg, data.artSub);

        var melt = ElementalReaction.MeltForPyro(elementalMastery, 0);
        var vaporize = ElementalReaction.VaporizeForPyro(elementalMastery, data.er_rate());

        var addAggravate = ElementalReaction.Aggravate(elementalMastery, data.er_aggravate());

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
        var expectedDmg_pluggedAtk
          = GetExpectedDamageSum(
            atk,
            pluggedAtkPerArray,
            dmgAdd + dmgAdd_pluggedAttack,
            dmgBonus + pluggedAtkDmgBonus,
            crit_pluggedAttack.ExpectedCritDmg,
            enemyRES,
            vaporize);

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
            ["武器"] = data.weapon.name,
            ["聖遺物セット"] = data.artSets.name,
            ["聖遺物メイン"] = data.artMain.name,
            ["バフキャラ"] = data.partyData.name,
            ["合計期待値"] = sum.ToString(),
            ["攻撃力"] = atk.ToString(),
            ["HP"] = hpSum.ToString(),
            ["バフ"] = dmgBonus.ToString(),
            ["会心ダメ期待値"] = crit_skill.ExpectedCritDmg.ToString(),
            ["熟知"] = elementalMastery.ToString(),
            ["率ダメ"] = crit_skill.RateDmg,
            ["会心ダメ比率"] = crit_skill.CritProportion,
            ["聖遺物組み合わせ"] = data.artSub.name,
            ["サブステ"] = crit_skill.SubCritRate.ToString(),
            ["サブHP%"] = data.artSub.hp_rate.ToString(),
            ["サブHP"] = data.artSub.hp.ToString(),
            ["スコア"] = data.artSub.Score.ToString()
        };

        //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));

        return result;
    }
}

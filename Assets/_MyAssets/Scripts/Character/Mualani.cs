using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mualani : BaseCharacter
{
    // lv9
    readonly static float normalAtkPer = 0.1476f + 0.3689f + 0.0738f * 3f;
    readonly float[] normalAtkPerArray = { normalAtkPer + const_addAtkPerHp, normalAtkPer, normalAtkPer };

    readonly static float const_addAtkPerHp = 0.66f;




    public override Dictionary<string, string> CalcDmg(Data data)
    {
        if (data.partyData.name.Contains("トーマ") == false) return null;
        // if (data.partyData.name.Contains("夜蘭") == false) return null;
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

        float atkPerSum = data.atk_rate();

        var homa_atkAdd = hpSum * data.weapon.homa;
        var sekisa_atkAdd = elementalMastery * data.weapon.sekisha;
        var kusanagi_atkAdd = (energyRecharge - 1) * data.weapon.kusanagi;

        var atk
            = data.base_atk() * (1 + atkPerSum)
            + data.atk()
            + homa_atkAdd
            + sekisa_atkAdd
            + kusanagi_atkAdd;


        float dmgBonus = data.dmg_bonus() + data.ElementalDmgBonus();

        float normalAtkDmgBonus = data.normal_atk_bonus();

        float chargedAtkDmgBonus = data.charged_atk_bonus();

        float pluggedAtkDmgBonus = data.plugged_atk_bonus();

        float skillDmgBonus = data.skill_bonus();

        float burstDmgBonus = data.burst_bonus();

        float attackSpeed = data.atk_speed();

        float critRate = data.crit_rate();

        var critRate_normalAtk = critRate + data.crit_rate_normal_atk();
        var critRate_chargedAtk = critRate + data.crit_rate_charged_atk();
        var critRate_pluggedAtk = critRate + data.crit_rate_plugged_atk();
        var critRate_skill = critRate + data.crit_rate_skill();
        var ritRate_burst = critRate + data.crit_rate_burst();

        float critDmg = data.crit_dmg();
        float critDmg_pluggedAtk = critDmg;


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

        // Crit.GetCritが重いので、使わないときはコメントアウト
        //var crit_ChargedAttack = new Crit(critRate_chargedAtk, critDmg, data.artSub);
        var crit_normalAttack = new Crit(critRate_normalAtk, critDmg, data.artSub);
        // var crit_pluggedAttack = new Crit(critRate_pluggedAtk, critDmg_pluggedAtk, data.artSub);
        // var crit_skill = new Crit(critRate_skill, critDmg, data.artSub);

        var melt = ElementalReaction.MeltForPyro(elementalMastery, 0);
        var vaporizeForPyro = ElementalReaction.VaporizeForPyro(elementalMastery, data.er_rate());
        var vaporizeForHydro = ElementalReaction.VaporizeForHydro(elementalMastery, data.er_rate());

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

        /*
        var expectedDmg_pluggedAtk
          = GetExpectedDamageSum(
            atk,
            pluggedAtkPerArray,
            dmgAdd + dmgAdd_pluggedAttack,
            dmgBonus + pluggedAtkDmgBonus,
            crit_pluggedAttack.ExpectedCritDmg,
            enemyRES,
            vaporize);
*/

        /*
                var expectedDmg_chargedAtk
                    = GetExpectedDamageSum(
                atk,
                chargedAtkPerArray,
                dmgAdd + dmgAdd_chargedAttack,
                dmgBonus + chargedAtkDmgBonus,
                crit_ChargedAttack.ExpectedCritDmg,
                enemyRES,
                vaporize);
        */

        var expectedDmg_normalAtk
        = GetExpectedDamageSum(
        hpSum,
        normalAtkPerArray,
        dmgAdd + dmgAdd_normalAttack,
        dmgBonus + normalAtkDmgBonus,
        crit_normalAttack.ExpectedCritDmg,
        enemyRES,
        vaporizeForHydro);

        var expectedDmg_normalAtk_1
        = GetExpectedDamage(
        hpSum,
        normalAtkPer + const_addAtkPerHp,
        dmgAdd + dmgAdd_normalAttack,
        dmgBonus + normalAtkDmgBonus,
        crit_normalAttack.ExpectedCritDmg,
        enemyRES,
        vaporizeForHydro);

        //        var hpSum = status.baseHp * (1 + hpPerSum) + data.hp();


        var sum = expectedDmg_normalAtk;

        Dictionary<string, string> result = new()
        {
            ["武器"] = data.weapon.name,
            ["聖遺物セット"] = data.artSetData.name,
            ["聖遺物メイン"] = data.artMainData.name,
            ["バフキャラ"] = data.partyData.name,
            ["合計期待値"] = sum.ToString(),
            ["一発目期待値"] = expectedDmg_normalAtk_1.ToString(),
            ["攻撃力"] = atk.ToString(),
            ["防御力"] = def.ToString(),
            ["HP"] = hpSum.ToString(),
            ["共通バフ"] = dmgBonus.ToString(),
            ["通常バフ"] = normalAtkDmgBonus.ToString(),
            // ["会心ダメ期待値"] = crit_ChargedAttack.ExpectedCritDmg.ToString(),
            ["熟知"] = elementalMastery.ToString(),
            ["率ダメ"] = crit_normalAttack.RateDmg,
            // ["会心ダメ比率"] = crit_ChargedAttack.CritProportion,
            ["聖遺物組み合わせ"] = data.artSub.name,
            ["サブステ"] = crit_normalAttack.SubCrit.ToString(),
            // ["サブHP%"] = data.artSub.hp_rate.ToString(),
            // ["サブHP"] = data.artSub.hp.ToString(),
            ["スコア"] = data.artSub.Score.ToString(),
        };

        //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));

        return result;
    }
}

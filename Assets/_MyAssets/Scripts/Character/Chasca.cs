using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chasca : BaseCharacter
{
  // lv9
  readonly static float chargedAtkRate_anemo = 0.878f;
  readonly static float chargedAtkRate_otherElement = 2.998f;

  // readonly float[] normalAtkPerArray = { normalAtkPer + const_addAtkPerHp, normalAtkPer, normalAtkPer };

  // readonly static float[] talent_chargedAtkRateAry = { 2.998f };
  readonly static float talent_chargedAtkDmgBonus = 0.65f;//




  public override Dictionary<string, string> CalcDmg(Data data)
  {
    //  if (data.memberData.name.Contains("トーマ") == false) return null;
    // if (data.memberData.name.Contains("夜蘭") == false) return null;
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


    float dmgBonus = data.dmg_bonus();
    float pyro_bonus = data.pyro_bonus();
    float hydro_bonus = data.hydro_bonus();
    float electro_bonus = data.electro_bonus();
    float cryo_bonus = data.cryo_bonus();
    float geo_bonus = data.geo_bonus();
    float anemo_bonus = data.anemo_bonus();
    float dendro_bonus = data.dendro_bonus();


    float normalAtkDmgBonus = data.normal_atk_bonus();

    float chargedAtkDmgBonus = data.charged_atk_bonus() + talent_chargedAtkDmgBonus;

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
    float add_cryo = data.add_cryo();


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
    var crit_ChargedAttack = new Crit(critRate_chargedAtk, critDmg, data.artSub);
    // var crit_normalAttack = new Crit(critRate_normalAtk, critDmg, data.artSub);
    // var crit_pluggedAttack = new Crit(critRate_pluggedAtk, critDmg_pluggedAtk, data.artSub);
    // var crit_skill = new Crit(critRate_skill, critDmg, data.artSub);



    var melt = ElementalReaction.MeltForPyro(elementalMastery, 0);
    var vaporizeForPyro = ElementalReaction.VaporizeForPyro(elementalMastery, data.er_rate());
    var vaporizeForHydro = ElementalReaction.VaporizeForHydro(elementalMastery, data.er_rate());

    var addAggravate = ElementalReaction.Aggravate(elementalMastery, data.er_aggravate());

    var enemyRES = GetElementalRes(data.memberData.res) * 0.5f;

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

    int elementalTypeCount = data.memberData.ElementalTypeCount();
    //Debug.Log(elementalTypeCount + " " + data.memberData.name);

    var expectedDmg_chargedAtk_anemo
    = GetExpectedDamage(
    atk,
    chargedAtkRate_anemo,
    dmgAdd + dmgAdd_chargedAttack,
    dmgBonus + chargedAtkDmgBonus + anemo_bonus,
    crit_ChargedAttack.ExpectedCritDmg,
    enemyRES,
    1);

    var expectedDmg_chargedAtk_pyro
    = GetExpectedDamage(
    atk,
    chargedAtkRate_otherElement,
    dmgAdd + dmgAdd_chargedAttack,
    dmgBonus + chargedAtkDmgBonus + pyro_bonus,
    crit_ChargedAttack.ExpectedCritDmg,
    enemyRES,
    1);

    var expectedDmg_chargedAtk_hydro
= GetExpectedDamage(
atk,
chargedAtkRate_otherElement,
dmgAdd + dmgAdd_chargedAttack,
dmgBonus + chargedAtkDmgBonus + hydro_bonus,
crit_ChargedAttack.ExpectedCritDmg,
enemyRES,
1);

    var expectedDmg_chargedAtk_electro
    = GetExpectedDamage(
    atk,
    chargedAtkRate_otherElement,
    dmgAdd + dmgAdd_chargedAttack,
    dmgBonus + chargedAtkDmgBonus + electro_bonus,
    crit_ChargedAttack.ExpectedCritDmg,
    enemyRES,
    1);

    var expectedDmg_chargedAtk_cryo
= GetExpectedDamage(
atk,
chargedAtkRate_otherElement,
dmgAdd + dmgAdd_chargedAttack + add_cryo,
dmgBonus + chargedAtkDmgBonus + cryo_bonus,
crit_ChargedAttack.ExpectedCritDmg,
enemyRES,
1);

    /*

     var expectedDmg_chargedAtk
     = GetExpectedDamage(
     hpSum,
     chargedAtkPer * talent_normalAtkRate,
     dmgAdd + dmgAdd_normalAttack,
     dmgBonus + normalAtkDmgBonus,
     crit_normalAttack.ExpectedCritDmg,
     enemyRES,
     1);
*/


    //        var hpSum = status.baseHp * (1 + hpPerSum) + data.hp();


    var sum = expectedDmg_chargedAtk_anemo * 3 + expectedDmg_chargedAtk_pyro + expectedDmg_chargedAtk_hydro + expectedDmg_chargedAtk_cryo;
    var crit = crit_ChargedAttack;

    Dictionary<string, string> result = new()
    {
      ["武器"] = data.weapon.name,
      ["聖遺物セット"] = data.artSetData.name,
      ["聖遺物メイン"] = data.artMainData.name,
      ["バフキャラ"] = data.memberData.name,
      ["合計期待値"] = sum.ToString(),
      ["一発目期待値"] = sum.ToString(),
      ["攻撃力"] = atk.ToString(),
      ["防御力"] = def.ToString(),
      ["HP"] = hpSum.ToString(),
      ["共通バフ"] = dmgBonus.ToString(),
      ["通常バフ"] = normalAtkDmgBonus.ToString(),
      // ["会心ダメ期待値"] = crit_ChargedAttack.ExpectedCritDmg.ToString(),
      ["熟知"] = elementalMastery.ToString(),
      ["率ダメ"] = crit.RateDmg,
      // ["会心ダメ比率"] = crit_ChargedAttack.CritProportion,
      ["聖遺物組み合わせ"] = data.artSub.name,
      ["サブステ"] = crit.SubCrit.ToString(),
      // ["サブHP%"] = data.artSub.hp_rate.ToString(),
      // ["サブHP"] = data.artSub.hp.ToString(),
      ["スコア"] = data.artSub.Score.ToString(),
    };

    //  Debug.Log(result);

    //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));

    return result;
  }
}

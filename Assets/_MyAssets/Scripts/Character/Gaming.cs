using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaming : BaseCharacter
{
  float[] pluggedAtkPerArray = { 3.91f, };

  float talent_addDmgBonusPluggedAtk = 0.2f;

  float constellation_atkRate = 0.2f;
  // float constellation_critRate = 0.2f;
  // float constellation_critDmg = 0.4f;
  float constellation_critRate = 0;
  float constellation_critDmg = 0;

  public override Dictionary<string, string> CalcDmg(Data data)
  {
    // if (data.partyData.name.Contains("ベネット")) return null;
    if (data.weapon.name.Contains("螭龍の剣") && !data.partyData.name.Contains("鍾離")) return null;
    // if (data.energy_recharge() == 0) return null;
    if (data.partyData.name.Contains("ベネット") == false && data.energy_recharge() == 0) return null;
    // if (data.partyData.name.Contains("鍾離") == false) return null;
    if (data.partyData.hydro_count == 0) return null;

    //if (data.partyData.name.Contains("フリーナ") == false) return null;
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


    float skillDmgBonus = data.skill_bonus();

    float burstDmgBonus = data.burst_bonus();

    float attackSpeed = data.atk_speed();

    float critRate = data.crit_rate();

    var critRate_normalAtk = critRate + data.crit_rate_normal_atk();
    var critRate_chargedAtk = critRate + data.crit_rate_charged_atk();
    var critRate_skill = critRate + data.crit_rate_skill();
    var ritRate_burst = critRate + data.crit_rate_burst();

    float critDmg = data.crit_dmg();


    float dmgAdd = data.add();

    var dmgAdd_sekikaku = def * data.weapon.sekikaku;


    var dmgAdd_normalAttack
    = data.add_normal_atk()
    + dmgAdd_sekikaku;

    var dmgAdd_chargedAttack
    = dmgAdd_sekikaku;




    // = getNum(weapon, "狩人ダメージアップ")
    // * elementalMastery;

    var dmgAdd_skill
    = data.add_skill()
    + def * data.weapon.cinnabar;

    //  var crit_ChargedAttack = new Crit(critRate_chargedAtk, critDmg, data.artSub);
    // var crit_normalAttack = new Crit(critRate_normalAtk, critDmg, data.artSub);
    //var crit_skill = new Crit(critRate_skill, critDmg, data.artSub);

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
    Property property = new()
    {
      data = data,
      critRate = critRate,
      critDmg = critDmg,
      atk = atk,
      dmgAdd = dmgAdd,
      dmgBonus = dmgBonus,
      res = enemyRES,
      elementalReaction = vaporize,
    };


    var (expectedDamage, crit) = ExpectedDmg_pluggedAtk(property);

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

    var sum = expectedDamage;

    Dictionary<string, string> result = new()
    {
      ["武器"] = data.weapon.name,
      ["聖遺物セット"] = data.artSetData.name,
      ["聖遺物メイン"] = data.artMainData.name,
      ["バフキャラ"] = data.partyData.name,
      ["合計期待値"] = sum.ToString(),
      ["攻撃力"] = atk.ToString(),
      ["HP"] = hpSum.ToString(),
      ["バフ"] = dmgBonus.ToString(),
      //["会心ダメ期待値"] = crit_skill.ExpectedCritDmg.ToString(),
      ["熟知"] = elementalMastery.ToString(),
      ["率ダメ"] = crit.RateDmg,
      // ["会心ダメ比率"] = crit_skill.CritProportion,
      //["聖遺物組み合わせ"] = data.artSub.name,
      ["サブステ"] = crit.SubCrit.ToString(),
      //["サブHP%"] = data.artSub.hp_rate.ToString(),
      //["サブHP"] = data.artSub.hp.ToString(),
      // ["スコア"] = data.artSub.Score.ToString()
    };

    //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));

    return result;
  }


  (float, Crit) ExpectedDmg_pluggedAtk(Property property)
  {
    var dmgAdd_pluggedAttack = property.data.add_plugged_atk();
    float pluggedAtkDmgBonus = property.data.plugged_atk_bonus() + talent_addDmgBonusPluggedAtk;
    var critRate_pluggedAtk = property.critRate + property.data.crit_rate_plugged_atk() + constellation_critRate;
    float critDmg_pluggedAtk = property.critDmg + constellation_critDmg;

    var crit_pluggedAttack = new Crit(critRate_pluggedAtk, critDmg_pluggedAtk, property.data.artSub);

    ExpectedDamage expectedDamage_pluggedAtk = new(
      property.atk,
      property.dmgAdd + dmgAdd_pluggedAttack,
      property.dmgBonus + pluggedAtkDmgBonus,
      crit_pluggedAttack.ExpectedCritDmg,
      property.res
    );

    float expectedDamage = expectedDamage_pluggedAtk.GetExpectedDamageSum(pluggedAtkPerArray, property.elementalReaction);

    return (expectedDamage, crit_pluggedAttack);

  }

  public class ExpectedDamage
  {
    readonly float atk;
    readonly float dmgAdd;
    readonly float dmgBonus;
    readonly float expectedCritDmg;
    readonly float res;

    public ExpectedDamage(float atk, float dmgAdd, float dmgBonus, float expectedCritDmg, float res)
    {
      this.atk = atk;
      this.dmgAdd = dmgAdd;
      this.dmgBonus = dmgBonus;
      this.expectedCritDmg = expectedCritDmg;
      this.res = res;
    }



    public float GetExpectedDamage(float talentRate, float elementalReaction = 1)
    {
      float dmg = (atk * talentRate + dmgAdd) * (1 + dmgBonus) * expectedCritDmg * res * elementalReaction;
      return dmg;
    }

    public float GetExpectedDamageSum(float[] talentRates, float elementalReaction = 1)
    {
      float dmg = 0;

      for (int i = 0; i < talentRates.Length; i++)
      {
        dmg += GetExpectedDamage(talentRates[i], elementalReaction);
      }
      return dmg;
    }
  }

  public class Property
  {
    public Data data;
    public float critRate;
    public float critDmg;
    public float atk;
    public float dmgAdd;
    public float dmgBonus;
    public float res;
    public float elementalReaction;
  }




}



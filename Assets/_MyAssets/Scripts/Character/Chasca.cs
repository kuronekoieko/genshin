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

    data.charged_atk_bonus += talent_chargedAtkDmgBonus;


    var (expectedDamage_charged_anemo, crit_charged) = data.ExpectedDmg(AttackType.Charged, chargedAtkRate_anemo, ElementType.Anemo);
    var (expectedDamage_charged_Pyro, crit_charged_Pyro) = data.ExpectedDmg(AttackType.Charged, chargedAtkRate_otherElement, ElementType.Pyro);
    var (expectedDamage_charged_Cryo, crit_charged_Cryo) = data.ExpectedDmg(AttackType.Charged, chargedAtkRate_otherElement, ElementType.Cryo);
    var (expectedDamage_charged_Electro, crit_charged_Electro) = data.ExpectedDmg(AttackType.Charged, chargedAtkRate_otherElement, ElementType.Electro);
    var (expectedDamage_charged_Hydro, crit_charged_Hydro) = data.ExpectedDmg(AttackType.Charged, chargedAtkRate_otherElement, ElementType.Hydro);


    var sum = expectedDamage_charged_anemo * 3 + expectedDamage_charged_Pyro + expectedDamage_charged_Hydro + expectedDamage_charged_Cryo;
    var crit = crit_charged;

    Dictionary<string, string> result = new()
    {
      ["武器"] = data.weapon.name,
      ["聖遺物セット"] = data.artSetData.name,
      ["聖遺物メイン"] = data.artMainData.name,
      ["バフキャラ"] = data.partyData.name,
      ["合計期待値"] = sum.ToString(),
      ["一発目期待値"] = sum.ToString(),
      //  ["攻撃力"] = atk.ToString(),
      // ["防御力"] = def.ToString(),
      //  ["HP"] = hpSum.ToString(),
      //["共通バフ"] = dmgBonus.ToString(),
      //["通常バフ"] = normalAtkDmgBonus.ToString(),
      // ["会心ダメ期待値"] = crit_ChargedAttack.ExpectedCritDmg.ToString(),
      ["熟知"] = data.elemental_mastery.ToString(),
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

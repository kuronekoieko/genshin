using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace so
{
  public class Chasca : BaseCharacterSO
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


      var ed_charged_anemo = ExpectedDamage.Single(data, AttackType.Charged, chargedAtkRate_anemo, ElementType.Anemo);
      var ed_charged_Pyro = ExpectedDamage.Single(data, AttackType.Charged, chargedAtkRate_otherElement, ElementType.Pyro);
      var ed_charged_Cryo = ExpectedDamage.Single(data, AttackType.Charged, chargedAtkRate_otherElement, ElementType.Cryo);
      var ed_charged_Electro = ExpectedDamage.Single(data, AttackType.Charged, chargedAtkRate_otherElement, ElementType.Electro);
      var ed_charged_Hydro = ExpectedDamage.Single(data, AttackType.Charged, chargedAtkRate_otherElement, ElementType.Hydro);


      var sum = ed_charged_anemo.Result * 3 + ed_charged_Pyro.Result + ed_charged_Hydro.Result + ed_charged_Cryo.Result;
      var crit = ed_charged_anemo.Crit;

      Dictionary<string, string> result = new()
      {
        ["武器"] = data.weapon.DisplayName,
        ["聖遺物セット"] = data.artSetData.name,
        ["聖遺物メイン"] = data.artMainData.name,
        ["バフキャラ"] = data.partyData.name,
        ["合計期待値"] = sum.ToIntString(),
        ["一発目期待値"] = sum.ToIntString(),
        //  ["攻撃力"] = atk.ToIntString(),
        // ["防御力"] = def.ToIntString(),
        //  ["HP"] = hpSum.ToIntString(),
        //["共通バフ"] = dmgBonus.ToIntString(),
        //["通常バフ"] = normalAtkDmgBonus.ToIntString(),
        // ["会心ダメ期待値"] = crit_ChargedAttack.ExpectedCritDmg.ToIntString(),
        ["熟知"] = data.elemental_mastery.ToIntString(),
        ["率ダメ"] = crit.RateDmg,
        // ["会心ダメ比率"] = crit_ChargedAttack.CritProportion,
        ["聖遺物組み合わせ"] = data.artSubData.name,
        ["サブステ"] = crit.SubCrit,
        // ["サブHP%"] = data.artSubData.hp_rate.ToIntString(),
        // ["サブHP"] = data.artSubData.hp.ToIntString(),
        ["スコア"] = data.artSubData.Score.ToIntString(),
      };

      //  Debug.Log(result);

      //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));

      return result;
    }
  }
}
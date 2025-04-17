using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Neuvillette", menuName = "Scriptable Objects/" + "Neuvillette")]
public class NeuvilletteSO : BaseCharacterSO
{
  // lv9
  readonly static float chargedAtkPer = 0.1447f;
  // readonly float[] normalAtkPerArray = { normalAtkPer + const_addAtkPerHp, normalAtkPer, normalAtkPer };

  readonly static float[] talent_chargedAtkRateAry = { 1f, 1.1f, 1.25f, 1.6f };
  readonly static float talent_HydroDmgBonus = 0.3f;




  public override Dictionary<string, string> CalcDmg(Data data)
  {

    data.hydro_bonus += talent_HydroDmgBonus;

    int elementalTypeCount = data.partyData.ElementalTypeCount();
    //Debug.Log(elementalTypeCount + " " + data.partyData.name);
    float talent_chargedAtkRate = talent_chargedAtkRateAry[elementalTypeCount - 1];



    // var ed_normal = ExpectedDamage.Single(data, AttackType.Normal,  normalAtkPerArray);
    var ed_charged = ExpectedDamage.Single(data, AttackType.Charged, chargedAtkPer * talent_chargedAtkRate, referenceStatus: ReferenceStatus.Hp);
    // var ed_plugged = ExpectedDamage.Single(data, AttackType.Plugged,pluggedAtkPerArray);
    // var expectedDamage_skill = ExpectedDamage.Single(data, AttackType.Skill, skillPerArray);
    // var (expectedDamage_burst, crit_burst) = ExpectedDamage.Single(data, AttackType.Skill, null);



    float sum = ed_charged.Result;
    var crit = ed_charged.Crit;

    Dictionary<string, string> result = new()
    {
      ["武器"] = data.weapon.DisplayName,
      ["聖遺物セット"] = data.artSetData.name,
      ["聖遺物メイン"] = data.artMainData.name,
      ["バフキャラ"] = data.partyData.name,
      ["合計期待値"] = sum.ToString(),
      ["一発目期待値"] = ed_charged.Result.ToString(),
      // ["攻撃力"] = atk.ToString(),
      // ["防御力"] = def.ToString(),
      ["HP合計"] = data.hp.ToString(),
      ["HP%合計"] = data.hp_rate.ToString(),
      ["HP%パーティ"] = data.partyData.hp_rate.ToString(),
      // ["共通バフ"] = dmgBonus.ToString(),
      //["通常バフ"] = normalAtkDmgBonus.ToString(),
      // ["会心ダメ期待値"] = crit_ChargedAttack.ExpectedCritDmg.ToString(),
      // ["熟知"] = elementalMastery.ToString(),
      ["率ダメ"] = crit.RateDmg,
      // ["会心ダメ比率"] = crit_ChargedAttack.CritProportion,
      ["聖遺物組み合わせ"] = data.artSubData.name,
      ["サブステ"] = crit.SubCrit,
      // ["サブHP%"] = data.artSubData.hp_rate.ToString(),
      // ["サブHP"] = data.artSubData.hp.ToString(),
      ["スコア"] = data.artSubData.Score.ToString(),
    };

    //  Debug.Log(result);

    //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));

    return result;
  }
}

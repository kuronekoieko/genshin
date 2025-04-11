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

    CharaData charaData = new(data);
    charaData.hydro_bonus += talent_HydroDmgBonus;

    int elementalTypeCount = data.partyData.ElementalTypeCount();
    //Debug.Log(elementalTypeCount + " " + data.partyData.name);
    float talent_chargedAtkRate = talent_chargedAtkRateAry[elementalTypeCount - 1];



    // var (expectedDamage_normal, crit_normal) = charaData.ExpectedDmg(AttackType.Normal,  normalAtkPerArray);
    var (expectedDamage_charged, crit_charged) = charaData.ExpectedDmg(AttackType.Charged, chargedAtkPer * talent_chargedAtkRate, referenceStatus: ReferenceStatus.Hp);
    // var (expectedDamage_plugged, crit_plugged) = charaData.ExpectedDmg(AttackType.Plugged,pluggedAtkPerArray);
    // var (expectedDamage_skill, crit_skill) = charaData.ExpectedDmg(AttackType.Skill, skillPerArray);
    // var (expectedDamage_burst, crit_burst) = charaData.ExpectedDmg(AttackType.Skill, null);



    var sum = expectedDamage_charged;
    var crit = crit_charged;

    Dictionary<string, string> result = new()
    {
      ["武器"] = data.weapon.name,
      ["聖遺物セット"] = data.artSetData.name,
      ["聖遺物メイン"] = data.artMainData.name,
      ["バフキャラ"] = data.partyData.name,
      ["合計期待値"] = sum.ToString(),
      ["一発目期待値"] = expectedDamage_charged.ToString(),
      // ["攻撃力"] = atk.ToString(),
      // ["防御力"] = def.ToString(),
      //["HP"] = hpSum.ToString(),
      // ["共通バフ"] = dmgBonus.ToString(),
      //["通常バフ"] = normalAtkDmgBonus.ToString(),
      // ["会心ダメ期待値"] = crit_ChargedAttack.ExpectedCritDmg.ToString(),
      // ["熟知"] = elementalMastery.ToString(),
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

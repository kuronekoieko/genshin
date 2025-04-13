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



        var vaporizeForHydro = ElementalReaction.VaporizeForHydro(data.elemental_mastery, data.er_rate);

        var (expectedDamage_normal_1, crit_normal_1) = data.ExpectedDmg(AttackType.Normal, normalAtkPerArray[0], referenceStatus: ReferenceStatus.Hp, er_multi: vaporizeForHydro);

        // var (expectedDamage_normal, crit_normal) = ExpectedDmg_multi(AttackType.Normal,  hpRate: normalAtkPerArray, elementalReaction: vaporizeForHydro);
        //  var (expectedDamage_charged, crit_charged) = data.ExpectedDmg(AttackType.Charged,  chargedAtkPerArray);
        //var (expectedDamage_plugged, crit_plugged) = data.ExpectedDmg(AttackType.Plugged,pluggedAtkPerArray);
        //var (expectedDamage_skill, crit_skill) = data.ExpectedDmg(AttackType.Skill, skillPerArray);
        //var (expectedDamage_burst, crit_burst) = data.ExpectedDmg(AttackType.Skill, null);





        var sum = expectedDamage_normal_1;

        Dictionary<string, string> result = new()
        {
            ["武器"] = data.weapon.name,
            ["聖遺物セット"] = data.artSetData.name,
            ["聖遺物メイン"] = data.artMainData.name,
            ["バフキャラ"] = data.partyData.name,
            ["合計期待値"] = sum.ToString(),
            ["一発目期待値"] = expectedDamage_normal_1.ToString(),
            //["攻撃力"] = atk.ToString(),
            // ["防御力"] = def.ToString(),
            //["HP"] = hpSum.ToString(),
            //["共通バフ"] = dmgBonus.ToString(),
            // ["通常バフ"] = normalAtkDmgBonus.ToString(),
            // ["会心ダメ期待値"] = crit_ChargedAttack.ExpectedCritDmg.ToString(),
            //  ["熟知"] = elementalMastery.ToString(),
            // ["率ダメ"] = crit_normalAttack.RateDmg,
            // ["会心ダメ比率"] = crit_ChargedAttack.CritProportion,
            ["聖遺物組み合わせ"] = data.artSub.name,
            // ["サブステ"] = crit_normalAttack.SubCrit.ToString(),
            // ["サブHP%"] = data.artSub.hp_rate.ToString(),
            // ["サブHP"] = data.artSub.hp.ToString(),
            ["スコア"] = data.artSub.Score.ToString(),
        };

        //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));

        return result;
    }
}

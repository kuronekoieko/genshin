using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Varesa : BaseCharacter
{
    // readonly float[] normalAtkPerArray = { 0, };
    // readonly float[] chargedAtkPerArray = { 0, };
    //readonly float[] pluggedAtkPerArray = { 3.91f, };
    // lv8
    readonly float[] pluggedAtkPerArray = { 5.52f, };

    // readonly float[] skillPerArray = { 0, };
    //readonly float[] burstPerArray = { 0, };

    // float talent_addPerPluggedAtk_1 = 0.5f;
    float talent_addPerPluggedAtk_2 = 1.8f;


    // float constellation_atkRate = 0.2f;
    // float constellation_critRate = 0.2f;
    // float constellation_critDmg = 0.4f;
    // float constellation_critRate = 0;
    //float constellation_critDmg = 0;

    public override Dictionary<string, string> CalcDmg(Data data)
    {

        //if (data.partyData.name.Contains("閑雲")) return null;
        // if (data.partyData.name.Contains("ベネット")) return null;
        //if (data.weapon.name.Contains("螭龍の剣(完凸)") && !data.partyData.name.Contains("鍾離")) return null;
        // if (data.energy_recharge() == 0) return null;
        //if (data.partyData.name.Contains("ベネット") == false && data.energy_recharge() == 0) return null;
        // if (data.partyData.name.Contains("鍾離") == false) return null;
        //if (data.partyData.hydro_count == 0) return null;

        //if (data.partyData.name.Contains("フリーナ") == false) return null;
        // if (data.energy_recharge() < 0.5f) return null;
        // if (data.weapon.name != "草薙の稲光") return null;
        // if (data.weapon.name != "和璞鳶") return null;

        CharaData charaData = GetCharaData(data);


        // var melt = ElementalReaction.MeltForPyro(charaData.elemental_mastery, 0);
        //  var vaporize = ElementalReaction.VaporizeForPyro(charaData.elemental_mastery, data.er_rate());
        var addAggravate = ElementalReaction.Aggravate(charaData.elemental_mastery, data.er_aggravate());

        float elementalReaction = 1;
        if (data.partyData.dendro_count > 0)
        {
            elementalReaction = addAggravate;
        }
        // var (expectedDamage, crit) = ExpectedDmg_normalAtk(property);
        // var (expectedDamage, crit) = ExpectedDmg_chargedAtk(property);
        var (expectedDamage, crit) = ExpectedDmg(AttackType.Plugged, charaData, data, elementalReaction);

        //  var (expectedDamage, crit) = ExpectedDmg_skill(property);
        // var (expectedDamage, crit) = ExpectedDmg_burst(property);


        var sum = Mathf.FloorToInt(expectedDamage);

        Dictionary<string, string> result = new()
        {
            ["武器"] = data.weapon.name,
            ["聖遺物セット"] = data.artSetData.name,
            ["聖遺物メイン"] = data.artMainData.name,
            ["バフキャラ"] = data.partyData.name,
            ["合計期待値"] = sum.ToString(),
            ["攻撃力"] = charaData.atk.ToString(),
            ["倍率"] = (pluggedAtkPerArray[0] + talent_addPerPluggedAtk_2).ToString(),
            // ["HP"] = charaData.hp.ToString(),
            ["加算"] = (charaData.add + charaData.add_plugged_atk).ToString(),
            ["バフ合計"] = (charaData.dmg_bonus + charaData.plugged_atk_bonus).ToString(),
            ["バフ共通"] = charaData.dmg_bonus.ToString(),
            ["バフ落下"] = charaData.plugged_atk_bonus.ToString(),
            ["耐性ダウン"] = charaData.res.ToString(),
            // ["耐性ダウン計算後"] = (GetElementalRes(charaData.res) * 0.5f).ToString(),
            //["会心ダメ期待値"] = crit.ExpectedCritDmg.ToString(),
            // ["耐性"] = charaData.res.ToString(),
            // ["蒸発"] = vaporize.ToString(),
            ["熟知"] = charaData.elemental_mastery.ToString(),
            ["率ダメ"] = crit.RateDmg,
            // ["会心ダメ比率"] = crit_skill.CritProportion,
            //["聖遺物組み合わせ"] = data.artSub.name,
            ["サブステ"] = crit.SubCrit.ToString(),
            //["サブHP%"] = data.artSub.hp_rate.ToString(),
            //["サブHP"] = data.artSub.hp.ToString(),
            ["スコア"] = data.artSub.Score.ToString()
        };

        //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));

        return result;
    }


    (float, Crit) ExpectedDmg(AttackType attackType, CharaData charaData, Data data, float elementalReaction)
    {

        ExpectedDamage expectedDamage = new(attackType, charaData, data.artSub);

        float result = expectedDamage.GetExpectedDamageSum(pluggedAtkPerArray, elementalReaction, talent_addPerPluggedAtk_2);

        return (result, expectedDamage.Crit);

    }
}


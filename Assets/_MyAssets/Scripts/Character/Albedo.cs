using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Albedo : BaseCharacter
{
    // スキル Lv9
    float[] skillPerArray = { 2.27f };

    // 固有天賦
    //  float passive_dmgBonusPerEM = 0.15f * 0.01f;


    public override Dictionary<string, string> CalcDmg(Data data)
    {
        CharaData charaData = GetCharaData(data);
        // var (expectedDamage_normal, crit_normal) = ExpectedDmg(AttackType.Normal, charaData, data, normalAtkPerArray);
        // var (expectedDamage_charged, crit_charged) = ExpectedDmg(AttackType.Charged, charaData, data, chargedAtkPerArray);
        //var (expectedDamage_plugged, crit_plugged) = ExpectedDmg(AttackType.Plugged, charaData, data, pluggedAtkPerArray);
        var (expectedDamage_skill, crit_skill) = ExpectedDmg(AttackType.Skill, charaData, data, skillPerArray);
        // var (expectedDamage_burst, crit_burst) = ExpectedDmg(AttackType.Skill, charaData, data, null);

        //  var melt = ElementalReaction.MeltForPyro(elementalMastery, 0);
        // var vaporize = ElementalReaction.VaporizeForPyro(elementalMastery, data.artSetData.er_rate);

        // var addAggravate = ElementalReaction.Aggravate(elementalMastery, data.artSetData.er_aggravate);



        Dictionary<string, string> result = new()
        {
            ["武器"] = data.weapon.name,
            ["聖遺物セット"] = data.artSetData.name,
            ["聖遺物メイン"] = data.artMainData.name,
            ["バフキャラ"] = data.partyData.name,
            ["スキル期待値"] = expectedDamage_skill.ToString(),
            ["攻撃力"] = charaData.atk.ToString(),
            ["HP"] = charaData.hp.ToString(),
            ["バフ"] = charaData.dmg_bonus.ToString(),
            ["会心ダメ期待値"] = crit_skill.ExpectedCritDmg.ToString(),
            ["熟知"] = charaData.elemental_mastery.ToString(),
            ["率ダメ"] = crit_skill.RateDmg,
            ["会心ダメ比率"] = crit_skill.CritProportion,
            ["聖遺物組み合わせ"] = data.artSub.name,
            ["サブステ"] = crit_skill.SubCrit.ToString(),
            ["サブHP%"] = data.artSub.hp_rate.ToString(),
            ["サブHP"] = data.artSub.hp.ToString(),
            ["スコア"] = data.artSub.Score.ToString()
        };

        //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));

        return result;
    }
}

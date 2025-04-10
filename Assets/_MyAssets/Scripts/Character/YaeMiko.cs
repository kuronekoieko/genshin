using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class YaeMiko : BaseCharacter
{
    // スキル Lv9
    float[] skillPerArray = { 1.031f, 1.289f, 1.612f };

    // 固有天賦
    float passive_dmgBonusPerEM = 0.15f * 0.01f;


    public override Dictionary<string, string> CalcDmg(Data data)
    {
        CharaData charaData = GetCharaData(data);
        charaData.skill_bonus += passive_dmgBonusPerEM * charaData.elemental_mastery;

        var addAggravate = ElementalReaction.Aggravate(charaData.elemental_mastery, data.artSetData.er_aggravate);

        // var (expectedDamage_normal, crit_normal) = ExpectedDmg(AttackType.Normal, charaData, data, status.elementType,normalAtkPerArray);
        //var (expectedDamage_charged, crit_charged) = ExpectedDmg(AttackType.Charged, charaData, data, status.elementType,chargedAtkPerArray);
        //var (expectedDamage_plugged, crit_plugged) = ExpectedDmg(AttackType.Plugged, charaData, data, status.elementType,pluggedAtkPerArray);
        var (expectedDamage_skill_0, crit_skill_0) = ExpectedDmg(AttackType.Skill, charaData, data, skillPerArray[0], er_add: addAggravate);
        var (expectedDamage_skill_1, crit_skill_1) = ExpectedDmg(AttackType.Skill, charaData, data, skillPerArray[1]);
        var (expectedDamage_skill_2, crit_skill_2) = ExpectedDmg(AttackType.Skill, charaData, data, skillPerArray[2]);

        //var (expectedDamage_burst, crit_burst) = ExpectedDmg(AttackType.Skill, charaData, data, status.elementType,null);






        var expectedDmg_gekika = expectedDamage_skill_0 + expectedDamage_skill_1 + expectedDamage_skill_2;



        Dictionary<string, string> result = new()
        {
            ["武器"] = data.weapon.name,
            ["聖遺物セット"] = data.artSetData.name,
            ["聖遺物メイン"] = data.artMainData.name,
            ["バフキャラ"] = data.partyData.name,
            ["スキル期待値"] = expectedDmg_gekika.ToString(),
            //  ["攻撃力"] = atk.ToString(),
            //["HP"] = hpSum.ToString(),
            // ["バフ"] = dmgBonus.ToString(),
            // ["会心ダメ期待値"] = crit_skill.ExpectedCritDmg.ToString(),
            // ["熟知"] = elementalMastery.ToString(),
            // ["率ダメ"] = crit_skill.RateDmg,
            // ["会心ダメ比率"] = crit_skill.CritProportion,
            ["聖遺物組み合わせ"] = data.artSub.name,
            // ["サブステ"] = crit_skill.SubCrit.ToString(),
            ["サブHP%"] = data.artSub.hp_rate.ToString(),
            ["サブHP"] = data.artSub.hp.ToString(),
            ["スコア"] = data.artSub.Score.ToString()
        };

        //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));

        return result;
    }



}

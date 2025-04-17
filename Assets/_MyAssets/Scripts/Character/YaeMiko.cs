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

        data.skill_bonus += passive_dmgBonusPerEM * data.elemental_mastery;

        ElementalReaction elementalReaction = new(ElementType.Electro, ElementType.Dendro, data);

        // var addAggravate = ElementalReaction.Aggravate(data.elemental_mastery, data.artSetData.er_aggravate);

        // var ed_normal = ExpectedDamage.Single(data, AttackType.Normal,  status.elementType,normalAtkPerArray);
        //var ed_charged = ExpectedDamage.Single(data, AttackType.Charged,  status.elementType,chargedAtkPerArray);
        //var ed_plugged = ExpectedDamage.Single(data, AttackType.Plugged,status.elementType,pluggedAtkPerArray);
        var ed_skill_0 = ExpectedDamage.Single(data, AttackType.Skill, skillPerArray[0], elementalReaction: elementalReaction);
        var ed_skill_1 = ExpectedDamage.Single(data, AttackType.Skill, skillPerArray[1]);
        var ed_skill_2 = ExpectedDamage.Single(data, AttackType.Skill, skillPerArray[2]);

        //var (expectedDamage_burst, crit_burst) = ExpectedDamage.Single(data, AttackType.Skill, status.elementType,null);


        var expectedDmg_gekika = ed_skill_0.Result + ed_skill_1.Result + ed_skill_2.Result;



        Dictionary<string, string> result = new()
        {
            ["武器"] = data.weapon.DisplayName,
            ["聖遺物セット"] = data.artSetData.name,
            ["聖遺物メイン"] = data.artMainData.name,
            ["バフキャラ"] = data.partyData.name,
            ["スキル期待値"] = expectedDmg_gekika.ToString(),
            //  ["攻撃力"] = atk.ToString(),
            //["HP"] = hpSum.ToString(),
            // ["バフ"] = dmgBonus.ToString(),
            // ["会心ダメ期待値"] = expectedDamage_skill.Crit.ExpectedCritDmg.ToString(),
            // ["熟知"] = elementalMastery.ToString(),
            // ["率ダメ"] = expectedDamage_skill.Crit.RateDmg,
            // ["会心ダメ比率"] = expectedDamage_skill.Crit.CritProportion,
            ["聖遺物組み合わせ"] = data.artSubData.name,
            // ["サブステ"] = expectedDamage_skill.Crit.SubCrit.ToString(),
            ["サブHP%"] = data.artSubData.hp_rate.ToString(),
            ["サブHP"] = data.artSubData.hp.ToString(),
            ["スコア"] = data.artSubData.Score.ToString()
        };

        //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));

        return result;
    }



}

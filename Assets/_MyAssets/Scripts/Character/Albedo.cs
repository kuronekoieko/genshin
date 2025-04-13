using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Albedo : BaseCharacter
{
    // スキル Lv9
    float[] skillPerArray = { 2.27f };


    public override Dictionary<string, string> CalcDmg(Data data)
    {

        var expectedDamage_skill = ExpectedDamage.Single(data, AttackType.Skill, skillPerArray[0], referenceStatus: ReferenceStatus.Def);

        Dictionary<string, string> result = new()
        {
            ["武器"] = data.weapon.name,
            ["聖遺物セット"] = data.artSetData.name,
            ["聖遺物メイン"] = data.artMainData.name,
            ["バフキャラ"] = data.partyData.name,
            ["スキル期待値"] = expectedDamage_skill.ToString(),
            ["攻撃力"] = data.atk.ToString(),
            ["HP"] = data.hp.ToString(),
            ["バフ"] = data.dmg_bonus.ToString(),
            ["会心ダメ期待値"] = expectedDamage_skill.Crit.ExpectedCritDmg.ToString(),
            ["熟知"] = data.elemental_mastery.ToString(),
            ["率ダメ"] = expectedDamage_skill.Crit.RateDmg,
            ["会心ダメ比率"] = expectedDamage_skill.Crit.CritProportion,
            ["聖遺物組み合わせ"] = data.artSubData.name,
            ["サブステ"] = expectedDamage_skill.Crit.SubCrit.ToString(),
            ["サブHP%"] = data.artSubData.hp_rate.ToString(),
            ["サブHP"] = data.artSubData.hp.ToString(),
            ["スコア"] = data.artSubData.Score.ToString()
        };

        //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));

        return result;
    }
}

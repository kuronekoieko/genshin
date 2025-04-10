using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Albedo : BaseCharacter
{
    // スキル Lv9
    float[] skillPerArray = { 2.27f };


    public override Dictionary<string, string> CalcDmg(Data data)
    {
        CharaData charaData = new(data);

        var (expectedDamage_skill, crit_skill) = charaData.ExpectedDmg(AttackType.Skill, skillPerArray[0], referenceStatus: ReferenceStatus.Def);


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

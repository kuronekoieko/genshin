using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chiori : BaseCharacter
{
    // スキル Lv9
    float[] normalAtkPerArray_atk = { 0.908f, 0.860f, 0.559f, 0.559f, 1.38f };

    float skillPerAtk = 1.40f;
    float skillPerDef = 1.74f;

    float constellation_addNormalAtkPerDef = 2.35f;


    public override Dictionary<string, string> CalcDmg(Data data)
    {



        var ed_normal = ExpectedDamage.Single(data, AttackType.Normal, constellation_addNormalAtkPerDef, referenceStatus: ReferenceStatus.Def);
        var expectedDamage_skill = ExpectedDamage.Single(data, AttackType.Skill, skillPerDef, referenceStatus: ReferenceStatus.Def);

        Dictionary<string, string> result = new()
        {
            ["武器"] = data.weapon.name,
            ["聖遺物セット"] = data.artSetData.name,
            ["聖遺物メイン"] = data.artMainData.name,
            ["バフキャラ"] = data.partyData.name,
            ["通常期待値"] = ed_normal.Result.ToString(),
            ["スキル期待値"] = expectedDamage_skill.ToString(),
            // ["攻撃力"] = atk.ToString(),
            // ["防御力"] = def.ToString(),
            //  ["HP"] = hpSum.ToString(),
            // ["バフ"] = dmgBonus.ToString(),
            ["会心ダメ期待値"] = ed_normal.Crit.ExpectedCritDmg.ToString(),
            // ["熟知"] = elementalMastery.ToString(),
            ["率ダメ"] = ed_normal.Crit.RateDmg,
            ["会心ダメ比率"] = ed_normal.Crit.CritProportion,
            ["聖遺物組み合わせ"] = data.artSubData.name,
            ["サブステ"] = ed_normal.Crit.SubCrit.ToString(),
            ["サブHP%"] = data.artSubData.hp_rate.ToString(),
            ["サブHP"] = data.artSubData.hp.ToString(),
            ["スコア"] = data.artSubData.Score.ToString(),
        };

        //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));

        return result;
    }
}
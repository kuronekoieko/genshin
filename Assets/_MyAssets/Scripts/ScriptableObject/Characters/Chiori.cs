using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Chiori", menuName = "Scriptable Objects/" + "Chiori")]
public class Chiori : BaseCharacterSO
{
    // スキル Lv9
    float[] normalAtkPerArray_atk = { 0.908f, 0.860f, 0.559f, 0.559f, 1.38f };

    float skillPerAtk = 1.40f;
    float skillPerDef = 1.74f;

    float constellation_addNormalAtkPerDef = 2.35f;


    public override Dictionary<string, string> CalcDmg(Data data)
    {
        float[] normalAtkPerArray_def = { constellation_addNormalAtkPerDef, constellation_addNormalAtkPerDef, constellation_addNormalAtkPerDef, constellation_addNormalAtkPerDef, constellation_addNormalAtkPerDef };


        var ed_normal_atk = ExpectedDamage.Sum(data, AttackType.Normal, normalAtkPerArray_atk, referenceStatus: ReferenceStatus.Atk);
        var ed_normal_def = ExpectedDamage.Sum(data, AttackType.Normal, normalAtkPerArray_def, referenceStatus: ReferenceStatus.Def);
        var ed_skill_atk = ExpectedDamage.Single(data, AttackType.Skill, skillPerAtk, referenceStatus: ReferenceStatus.Atk);
        var ed_skill_def = ExpectedDamage.Single(data, AttackType.Skill, skillPerDef, referenceStatus: ReferenceStatus.Def);

        float sum_normal = ed_normal_atk.Result + ed_normal_def.Result;
        float sum_skill = ed_skill_atk.Result + ed_skill_def.Result;

        var ed = ed_normal_def;

        Dictionary<string, string> result = new()
        {
            ["武器"] = data.weapon.DisplayName,
            ["聖遺物セット"] = data.artSetData.name,
            ["聖遺物メイン"] = data.artMainData.name,
            ["バフキャラ"] = data.partyData.name,
            ["通常期待値"] = sum_normal.ToIntString(),
            ["スキル期待値"] = sum_skill.ToIntString(),
            ["攻撃力"] = data.atk.ToIntString(),
            ["防御力"] = data.def.ToIntString(),
            //  ["HP"] = hpSum.ToIntString(),
            ["バフ"] = ed.DmgBonus.ToIntString(),
            ["会心ダメ期待値"] = ed.Crit.ExpectedCritDmg.ToIntString(),
            // ["熟知"] = elementalMastery.ToIntString(),
            ["率ダメ"] = ed.Crit.RateDmg,
            ["会心ダメ比率"] = ed.Crit.CritProportion,
            ["聖遺物組み合わせ"] = data.artSubData.name,
            ["サブステ"] = ed.Crit.SubCrit,
            ["サブHP%"] = data.artSubData.hp_rate.ToIntString(),
            ["サブHP"] = data.artSubData.hp.ToIntString(),
            ["スコア"] = data.artSubData.Score.ToIntString(),
        };

        //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));

        return result;
    }
}

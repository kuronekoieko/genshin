using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arataki : BaseCharacter
{
    // スキル Lv9
    float[] normalAtkPerArray = { 1.456f, 1.403f, 1.684f, 2.154f, };
    float[] chargedAtkPerArray = { 1.675f, 1.675f, 1.675f, 1.675f, 3.508f, };
    //float[] chargedAtkPerArray = { 1.675f };

    float burst_addAtkPerDef = 0.979f;
    float talent_addDmg_chargedAtk_PerDef = 0.35f;


    public override Dictionary<string, string> CalcDmg(Data data)
    {
        // 時計原チャじゃないと、かなりdps落ちる
        if (data.energy_recharge < 0.5f) return null;


        data.atk += burst_addAtkPerDef * data.def;
        data.add_charged_atk += talent_addDmg_chargedAtk_PerDef * data.def;


        var ed_normal = ExpectedDamage.Sum(data, AttackType.Normal, normalAtkPerArray);
        var ed_charged = ExpectedDamage.Sum(data, AttackType.Charged, chargedAtkPerArray);

        var sum = ed_normal.Result + ed_charged.Result;

        Dictionary<string, string> result = new()
        {
            ["武器"] = data.weapon.name,
            ["聖遺物セット"] = data.artSetData.name,
            ["聖遺物メイン"] = data.artMainData.name,
            ["バフキャラ"] = data.partyData.name,
            ["合計期待値"] = sum.ToString(),
            ["攻撃力"] = data.atk.ToString(),
            ["HP"] = data.hp.ToString(),
            //  ["バフ"] = dmgBonus.ToString(),
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

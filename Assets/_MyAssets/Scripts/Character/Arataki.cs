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
        if (data.energy_recharge() < 0.5f) return null;

        CharaData charaData = GetCharaData(data);
        var (expectedDamage_normal, crit_normal) = ExpectedDmg(AttackType.Normal, charaData, data, normalAtkPerArray);
        var (expectedDamage_charged, crit_charged) = ExpectedDmg(AttackType.Charged, charaData, data, chargedAtkPerArray);
        //var (expectedDamage_plugged, crit_plugged) = ExpectedDmg(AttackType.Plugged, charaData, data, pluggedAtkPerArray);
        var (expectedDamage_skill, crit_skill) = ExpectedDmg(AttackType.Skill, charaData, data, null);
        var (expectedDamage_burst, crit_burst) = ExpectedDmg(AttackType.Skill, charaData, data, null);

        var sum = expectedDamage_normal + expectedDamage_charged;

        Dictionary<string, string> result = new()
        {
            ["武器"] = data.weapon.name,
            ["聖遺物セット"] = data.artSetData.name,
            ["聖遺物メイン"] = data.artMainData.name,
            ["バフキャラ"] = data.partyData.name,
            ["合計期待値"] = sum.ToString(),
            ["攻撃力"] = charaData.atk.ToString(),
            ["HP"] = charaData.hp.ToString(),
            //  ["バフ"] = dmgBonus.ToString(),
            ["会心ダメ期待値"] = crit_skill.ExpectedCritDmg.ToString(),
            // ["熟知"] = elementalMastery.ToString(),
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

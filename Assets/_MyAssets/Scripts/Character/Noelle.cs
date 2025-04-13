using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noelle : BaseCharacter
{
    // スキル Lv9
    float[] normalAtkPerArray = { 1.45f, 1.35f, 1.58f, 2.08f, };

    float[] pluggedAtkPerArray = { 2.74f };

    float burst_addAtkPerDef = 0.68f;
    float constellation_addAtkPerDef = 0.5f;


    public override Dictionary<string, string> CalcDmg(Data data)
    {

        data.atk += (burst_addAtkPerDef + constellation_addAtkPerDef) * data.def;

        var (expectedDamage_normal, crit_normal) = data.ExpectedDmg(AttackType.Normal, atkRate: normalAtkPerArray[0]);
        var (expectedDamage_plugged, crit_plugged) = data.ExpectedDmg(AttackType.Plugged, atkRate: pluggedAtkPerArray[0]);


        Dictionary<string, string> result = new()
        {
            ["武器"] = data.weapon.name,
            ["聖遺物セット"] = data.artSetData.name,
            ["聖遺物メイン"] = data.artMainData.name,
            ["バフキャラ"] = data.partyData.name,
            ["合計期待値"] = expectedDamage_plugged.ToString(),
            ["攻撃力"] = data.atk.ToString(),
            ["HP"] = data.hp.ToString(),
            ["バフ"] = data.dmg_bonus.ToString(),
            ["会心ダメ期待値"] = crit_plugged.ExpectedCritDmg.ToString(),
            ["熟知"] = data.elemental_mastery.ToString(),
            ["率ダメ"] = crit_plugged.RateDmg,
            ["会心ダメ比率"] = crit_plugged.CritProportion,
            ["聖遺物組み合わせ"] = data.artSub.name,
            ["サブステ"] = crit_plugged.SubCrit.ToString(),
            ["サブHP%"] = data.artSub.hp_rate.ToString(),
            ["サブHP"] = data.artSub.hp.ToString(),
            ["スコア"] = data.artSub.Score.ToString()
        };

        //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));

        return result;
    }



}

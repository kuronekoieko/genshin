using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gaming", menuName = "Scriptable Objects/Gaming")]
public class GamingSO : BaseCharacterSO
{
    readonly float[] pluggedAtkPerArray = { 3.686f, };

    // readonly float[] skillPerArray = { 0, };
    // readonly float[] burstPerArray = { 0, };

    float talent_addDmgBonusPluggedAtk = 0.2f;

    float constellation_atkRate = 0.2f;
    float constellation_critRate = 0.2f;
    float constellation_critDmg = 0.4f;
    // float constellation_critRate = 0;
    //float constellation_critDmg = 0;

    public override Dictionary<string, string> CalcDmg(Data data)
    {/*
        if (!data.artSetData.name.Contains("ファントム") &&
          !data.artSetData.name.Contains("火魔女") &&
          !data.artSetData.name.Contains("金メッキ") &&
          !data.artSetData.name.Contains("辰砂")
          ) return null;
        //if (data.partyData.name.Contains("閑雲")) return null;
        // if (data.partyData.name.Contains("ベネット")) return null;
        if (data.weapon.name.Contains("螭龍の剣(完凸)") && !data.partyData.name.Contains("鍾離")) return null;
        // if (data.energy_recharge() == 0) return null;
        //if (data.partyData.name.Contains("ベネット") == false && data.energy_recharge() == 0) return null;
        // if (data.partyData.name.Contains("鍾離") == false) return null;
        if (data.partyData.hydro_count == 0) return null;

        //if (data.partyData.name.Contains("フリーナ") == false) return null;
        // if (data.energy_recharge() < 0.5f) return null;
*/
        Data charaData = data;
        charaData.atk += data.BaseAtk * constellation_atkRate;


        charaData.crit_rate_plugged_atk += constellation_critRate;
        charaData.crit_dmg_plugged += constellation_critDmg;
        charaData.crit_dmg_plugged += talent_addDmgBonusPluggedAtk;

        var vaporize = ElementalReaction.VaporizeForPyro(charaData.elemental_mastery, data.er_rate);

        var (expectedDamage, crit) = charaData.ExpectedDmg(AttackType.Plugged, pluggedAtkPerArray[0], er_multi: vaporize);

        var sum = expectedDamage;

        Dictionary<string, string> result = new()
        {
            ["武器"] = data.weapon.name,
            ["聖遺物セット"] = data.artSetData.name,
            ["聖遺物メイン"] = data.artMainData.name,
            ["バフキャラ"] = data.partyData.name,
            ["合計期待値"] = sum.ToString(),
            ["攻撃力"] = charaData.atk.ToString(),
            // ["HP"] = baseData.hp.ToString(),
            // ["加算"] = (baseData.add + data.add_plugged_atk()).ToString(),
            //["バフ"] = (baseData.dmg_bonus + data.plugged_atk_bonus() + talent_addDmgBonusPluggedAtk).ToString(),
            //["会心ダメ期待値"] = crit.ExpectedCritDmg.ToString(),
            // ["耐性"] = baseData.res.ToString(),
            // ["蒸発"] = vaporize.ToString(),
            ["熟知"] = charaData.elemental_mastery.ToString(),
            ["率ダメ"] = crit.RateDmg,
            // ["会心ダメ比率"] = crit_skill.CritProportion,
            //["聖遺物組み合わせ"] = data.artSub.name,
            ["サブステ"] = crit.SubCrit.ToString(),
            //["サブHP%"] = data.artSub.hp_rate.ToString(),
            //["サブHP"] = data.artSub.hp.ToString(),
            ["スコア"] = data.artSub.Score.ToString()
        };

        //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));



        return result;
    }
}

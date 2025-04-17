using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gaming", menuName = "Scriptable Objects/Gaming")]
public class GamingSO : BaseCharacterSO
{
    readonly float[] pluggedAtkPerArray = { 3.686f, };

    float talent_addDmgBonusPluggedAtk = 0.2f;

    float constellation_atkRate = 0.2f;
    float constellation_critRate = 0.2f;
    float constellation_critDmg = 0.4f;


    public override Dictionary<string, string> CalcDmg(Data data)
    {

        data.atk += data.BaseAtk * constellation_atkRate;


        data.crit_rate_plugged_atk += constellation_critRate;
        data.crit_dmg_plugged += constellation_critDmg;
        data.plugged_atk_bonus += talent_addDmgBonusPluggedAtk;

        ElementalReaction elementalReaction = new(ElementType.Pyro, ElementType.Hydro, data);

        var ed = ExpectedDamage.Single(data, AttackType.Plugged, pluggedAtkPerArray[0], elementalReaction: elementalReaction);

        var sum = ed.Result;

        Dictionary<string, string> result = new()
        {
            ["武器"] = data.weapon.DisplayName,
            ["聖遺物セット"] = data.artSetData.name,
            ["聖遺物メイン"] = data.artMainData.name,
            ["バフキャラ"] = data.partyData.name,
            //["バフキャラ1"] = data.partyData.GetMemberName(0),
            //["バフキャラ2"] = data.partyData.GetMemberName(1),
            //["バフキャラ3"] = data.partyData.GetMemberName(2),
            ["合計期待値"] = sum.ToString(),
            ["攻撃力"] = data.atk.ToString(),
            // ["HP"] = baseData.hp.ToString(),
            // ["加算"] = (baseData.add + data.add_plugged_atk()).ToString(),
            //["バフ"] = (baseData.dmg_bonus + data.plugged_atk_bonus() + talent_addDmgBonusPluggedAtk).ToString(),
            //["会心ダメ期待値"] = crit.ExpectedCritDmg.ToString(),
            // ["耐性"] = baseData.res.ToString(),
            // ["蒸発"] = vaporize.ToString(),
            ["熟知"] = data.elemental_mastery.ToString(),
            ["率ダメ"] = ed.Crit.RateDmg,
            // ["会心ダメ比率"] = expectedDamage_skill.Crit.CritProportion,
            //["聖遺物組み合わせ"] = data.artSubData.name,
            ["サブステ"] = ed.Crit.SubCrit,
            //["サブHP%"] = data.artSubData.hp_rate.ToString(),
            //["サブHP"] = data.artSubData.hp.ToString(),
            ["スコア"] = data.artSubData.Score.ToString()
        };

        //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));



        return result;
    }
}

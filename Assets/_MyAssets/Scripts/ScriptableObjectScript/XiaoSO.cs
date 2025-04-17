using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Xiao", menuName = "Scriptable Objects/Xiao")]
public class XiaoSO : BaseCharacterSO
{
    float[] pluggedAtkPerArray = { 4.04f, };

    float burst_bonus = 0.952f + 0.175f;
    // float talent_addDmg_chargedAtk_PerDef = 0.35f;


    public override Dictionary<string, string> CalcDmg(Data data)
    {
        // if (data.energy_recharge() < 0.5f) return null;
        // if (data.weapon.name != "草薙の稲光") return null;
        // if (data.weapon.name != "和璞鳶") return null;


        data.normal_atk_bonus += burst_bonus;
        data.charged_atk_bonus += burst_bonus;
        data.plugged_atk_bonus += burst_bonus;


        var ed_plugged = ExpectedDamage.Single(data, AttackType.Plugged, pluggedAtkPerArray[0]);



        var sum = ed_plugged.Result;

        Dictionary<string, string> result = new()
        {
            ["武器"] = data.weapon.DisplayName,
            ["聖遺物セット"] = data.artSetData.name,
            ["聖遺物メイン"] = data.artMainData.name,
            ["バフキャラ"] = data.partyData.name,
            ["合計期待値"] = sum.ToString(),
            ["攻撃力"] = data.atk.ToString(),
            ["倍率"] = (pluggedAtkPerArray[0]).ToString(),
            // ["HP"] = data.hp.ToString(),
            ["加算"] = (data.add + data.add_plugged_atk).ToString(),
            ["バフ合計"] = (data.dmg_bonus + data.plugged_atk_bonus).ToString(),
            ["バフ共通"] = data.dmg_bonus.ToString(),
            ["バフ落下"] = data.plugged_atk_bonus.ToString(),
            ["耐性ダウン"] = data.res.ToString(),
            ["パーティ元素"] = data.partyData.Log,
            // ["耐性ダウン計算後"] = (GetElementalRes(data.res) * 0.5f).ToString(),
            //["会心ダメ期待値"] = crit.ExpectedCritDmg.ToString(),
            // ["耐性"] = data.res.ToString(),
            // ["蒸発"] = vaporize.ToString(),
            ["熟知"] = data.elemental_mastery.ToString(),
            ["率ダメ"] = ed_plugged.Crit.RateDmg,
            // ["会心ダメ比率"] = expectedDamage_skill.Crit.CritProportion,
            //["聖遺物組み合わせ"] = data.artSubData.name,
            ["サブステ"] = ed_plugged.Crit.SubCrit.ToString(),
            //["サブHP%"] = data.artSubData.hp_rate.ToString(),
            //["サブHP"] = data.artSubData.hp.ToString(),
            ["スコア"] = data.artSubData.Score.ToString()
        };

        //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));

        return result;
    }
}

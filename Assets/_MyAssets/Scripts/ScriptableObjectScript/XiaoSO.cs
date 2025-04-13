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
            ["武器"] = data.weapon.name,
            ["聖遺物セット"] = data.artSetData.name,
            ["聖遺物メイン"] = data.artMainData.name,
            ["バフキャラ"] = data.partyData.name,
            ["合計期待値"] = sum.ToString(),
            //  ["攻撃力"] = atk.ToString(),
            // ["HP"] = hpSum.ToString(),
            //["バフ"] = dmgBonus.ToString(),
            //["会心ダメ期待値"] = expectedDamage_skill.Crit.ExpectedCritDmg.ToString(),
            //["熟知"] = elementalMastery.ToString(),
            // ["率ダメ"] = expectedDamage_skill.Crit.RateDmg,
            //   ["会心ダメ比率"] = expectedDamage_skill.Crit.CritProportion,
            ["聖遺物組み合わせ"] = data.artSub.name,
            // ["サブステ"] = expectedDamage_skill.Crit.SubCrit.ToString(),
            ["サブHP%"] = data.artSub.hp_rate.ToString(),
            ["サブHP"] = data.artSub.hp.ToString(),
            ["スコア"] = data.artSub.Score.ToString()
        };

        //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));

        return result;
    }
}

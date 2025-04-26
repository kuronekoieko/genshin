using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Tartaglia", menuName = "Scriptable Objects/Tartaglia")]
public class Tartaglia : BaseCharacterSO
{



    public override Dictionary<string, string> CalcDmg(Data data)
    {


        var ed_normal = GetExpectedDamageFromStatus(data);

        var ed = ed_normal;
        var sum = ed_normal.Result;


        Dictionary<string, string> result = new()
        {
            ["武器"] = data.weaponData.DisplayName,
            ["聖遺物セット"] = data.artSetData.name,
            ["聖遺物メイン"] = data.artMainData.name,
            ["バフキャラ"] = data.partyData.name,
            ["合計期待値"] = sum.ToString(),
            ["攻撃力"] = data.atk.ToString(),
            //["倍率"] = (pluggedAtkPerArray[0]).ToString(),
            // ["HP"] = data.hp.ToString(),
            ["加算"] = ed.DmgAdd.ToString(),
            ["バフ合計"] = ed.DmgBonus.ToString(),
            ["バフ共通"] = data.dmg_bonus.ToString(),
            ["元素バフ"] = data.ElementalDmgBonus(ElementType.Hydro).ToString(),
            ["バフ通常"] = data.normal_atk_bonus.ToString(),
            ["耐性ダウン"] = data.res.ToString(),
            ["パーティ元素"] = data.partyData.Log,
            // ["耐性ダウン計算後"] = (GetElementalRes(data.res) * 0.5f).ToString(),
            //["会心ダメ期待値"] = crit.ExpectedCritDmg.ToString(),
            // ["耐性"] = data.res.ToString(),
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

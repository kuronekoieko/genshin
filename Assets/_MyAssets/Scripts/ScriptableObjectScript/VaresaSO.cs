using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Varesa", menuName = "Scriptable Objects/Varesa")]
public class VaresaSO : BaseCharacterSO
{
    readonly float[] pluggedAtkPerArray = { 5.52f, };


    float talent_addPerPluggedAtk_2 = 1.8f;


    public override Dictionary<string, string> CalcDmg(Data data)
    {



        Data charaData = data;


        var addAggravate = ElementalReaction.Aggravate(charaData.elemental_mastery, data.er_aggravate);

        float elementalReaction = 0;
        if (data.partyData.dendro_count > 0)
        {
            elementalReaction = addAggravate;
        }

        var (expectedDamage, crit) = charaData.ExpectedDmg(AttackType.Plugged, pluggedAtkPerArray[0] + talent_addPerPluggedAtk_2, er_add: elementalReaction);



        var sum = Mathf.FloorToInt(expectedDamage);

        Dictionary<string, string> result = new()
        {
            ["武器"] = data.weapon.name,
            ["聖遺物セット"] = data.artSetData.name,
            ["聖遺物メイン"] = data.artMainData.name,
            ["バフキャラ"] = data.partyData.name,
            ["合計期待値"] = sum.ToString(),
            ["攻撃力"] = charaData.atk.ToString(),
            ["倍率"] = (pluggedAtkPerArray[0] + talent_addPerPluggedAtk_2).ToString(),
            // ["HP"] = charaData.hp.ToString(),
            ["加算"] = (charaData.add + charaData.add_plugged_atk).ToString(),
            ["バフ合計"] = (charaData.dmg_bonus + charaData.plugged_atk_bonus).ToString(),
            ["バフ共通"] = charaData.dmg_bonus.ToString(),
            ["バフ落下"] = charaData.plugged_atk_bonus.ToString(),
            ["耐性ダウン"] = charaData.res.ToString(),
            // ["耐性ダウン計算後"] = (GetElementalRes(charaData.res) * 0.5f).ToString(),
            //["会心ダメ期待値"] = crit.ExpectedCritDmg.ToString(),
            // ["耐性"] = charaData.res.ToString(),
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

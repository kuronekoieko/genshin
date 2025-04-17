using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Varesa", menuName = "Scriptable Objects/Varesa")]
public class VaresaSO : BaseCharacterSO
{
    readonly float[] pluggedAtkPerArray = { 5.52f, };


    float talent_addPerPluggedAtk_2 = 1.8f;


    public override Dictionary<string, string> CalcDmg(Data data)
    {
        ElementalReaction elementalReaction = new(ElementType.Electro, ElementType.Dendro, data);

        if (data.partyData.ElementCounts[ElementType.Dendro] > 0)
        {
            elementalReaction = null;
        }

        float[] ary = new[] { pluggedAtkPerArray[0] + talent_addPerPluggedAtk_2 };
        var expectedDamage = ExpectedDamage.Sum(data, AttackType.Plugged, ary, elementalReaction: elementalReaction);



        var sum = expectedDamage.Result;

        Dictionary<string, string> result = new()
        {
            ["武器"] = data.weapon.DisplayName,
            ["聖遺物セット"] = data.artSetData.name,
            ["聖遺物メイン"] = data.artMainData.name,
            ["バフキャラ"] = data.partyData.name,
            ["合計期待値"] = sum.ToIntString(),
            ["攻撃力"] = data.atk.ToIntString(),
            ["倍率"] = (pluggedAtkPerArray[0] + talent_addPerPluggedAtk_2).ToIntString(),
            // ["HP"] = data.hp.ToIntString(),
            ["加算"] = (data.add + data.add_plugged_atk).ToIntString(),
            ["バフ合計"] = (data.dmg_bonus + data.plugged_atk_bonus + data.electro_bonus).ToIntString(),
            ["バフ共通"] = data.dmg_bonus.ToIntString(),
            ["雷バフ"] = data.electro_bonus.ToIntString(),
            ["バフ落下"] = data.plugged_atk_bonus.ToIntString(),
            ["耐性ダウン"] = data.res.ToIntString(),
            ["パーティ元素"] = data.partyData.Log,
            // ["聖遺物メインデータ"] = JsonUtility.ToJson(data.artMainData),
            // ["耐性ダウン計算後"] = (GetElementalRes(data.res) * 0.5f).ToIntString(),
            //["会心ダメ期待値"] = crit.ExpectedCritDmg.ToIntString(),
            // ["耐性"] = data.res.ToIntString(),
            // ["蒸発"] = vaporize.ToIntString(),
            ["熟知"] = data.elemental_mastery.ToIntString(),
            ["率ダメ"] = expectedDamage.Crit.RateDmg,
            // ["会心ダメ比率"] = expectedDamage_skill.Crit.CritProportion,
            //["聖遺物組み合わせ"] = data.artSubData.name,
            ["サブステ"] = expectedDamage.Crit.SubCrit,
            //["サブHP%"] = data.artSubData.hp_rate.ToIntString(),
            //["サブHP"] = data.artSubData.hp.ToIntString(),
            ["スコア"] = data.artSubData.Score.ToIntString()
        };

        //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));

        return result;
    }

}

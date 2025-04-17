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

        float[] ary = new[] { pluggedAtkPerArray[0] + talent_addPerPluggedAtk_2 };
        var expectedDamage = ExpectedDamage.Sum(data, AttackType.Plugged, ary, elementalReaction: elementalReaction);



        var sum = expectedDamage.Result;

        Dictionary<string, string> result = new()
        {
            ["武器"] = data.Weapon.DisplayName,
            ["聖遺物セット"] = data.ArtSetData.name,
            ["聖遺物メイン"] = data.ArtMainData.name,
            ["バフキャラ"] = data.PartyData.name,
            ["合計期待値"] = sum.ToString(),
            ["攻撃力"] = data.atk.ToString(),
            ["倍率"] = (pluggedAtkPerArray[0] + talent_addPerPluggedAtk_2).ToString(),
            // ["HP"] = data.hp.ToString(),
            ["加算"] = (data.add + data.add_plugged_atk).ToString(),
            ["バフ合計"] = (data.dmg_bonus + data.plugged_atk_bonus + data.electro_bonus).ToString(),
            ["バフ共通"] = data.dmg_bonus.ToString(),
            ["雷バフ"] = data.electro_bonus.ToString(),
            ["バフ落下"] = data.plugged_atk_bonus.ToString(),
            ["耐性ダウン"] = data.res.ToString(),
            ["パーティ元素"] = data.PartyData.Log,
            // ["聖遺物メインデータ"] = JsonUtility.ToJson(data.artMainData),
            // ["耐性ダウン計算後"] = (GetElementalRes(data.res) * 0.5f).ToString(),
            //["会心ダメ期待値"] = crit.ExpectedCritDmg.ToString(),
            // ["耐性"] = data.res.ToString(),
            // ["蒸発"] = vaporize.ToString(),
            ["熟知"] = data.elemental_mastery.ToString(),
            ["率ダメ"] = expectedDamage.Crit.RateDmg,
            // ["会心ダメ比率"] = expectedDamage_skill.Crit.CritProportion,
            //["聖遺物組み合わせ"] = data.artSubData.name,
            ["サブステ"] = expectedDamage.Crit.SubCrit,
            //["サブHP%"] = data.artSubData.hp_rate.ToString(),
            //["サブHP"] = data.artSubData.hp.ToString(),
            ["スコア"] = data.ArtSubData.Score.ToString()
        };

        //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));

        return result;
    }

}

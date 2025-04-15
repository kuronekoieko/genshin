using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Noelle", menuName = "Scriptable Objects/" + "Noelle")]
public class Noelle : BaseCharacterSO
{
    // スキル Lv9
    float[] normalAtkPerArray = { 1.45f, 1.35f, 1.58f, 2.08f, };

    float[] pluggedAtkPerArray = { 2.74f };

    float burst_addAtkPerDef = 0.68f;
    float constellation_addAtkPerDef = 0.5f;


    public override Dictionary<string, string> CalcDmg(Data data)
    {

        data.atk += (burst_addAtkPerDef + constellation_addAtkPerDef) * data.def;

        var ed_normal = ExpectedDamage.Single(data, AttackType.Normal, atkRate: normalAtkPerArray[0]);
        var ed_plugged = ExpectedDamage.Single(data, AttackType.Plugged, atkRate: pluggedAtkPerArray[0]);


        Dictionary<string, string> result = new()
        {
            ["武器"] = data.weapon.name,
            ["聖遺物セット"] = data.artSetData.name,
            ["聖遺物メイン"] = data.artMainData.name,
            ["バフキャラ"] = data.partyData.name,
            ["合計期待値"] = ed_plugged.Result.ToString(),
            ["攻撃力"] = data.atk.ToString(),
            ["防御力"] = data.def.ToString(),
            // ["HP"] = data.hp.ToString(),
            ["バフ合計"] = (data.dmg_bonus + data.plugged_atk_bonus + data.ElementalDmgBonus(ElementType.Geo)).ToString(),
            ["バフ共通"] = data.dmg_bonus.ToString(),
            ["元素バフ"] = data.ElementalDmgBonus(ElementType.Geo).ToString(),
            ["バフ落下"] = data.plugged_atk_bonus.ToString(),
            ["パーティ元素"] = data.partyData.Log,

            // ["会心ダメ期待値"] = ed_plugged.Crit.ExpectedCritDmg.ToString(),
            // ["熟知"] = data.elemental_mastery.ToString(),
            ["率ダメ"] = ed_plugged.Crit.RateDmg,
            ["会心ダメ比率"] = ed_plugged.Crit.CritProportion,
            ["聖遺物組み合わせ"] = data.artSubData.name,
            ["サブステ"] = ed_plugged.Crit.SubCrit.ToString(),
            ["サブHP%"] = data.artSubData.hp_rate.ToString(),
            ["サブHP"] = data.artSubData.hp.ToString(),
            ["スコア"] = data.artSubData.Score.ToString()
        };

        //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));

        return result;
    }



}

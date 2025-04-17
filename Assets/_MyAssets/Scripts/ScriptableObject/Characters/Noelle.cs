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

        //var ed_normal = ExpectedDamage.Sum(data, AttackType.Normal, normalAtkPerArray);
        var ed_plugged = ExpectedDamage.Single(data, AttackType.Plugged, atkRate: pluggedAtkPerArray[0]);
        ExpectedDamage ed = ed_plugged;

        Dictionary<string, string> result = new()
        {
            ["武器"] = data.Weapon.DisplayName,
            ["聖遺物セット"] = data.ArtSetData.name,
            ["聖遺物メイン"] = data.ArtMainData.name,
            ["バフキャラ"] = data.PartyData.name,
            ["合計期待値"] = ed.Result.ToString(),
            ["攻撃力"] = data.atk.ToString(),
            ["防御力"] = data.def.ToString(),
            // ["HP"] = data.hp.ToString(),
            ["バフ合計"] = ed.DmgBonus.ToString(),
            ["バフ共通"] = data.dmg_bonus.ToString(),
            ["元素バフ"] = data.ElementalDmgBonus(ElementType.Geo).ToString(),
            ["バフ落下"] = data.plugged_atk_bonus.ToString(),
            ["パーティ元素"] = data.PartyData.Log,

            // ["会心ダメ期待値"] = ed_plugged.Crit.ExpectedCritDmg.ToString(),
            // ["熟知"] = data.elemental_mastery.ToString(),
            ["率ダメ"] = ed.Crit.RateDmg,
            ["会心ダメ比率"] = ed.Crit.CritProportion,
            ["聖遺物組み合わせ"] = data.ArtSubData.name,
            ["サブステ"] = ed.Crit.SubCrit,
            ["サブHP%"] = data.ArtSubData.hp_rate.ToString(),
            ["サブHP"] = data.ArtSubData.hp.ToString(),
            ["スコア"] = data.ArtSubData.Score.ToString()
        };

        //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));

        return result;
    }



}

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
            ["武器"] = data.weapon.DisplayName,
            ["聖遺物セット"] = data.artSetData.name,
            ["聖遺物メイン"] = data.artMainData.name,
            ["バフキャラ"] = data.partyData.name,
            ["合計期待値"] = ed.Result.ToIntString(),
            ["攻撃力"] = data.atk.ToIntString(),
            ["防御力"] = data.def.ToIntString(),
            // ["HP"] = data.hp.ToIntString(),
            ["バフ合計"] = ed.DmgBonus.ToIntString(),
            ["バフ共通"] = data.dmg_bonus.ToIntString(),
            ["元素バフ"] = data.ElementalDmgBonus(ElementType.Geo).ToIntString(),
            ["バフ落下"] = data.plugged_atk_bonus.ToIntString(),
            ["パーティ元素"] = data.partyData.Log,

            // ["会心ダメ期待値"] = ed_plugged.Crit.ExpectedCritDmg.ToIntString(),
            // ["熟知"] = data.elemental_mastery.ToIntString(),
            ["率ダメ"] = ed.Crit.RateDmg,
            ["会心ダメ比率"] = ed.Crit.CritProportion,
            ["聖遺物組み合わせ"] = data.artSubData.name,
            ["サブステ"] = ed.Crit.SubCrit,
            ["サブHP%"] = data.artSubData.hp_rate.ToIntString(),
            ["サブHP"] = data.artSubData.hp.ToIntString(),
            ["スコア"] = data.artSubData.Score.ToIntString()
        };

        //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));

        return result;
    }



}

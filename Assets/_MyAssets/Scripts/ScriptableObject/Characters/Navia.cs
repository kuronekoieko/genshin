using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Navia", menuName = "Scriptable Objects/Navia")]
public class Navia : BaseCharacterSO
{

    float talent_dmg_bonus = 0.4f;
    float talent_atk_per = 0.2f;

    public override Dictionary<string, string> CalcDmg(Data data)
    {
        data.normal_atk_bonus += talent_dmg_bonus;
        data.charged_atk_bonus += talent_dmg_bonus;
        data.plugged_atk_bonus += talent_dmg_bonus;

        int eCount = data.PartyData.ElementCounts[ElementType.Pyro] + data.PartyData.ElementCounts[ElementType.Cryo] + data.PartyData.ElementCounts[ElementType.Electro] + data.PartyData.ElementCounts[ElementType.Hydro];
        if (eCount == 1) data.AddAtkRate(talent_atk_per);
        if (eCount > 1) data.AddAtkRate(talent_atk_per * 2f);

        var ed_skill = GetExpectedDamageFromStatus(data);

        var ed = ed_skill;
        var sum = ed_skill.Result;


        Dictionary<string, string> result = new()
        {
            ["武器"] = data.Weapon.DisplayName,
            ["聖遺物セット"] = data.ArtSetData.name,
            ["聖遺物メイン"] = data.ArtMainData.name,
            ["バフキャラ"] = data.PartyData.name,
            ["合計期待値"] = sum.ToString(true),
            ["攻撃力"] = data.atk.ToString(true),
            //["倍率"] = (pluggedAtkPerArray[0]).ToString(),
            // ["HP"] = data.hp.ToString(),
            ["加算"] = ed.DmgAdd.ToString(true),
            ["バフ合計"] = ed.DmgBonus.ToString(),
            ["バフ共通"] = data.dmg_bonus.ToString(),
            ["元素バフ"] = data.ElementalDmgBonus(ElementType.Hydro).ToString(),
            ["バフ通常"] = data.normal_atk_bonus.ToString(),
            ["耐性ダウン"] = data.res.ToString(),
            ["パーティ元素"] = data.PartyData.Log,
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
            ["スコア"] = data.ArtSubData.Score.ToString()
        };

        //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));

        return result;
    }
}

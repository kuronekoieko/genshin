using UnityEngine;
using System.Collections.Generic;

public class Varesa : BaseCharacter
{
    readonly float[] normalAtkPerArray = { 0, };
    readonly float[] chargedAtkPerArray = { 0, };
    //readonly float[] pluggedAtkPerArray = { 3.91f, };
    // lv8
    readonly float[] pluggedAtkPerArray = { 5.52f, };

    readonly float[] skillPerArray = { 0, };
    readonly float[] burstPerArray = { 0, };

    float talent_addPerPluggedAtk_1 = 0.5f;
    float talent_addPerPluggedAtk_2 = 1.8f;


    // float constellation_atkRate = 0.2f;
    // float constellation_critRate = 0.2f;
    // float constellation_critDmg = 0.4f;
    // float constellation_critRate = 0;
    //float constellation_critDmg = 0;

    public override Dictionary<string, string> CalcDmg(Data data)
    {

        //if (data.partyData.name.Contains("閑雲")) return null;
        // if (data.partyData.name.Contains("ベネット")) return null;
        //if (data.weapon.name.Contains("螭龍の剣(完凸)") && !data.partyData.name.Contains("鍾離")) return null;
        // if (data.energy_recharge() == 0) return null;
        //if (data.partyData.name.Contains("ベネット") == false && data.energy_recharge() == 0) return null;
        // if (data.partyData.name.Contains("鍾離") == false) return null;
        //if (data.partyData.hydro_count == 0) return null;

        //if (data.partyData.name.Contains("フリーナ") == false) return null;
        // if (data.energy_recharge() < 0.5f) return null;
        // if (data.weapon.name != "草薙の稲光") return null;
        // if (data.weapon.name != "和璞鳶") return null;

        BaseData baseData = GetBaseData(data);

        // var melt = ElementalReaction.MeltForPyro(baseData.elemental_mastery, 0);
        //  var vaporize = ElementalReaction.VaporizeForPyro(baseData.elemental_mastery, data.er_rate());
        var addAggravate = ElementalReaction.Aggravate(baseData.elemental_mastery, data.er_aggravate());

        float elementalReaction = 1;
        if (data.partyData.dendro_count > 0)
        {
            elementalReaction = addAggravate;
        }
        // var (expectedDamage, crit) = ExpectedDmg_normalAtk(property);
        // var (expectedDamage, crit) = ExpectedDmg_chargedAtk(property);
        var (expectedDamage, crit) = ExpectedDmg_pluggedAtk(baseData, data, elementalReaction);

        //  var (expectedDamage, crit) = ExpectedDmg_skill(property);
        // var (expectedDamage, crit) = ExpectedDmg_burst(property);


        var sum = Mathf.FloorToInt(expectedDamage);

        Dictionary<string, string> result = new()
        {
            ["武器"] = data.weapon.name,
            ["聖遺物セット"] = data.artSetData.name,
            ["聖遺物メイン"] = data.artMainData.name,
            ["バフキャラ"] = data.partyData.name,
            ["合計期待値"] = sum.ToString(),
            ["攻撃力"] = baseData.atk.ToString(),
            // ["HP"] = baseData.hp.ToString(),
            // ["加算"] = (baseData.add + data.add_plugged_atk()).ToString(),
            //["バフ"] = (baseData.dmg_bonus + data.plugged_atk_bonus() + talent_addDmgBonusPluggedAtk).ToString(),
            //["会心ダメ期待値"] = crit.ExpectedCritDmg.ToString(),
            // ["耐性"] = baseData.res.ToString(),
            // ["蒸発"] = vaporize.ToString(),
            ["熟知"] = baseData.elemental_mastery.ToString(),
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

    BaseData GetBaseData(Data data)
    {
        BaseData baseData = new()
        {
            heal_bonus = data.heal_bonus(),
            hp_rate = data.hpPerSum(),
            energy_recharge = 1 + data.energy_recharge(),
            elemental_mastery = data.elemental_mastery(),
            def_rate = data.def_rate(),
            atk_rate = data.atk_rate(),
            dmg_bonus = data.dmg_bonus() + data.ElementalDmgBonus(),
            atk_speed = data.atk_speed(),
            crit_rate = data.crit_rate(),
            crit_dmg = data.crit_dmg(),
            add = data.add(),
            res = GetElementalRes(data.partyData.res) * 0.5f
        };

        baseData.hp = status.baseHp * (1 + baseData.hp_rate) + data.hp();
        baseData.def = status.baseDef * (1 + baseData.def_rate) + data.def();


        var homa_atkAdd = baseData.hp * data.weapon.homa;
        var sekisa_atkAdd = baseData.elemental_mastery * data.weapon.sekisha;
        var kusanagi_atkAdd = (baseData.energy_recharge - 1) * data.weapon.kusanagi;

        baseData.atk
            = data.base_atk() * (1 + baseData.atk_rate)
            + data.atk()
            + homa_atkAdd
            + sekisa_atkAdd
            + kusanagi_atkAdd;

        return baseData;
    }


    (float, Crit) ExpectedDmg_pluggedAtk(BaseData baseData, Data data, float elementalReaction)
    {
        float dmgAdd = data.add_plugged_atk();
        float dmgBonus = data.plugged_atk_bonus();
        float critRate = baseData.crit_rate + data.crit_rate_plugged_atk();
        float critDmg = baseData.crit_dmg + data.crit_dmg_plugged();

        var crit = new Crit(critRate, critDmg, data.artSub);

        ExpectedDamage expectedDamage_pluggedAtk = new(
          baseData.atk,
          baseData.add + dmgAdd,
          baseData.dmg_bonus + dmgBonus,
          crit.ExpectedCritDmg,
          baseData.res
        );

        float expectedDamage = expectedDamage_pluggedAtk.GetExpectedDamageSum(pluggedAtkPerArray, elementalReaction);

        return (expectedDamage, crit);

    }



    (float, Crit) ExpectedDmg_normalAtk(BaseData baseData, Data data, float elementalReaction)
    {
        var dmgAdd_sekikaku = baseData.def * data.weapon.sekikaku;

        float dmgAdd = data.add_normal_atk() + dmgAdd_sekikaku;
        float dmgBonus = data.normal_atk_bonus();
        float critRate = baseData.crit_rate + data.crit_rate_normal_atk();
        float critDmg = baseData.crit_dmg;

        var crit = new Crit(critRate, critDmg, data.artSub);

        ExpectedDamage expectedDamage_pluggedAtk = new(
          baseData.atk,
          baseData.add + dmgAdd,
          baseData.dmg_bonus + dmgBonus,
          crit.ExpectedCritDmg,
          baseData.res
        );

        float expectedDamage = expectedDamage_pluggedAtk.GetExpectedDamageSum(pluggedAtkPerArray, elementalReaction);

        return (expectedDamage, crit);

    }

    (float, Crit) ExpectedDmg_chargedAtk(BaseData baseData, Data data, float elementalReaction)
    {
        var dmgAdd_sekikaku = baseData.def * data.weapon.sekikaku;

        float dmgAdd = data.add_charged_atk() + dmgAdd_sekikaku;
        float dmgBonus = data.charged_atk_bonus();
        float critRate = baseData.crit_rate + data.crit_rate_charged_atk();
        float critDmg = baseData.crit_dmg;

        var crit = new Crit(critRate, critDmg, data.artSub);

        ExpectedDamage expectedDamage_pluggedAtk = new(
          baseData.atk,
          baseData.add + dmgAdd,
          baseData.dmg_bonus + dmgBonus,
          crit.ExpectedCritDmg,
          baseData.res
        );

        float expectedDamage = expectedDamage_pluggedAtk.GetExpectedDamageSum(pluggedAtkPerArray, elementalReaction);

        return (expectedDamage, crit);

    }

    (float, Crit) ExpectedDmg_skill(BaseData baseData, Data data, float elementalReaction)
    {
        float dmgAdd = data.add_skill() + baseData.def * data.weapon.cinnabar;
        float dmgBonus = data.skill_bonus();
        float critRate = baseData.crit_rate + data.crit_rate_skill();
        float critDmg = baseData.crit_dmg;

        var crit = new Crit(critRate, critDmg, data.artSub);

        ExpectedDamage expectedDamage_pluggedAtk = new(
          baseData.atk,
          baseData.add + dmgAdd,
          baseData.dmg_bonus + dmgBonus,
          crit.ExpectedCritDmg,
          baseData.res
        );

        float expectedDamage = expectedDamage_pluggedAtk.GetExpectedDamageSum(pluggedAtkPerArray, elementalReaction);

        return (expectedDamage, crit);

    }

    (float, Crit) ExpectedDmg_burst(BaseData baseData, Data data, float elementalReaction)
    {
        float dmgAdd = data.add_burst();
        float dmgBonus = data.burst_bonus();
        float critRate = baseData.crit_rate + data.crit_rate_burst();
        float critDmg = baseData.crit_dmg;

        var crit = new Crit(critRate, critDmg, data.artSub);

        ExpectedDamage expectedDamage_pluggedAtk = new(
          baseData.atk,
          baseData.add + dmgAdd,
          baseData.dmg_bonus + dmgBonus,
          crit.ExpectedCritDmg,
          baseData.res
        );

        float expectedDamage = expectedDamage_pluggedAtk.GetExpectedDamageSum(pluggedAtkPerArray, elementalReaction);

        return (expectedDamage, crit);

    }
}

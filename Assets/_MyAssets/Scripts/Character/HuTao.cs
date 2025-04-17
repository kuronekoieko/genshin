using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuTao : BaseCharacter
{
    // lv10
    float[] chargedAtkPerArray = { 2.42f, };

    float skill_addAtkPerHp = 6.26f * 0.01f;

    float talent_dmgBonus = 0.33f;




    public override Dictionary<string, string> CalcDmg(Data data)
    {
        if (data.partyData.name.Contains("夜蘭") == false) return null;
        // if (data.energy_recharge() < 0.5f) return null;
        // if (data.weapon.name != "草薙の稲光") return null;
        // if (data.weapon.name != "和璞鳶") return null;


        data.pyro_bonus += talent_dmgBonus;
        data.atk += data.hp * skill_addAtkPerHp;

        ElementalReaction elementalReaction = new(ElementType.Pyro, ElementType.Hydro, data);

        // var melt = ElementalReaction.MeltForPyro(elementalMastery, 0);

        // var ed_normal = ExpectedDamage.Single(data, AttackType.Normal,  status.elementType,normalAtkPerArray);
        var ed_charged = ExpectedDamage.Single(data, AttackType.Charged, chargedAtkPerArray[0], elementalReaction: elementalReaction);

        // var ed_plugged = ExpectedDamage.Single(data, AttackType.Plugged,status.elementType,pluggedAtkPerArray);
        // var expectedDamage_skill = ExpectedDamage.Single(data, AttackType.Skill, status.elementType,skillPerArray);
        // var (expectedDamage_burst, crit_burst) = ExpectedDamage.Single(data, AttackType.Skill, status.elementType,null);





        var sum = ed_charged;

        Dictionary<string, string> result = new()
        {
            ["武器"] = data.weapon.DisplayName,
            ["聖遺物セット"] = data.artSetData.name,
            ["聖遺物メイン"] = data.artMainData.name,
            ["バフキャラ"] = data.partyData.name,
            ["合計期待値"] = sum.ToString(),
            //  ["攻撃力"] = atk.ToString(),
            // ["防御力"] = def.ToString(),
            // ["HP"] = hpSum.ToString(),
            // ["バフ"] = dmgBonus.ToString(),
            // ["会心ダメ期待値"] = crit_ChargedAttack.ExpectedCritDmg.ToString(),
            // ["熟知"] = elementalMastery.ToString(),
            // ["率ダメ"] = crit_ChargedAttack.RateDmg,
            // ["会心ダメ比率"] = crit_ChargedAttack.CritProportion,
            ["聖遺物組み合わせ"] = data.artSubData.name,
            //   ["サブステ"] = crit_ChargedAttack.SubCrit.ToString(),
            // ["サブHP%"] = data.artSubData.hp_rate.ToString(),
            // ["サブHP"] = data.artSubData.hp.ToString(),
            ["スコア"] = data.artSubData.Score.ToString(),
        };

        //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));

        return result;
    }
}

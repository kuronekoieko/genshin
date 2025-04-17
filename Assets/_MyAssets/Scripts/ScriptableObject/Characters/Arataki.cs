using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace so
{
    public class Arataki : BaseCharacterSO
    {
        // スキル Lv9
        float[] normalAtkPerArray = { 1.456f, 1.403f, 1.684f, 2.154f, };
        float[] chargedAtkPerArray = { 1.675f, 1.675f, 1.675f, 1.675f, 3.508f, };
        //float[] chargedAtkPerArray = { 1.675f };

        float burst_addAtkPerDef = 0.979f;
        float talent_addDmg_chargedAtk_PerDef = 0.35f;


        public override Dictionary<string, string> CalcDmg(Data data)
        {
            // 時計原チャじゃないと、かなりdps落ちる
            if (data.energy_recharge < 0.5f) return null;


            data.atk += burst_addAtkPerDef * data.def;
            data.add_charged_atk += talent_addDmg_chargedAtk_PerDef * data.def;


            var ed_normal = ExpectedDamage.Sum(data, AttackType.Normal, normalAtkPerArray);
            var ed_charged = ExpectedDamage.Sum(data, AttackType.Charged, chargedAtkPerArray);

            var sum = ed_normal.Result + ed_charged.Result;

            Dictionary<string, string> result = new()
            {
                ["武器"] = data.weapon.DisplayName,
                ["聖遺物セット"] = data.artSetData.name,
                ["聖遺物メイン"] = data.artMainData.name,
                ["バフキャラ"] = data.partyData.name,
                ["合計期待値"] = sum.ToIntString(),
                ["攻撃力"] = data.atk.ToIntString(),
                ["HP"] = data.hp.ToIntString(),
                //  ["バフ"] = dmgBonus.ToIntString(),
                // ["会心ダメ期待値"] = expectedDamage_skill.Crit.ExpectedCritDmg.ToIntString(),
                // ["熟知"] = elementalMastery.ToIntString(),
                // ["率ダメ"] = expectedDamage_skill.Crit.RateDmg,
                // ["会心ダメ比率"] = expectedDamage_skill.Crit.CritProportion,
                ["聖遺物組み合わせ"] = data.artSubData.name,
                // ["サブステ"] = expectedDamage_skill.Crit.SubCrit,
                ["サブHP%"] = data.artSubData.hp_rate.ToIntString(),
                ["サブHP"] = data.artSubData.hp.ToIntString(),
                ["スコア"] = data.artSubData.Score.ToIntString()
            };

            //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));

            return result;
        }
    }
}
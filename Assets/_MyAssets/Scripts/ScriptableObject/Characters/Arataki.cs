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
                ["武器"] = data.Weapon.DisplayName,
                ["聖遺物セット"] = data.ArtSetData.name,
                ["聖遺物メイン"] = data.ArtMainData.name,
                ["バフキャラ"] = data.PartyData.name,
                ["合計期待値"] = sum.ToString(),
                ["攻撃力"] = data.atk.ToString(),
                ["HP"] = data.hp.ToString(),
                //  ["バフ"] = dmgBonus.ToString(),
                // ["会心ダメ期待値"] = expectedDamage_skill.Crit.ExpectedCritDmg.ToString(),
                // ["熟知"] = elementalMastery.ToString(),
                // ["率ダメ"] = expectedDamage_skill.Crit.RateDmg,
                // ["会心ダメ比率"] = expectedDamage_skill.Crit.CritProportion,
                ["聖遺物組み合わせ"] = data.ArtSubData.name,
                // ["サブステ"] = expectedDamage_skill.Crit.SubCrit,
                ["サブHP%"] = data.ArtSubData.hp_rate.ToString(),
                ["サブHP"] = data.ArtSubData.hp.ToString(),
                ["スコア"] = data.ArtSubData.Score.ToString()
            };

            //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));

            return result;
        }
    }
}
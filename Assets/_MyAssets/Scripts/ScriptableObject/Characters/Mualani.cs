using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace so
{
    public class Mualani : BaseCharacterSO
    {
        // lv9
        readonly static float normalAtkPer = 0.1476f + 0.3689f + 0.0738f * 3f;
        readonly float[] normalAtkPerArray = { normalAtkPer + const_addAtkPerHp, normalAtkPer, normalAtkPer };

        readonly static float const_addAtkPerHp = 0.66f;




        public override Dictionary<string, string> CalcDmg(Data data)
        {
            if (data.partyData.name.Contains("トーマ") == false) return null;
            // if (data.partyData.name.Contains("夜蘭") == false) return null;
            // if (data.energy_recharge() < 0.5f) return null;
            // if (data.weapon.name != "草薙の稲光") return null;
            // if (data.weapon.name != "和璞鳶") return null;


            ElementalReaction elementalReaction = new(ElementType.Hydro, ElementType.Pyro, data);


            var ed_normal_1 = ExpectedDamage.Single(data, AttackType.Normal, normalAtkPerArray[0], referenceStatus: ReferenceStatus.Hp, elementalReaction: elementalReaction);

            // var ed_normal = ExpectedDmg_multi(AttackType.Normal,  hpRate: normalAtkPerArray, elementalReaction: vaporizeForHydro);
            //  var ed_charged = ExpectedDamage.Single(data, AttackType.Charged,  chargedAtkPerArray);
            //var ed_plugged = ExpectedDamage.Single(data, AttackType.Plugged,pluggedAtkPerArray);
            //var expectedDamage_skill = ExpectedDamage.Single(data, AttackType.Skill, skillPerArray);
            //var (expectedDamage_burst, crit_burst) = ExpectedDamage.Single(data, AttackType.Skill, null);





            var ed = ed_normal_1;

            Dictionary<string, string> result = new()
            {
                ["武器"] = data.weapon.DisplayName,
                ["聖遺物セット"] = data.artSetData.name,
                ["聖遺物メイン"] = data.artMainData.name,
                ["バフキャラ"] = data.partyData.name,
                ["合計期待値"] = ed.Result.ToIntString(),
                ["一発目期待値"] = ed.Result.ToIntString(),
                //["攻撃力"] = atk.ToIntString(),
                // ["防御力"] = def.ToIntString(),
                //["HP"] = hpSum.ToIntString(),
                //["共通バフ"] = dmgBonus.ToIntString(),
                // ["通常バフ"] = normalAtkDmgBonus.ToIntString(),
                // ["会心ダメ期待値"] = crit_ChargedAttack.ExpectedCritDmg.ToIntString(),
                //  ["熟知"] = elementalMastery.ToIntString(),
                // ["率ダメ"] = crit_normalAttack.RateDmg,
                // ["会心ダメ比率"] = crit_ChargedAttack.CritProportion,
                ["聖遺物組み合わせ"] = data.artSubData.name,
                // ["サブステ"] = crit_normalAttack.SubCrit,
                // ["サブHP%"] = data.artSubData.hp_rate.ToIntString(),
                // ["サブHP"] = data.artSubData.hp.ToIntString(),
                ["スコア"] = data.artSubData.Score.ToIntString(),
            };

            //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));

            return result;
        }
    }
}
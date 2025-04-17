using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace so
{
    public class HuTao : BaseCharacterSO
    {
        // lv10
        float[] chargedAtkPerArray = { 2.426f, };
        readonly float[] pluggedAtkPerArray = { 2.92f, };

        float skill_addAtkPerHp = 6.26f * 0.01f;

        float talent_dmgBonus_when_hp_lower = 0.33f;




        public override Dictionary<string, string> CalcDmg(Data data)
        {
            // if (data.partyData.name.Contains("夜蘭") == false) return null;
            // if (data.energy_recharge() < 0.5f) return null;
            // if (data.weapon.name != "草薙の稲光") return null;
            // if (data.weapon.name != "和璞鳶") return null;


            bool isHealer = data.partyData.members.Any(m => m.HealerType == HealerType.Healer);

            if (isHealer)
            {
                data.pyro_bonus += talent_dmgBonus_when_hp_lower;
            }
            data.atk += data.hp * skill_addAtkPerHp;


            ElementalReaction elementalReaction = new(ElementType.Pyro, ElementType.Hydro, data);

            // var melt = ElementalReaction.MeltForPyro(elementalMastery, 0);

            // var ed_normal = ExpectedDamage.Single(data, AttackType.Normal,  status.elementType,normalAtkPerArray);
            var ed_charged = ExpectedDamage.Single(data, AttackType.Charged, chargedAtkPerArray[0], elementalReaction: elementalReaction);
            // var ed_plugged = ExpectedDamage.Single(data, AttackType.Plugged, pluggedAtkPerArray[0], elementalReaction: elementalReaction);

            // var ed_plugged = ExpectedDamage.Single(data, AttackType.Plugged,status.elementType,pluggedAtkPerArray);
            // var expectedDamage_skill = ExpectedDamage.Single(data, AttackType.Skill, status.elementType,skillPerArray);
            // var (expectedDamage_burst, crit_burst) = ExpectedDamage.Single(data, AttackType.Skill, status.elementType,null);





            //  var ed = ed_charged;
            var ed = ed_charged;

            //float sum = ed_charged.Result + ed_plugged.Result;
            float sum = ed_charged.Result;


            Dictionary<string, string> result = new()
            {
                ["武器"] = data.weapon.DisplayName,
                ["聖遺物セット"] = data.artSetData.name,
                ["聖遺物メイン"] = data.artMainData.name,
                ["バフキャラ"] = data.partyData.name,
                ["合計期待値"] = sum.ToString(isInt: true),
                ["攻撃力"] = data.atk.ToString(isInt: true),
                // ["防御力"] = def.ToString(),
                ["HP"] = data.hp.ToString(isInt: true),
                ["バフ"] = ed.DmgBonus.ToString(),
                ["会心ダメ期待値"] = ed.Crit.ExpectedCritDmg.ToString(),
                ["熟知"] = data.elemental_mastery.ToString(),
                ["元素反応"] = elementalReaction.result_er_multi.ToString(),
                ["率ダメ"] = ed.Crit.RateDmg,
                // ["会心ダメ比率"] = crit_ChargedAttack.CritProportion,
                ["聖遺物組み合わせ"] = data.artSubData.name,
                ["花"] = data.artSubData.flower.DisplayName,
                ["羽"] = data.artSubData.plume.DisplayName,
                ["時計"] = data.artSubData.sands.DisplayName,
                ["杯"] = data.artSubData.goblet.DisplayName,
                ["冠"] = data.artSubData.circlet.DisplayName,
                ["サブステ"] = ed.Crit.SubCrit,
                ["サブHP%"] = data.artSubData.hp_rate.ToString(),
                ["サブHP"] = data.artSubData.hp.ToString(),
                ["スコア"] = data.artSubData.Score.ToString(),
            };

            //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));

            return result;
        }
    }
}
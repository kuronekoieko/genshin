using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace so
{
    public class HuTao : BaseCharacterSO
    {
        // lv10
        // float[] chargedAtkPerArray = { 2.426f, };
        // readonly float[] pluggedAtkPerArray = { 2.92f, };

        float skill_addAtkPerHp = 6.26f * 0.01f;

        float talent_dmgBonus_when_hp_lower = 0.33f;




        public override Dictionary<string, string> CalcDmg(Data data)
        {

            bool existsHealer = data.partyData.members.Any(m => m.is_healer);

            if (existsHealer == false)
            {
                data.pyro_bonus += talent_dmgBonus_when_hp_lower;

                bool existsShield = data.partyData.members.Any(m => m.has_shields);
                if (existsShield == false) return null;
            }


            data.atk += data.hp * skill_addAtkPerHp;


            ElementalReaction elementalReaction = new(ElementType.Pyro, ElementType.Hydro, data);


            var ed_status = GetExpectedDamageFromStatus(data, elementalReaction);

            // var ed_plugged = ExpectedDamage.Single(data, AttackType.Plugged, pluggedAtkPerArray[0], elementalReaction: elementalReaction);

            var ed = ed_status;

            //float sum = ed_charged.Result + ed_plugged.Result;
            float sum = ed.Result;


            Dictionary<string, string> result = new()
            {
                ["武器"] = data.weaponData.DisplayName,
                ["聖遺物セット"] = data.artSetData.name,
                ["聖遺物メイン"] = data.artMainData.name,
                ["バフキャラ"] = data.partyData.name,
                ["合計期待値"] = sum.ToString(isInt: true),
                ["攻撃力"] = data.atk.ToString(isInt: true),
                ["護摩攻撃力up"] = (data.hp * data.weaponData.homa).ToString(),
                ["加算"] = ed.DmgAdd.ToString(),
                // ["防御力"] = def.ToString(),
                ["HP"] = data.hp.ToString(isInt: true),
                ["バフ"] = ed.DmgBonus.ToString(),
                ["バフ共通"] = data.dmg_bonus.ToString(),
                ["元素バフ"] = data.pyro_bonus.ToString(),
                ["バフ落下"] = data.plugged_atk_bonus.ToString(),
                ["会心ダメ期待値"] = ed.Crit.ExpectedCritDmg.ToString(),
                ["熟知"] = data.elemental_mastery.ToString(),
                ["元素反応"] = elementalReaction.result_er_multi.ToString(),
                ["耐性ダウン計算後"] = ed.Res.ToString(),
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
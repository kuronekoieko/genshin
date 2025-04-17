using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace so
{
    public class Albedo : BaseCharacterSO
    {
        // スキル Lv9
        float[] skillPerArray = { 2.27f };


        public override Dictionary<string, string> CalcDmg(Data data)
        {

            var expectedDamage_skill = ExpectedDamage.Single(data, AttackType.Skill, skillPerArray[0], referenceStatus: ReferenceStatus.Def);

            var ed = expectedDamage_skill;

            Dictionary<string, string> result = new()
            {
                ["武器"] = data.Weapon.DisplayName,
                ["聖遺物セット"] = data.ArtSetData.name,
                ["聖遺物メイン"] = data.ArtMainData.name,
                ["バフキャラ"] = data.PartyData.name,
                ["スキル期待値"] = ed.Result.ToString(),
                ["防御力"] = data.def.ToString(),
                ["HP"] = data.hp.ToString(),
                ["バフ"] = ed.DmgBonus.ToString(),
                ["加算"] = ed.DmgAdd.ToString(),
                ["会心ダメ期待値"] = ed.Crit.ExpectedCritDmg.ToString(),
                ["熟知"] = data.elemental_mastery.ToString(),
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
}
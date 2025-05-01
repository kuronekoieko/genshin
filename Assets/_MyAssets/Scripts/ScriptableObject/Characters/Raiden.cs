using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Raiden", menuName = "Scriptable Objects/Raiden")]
public class Raiden : BaseCharacterSO
{
    readonly float _burstMultiplier_first = 7.21f;
    readonly float[] _burstMultiplier_normalAtks = new[] { 0.798f, 0.784f, 0.960f, 0.551f, 0.553f, 1.319f, };

    float skill_burstDmgBonusPerEnergyCost = 0.3f * 0.01f;
    float burst_ganrikiStuckPerMembersEnergyCost = 0.2f;
    float burst_multiplierAddPerGanriki_first = 0.07f;
    float burst_multiplierAddPerGanriki_normalAtks = 0.0131f;
    float ganrikiStuckMax = 60;

    float talent_electroDmgBonusPerERPercentage = 0.4f * 0.01f;

    public override Dictionary<string, string> CalcDmg(Data data)
    {
        data.burst_bonus += skill_burstDmgBonusPerEnergyCost * status.energyCost;
        float energyRechargePercentageOver100 = Mathf.Clamp(data.energy_recharge - 1f, 0, 10000) * 100f;
        float talent_electroDmgBonus = talent_electroDmgBonusPerERPercentage * energyRechargePercentageOver100;
        data.electro_bonus += talent_electroDmgBonus;

        float ganrikiStuck = Mathf.Clamp(data.partyData.MembersEnergyCostSum * burst_ganrikiStuckPerMembersEnergyCost, 0, ganrikiStuckMax);

        float burst_multiplierAdd_first = burst_multiplierAddPerGanriki_first * ganrikiStuck;
        float burstMultiplier_first = _burstMultiplier_first + burst_multiplierAdd_first;

        float burst_multiplierAdd_normalAtks = burst_multiplierAddPerGanriki_normalAtks * ganrikiStuck;
        float[] burstMultiplier_normalAtks = new float[_burstMultiplier_normalAtks.Length];
        for (int i = 0; i < burstMultiplier_normalAtks.Length; i++)
        {
            burstMultiplier_normalAtks[i] += burst_multiplierAdd_normalAtks;
        }


        var ed_burst_first = ExpectedDamage.Single(data, AttackType.Burst, burstMultiplier_first);
        var ed_burst_normal = ExpectedDamage.Sum(data, AttackType.Burst, burstMultiplier_normalAtks);




        Dictionary<string, string> result = new()
        {
            ["武器"] = data.weaponData.DisplayName,
            ["聖遺物セット"] = data.artSetData.name,
            ["聖遺物メイン"] = data.artMainData.name,
            ["バフキャラ"] = data.partyData.name,
            ["爆発一発目期待値"] = ed_burst_first.Result.ToString(true),
            ["爆発通常合計期待値"] = ed_burst_normal.Result.ToString(true),
            ["願力"] = ganrikiStuck.ToString(true),
            ["爆発一発目倍率"] = burstMultiplier_first.ToString(),
            ["爆発一発目倍率アップ"] = burst_multiplierAdd_first.ToString(),
            ["爆発通常倍率アップ"] = burst_multiplierAdd_normalAtks.ToString(),
            ["元チャ"] = data.energy_recharge.ToString(),
            ["100%以上の元チャ[%]"] = energyRechargePercentageOver100.ToString(),
            ["天賦の雷バフ"] = talent_electroDmgBonus.ToString(),
            ["草薙攻撃力%"] = data.kusanagi_atkRate.ToString(),
            ["絶縁爆発バフ"] = data.zetsuen_burstBonus.ToString(),
            ["攻撃力"] = data.atk.ToString(true),
            ["加算"] = ed_burst_first.DmgAdd.ToString(true),
            ["バフ合計"] = ed_burst_first.DmgBonus.ToString(),
            ["バフ共通"] = data.dmg_bonus.ToString(),
            ["元素バフ"] = data.ElementalDmgBonus(ElementType.Electro).ToString(),
            ["バフ爆発"] = data.burst_bonus.ToString(),
            ["耐性ダウン"] = data.res.ToString(),
            ["パーティ元素"] = data.partyData.Log,
            ["耐性ダウン計算後"] = ed_burst_first.Res.ToString(),
            ["熟知"] = data.elemental_mastery.ToString(),
            ["率ダメ"] = ed_burst_first.Crit.RateDmg,
            //["聖遺物組み合わせ"] = data.artSubData.name,
            ["サブステ"] = ed_burst_first.Crit.SubCrit,
            ["スコア"] = data.artSubData.Score.ToString()
        };

        //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));

        return result;
    }
}

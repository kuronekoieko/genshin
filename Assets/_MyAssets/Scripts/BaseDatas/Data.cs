using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Serializable]
public class Data : BaseData
{
    public WeaponData weaponData;
    public PartyData partyData;
    public Status status;
    public Ascend ascend;
    public ArtMainData artMainData;
    public ArtSetData artSetData;
    public ArtSubData artSubData;
    public float BaseAtk => status.baseAtk + weaponData.base_atk;

    public Data(BaseDataSet baseDataSet)
    {
        weaponData = baseDataSet.weaponData;
        artMainData = baseDataSet.artMainData;
        artSetData = baseDataSet.artSetData;
        artSubData = baseDataSet.artSubData;
        partyData = baseDataSet.partyData;
        status = baseDataSet.status;
        ascend = baseDataSet.ascend;

        baseDataSet.SetInstance(this);

        SetFields();
        SetCharaData();
    }


    void SetFields()
    {
        heal_bonus += ascend.heal_bonus;
        hp_rate += ascend.hpPer;
        energy_recharge += status.defaultEnergyRecharge + ascend.energyRecharge;
        elemental_mastery += ascend.elemental_mastery;
        def_rate += ascend.defPer;
        atk_rate += ascend.atkPer;

        pyro_bonus += ascend.pyro_bonus;
        hydro_bonus += ascend.hydro_bonus;
        electro_bonus += ascend.electro_bonus;
        cryo_bonus += ascend.cryo_bonus;
        geo_bonus += ascend.geo_bonus;
        anemo_bonus += ascend.anemo_bonus;
        dendro_bonus += ascend.dendro_bonus;
        physics_bonus += ascend.physics_bonus;

        crit_rate += status.defaultCritRate + ascend.critRate;
        crit_dmg += status.defaultCritDmg + ascend.critDmg;
    }

    void SetCharaData()
    {
        hp = status.baseHp * (1 + hp_rate) + hp;
        def = status.baseDef * (1 + def_rate) + def;

        // 赤角
        var dmgAdd_sekikaku = def * weaponData.sekikaku;
        add_normal_atk += dmgAdd_sekikaku;
        add_charged_atk += dmgAdd_sekikaku;

        // シナバースピンドル
        var dmgAdd_cinnabar = def * weaponData.cinnabar;
        add_skill += dmgAdd_cinnabar;

        // 絶縁
        float zetsuen_burstBonus = Mathf.Clamp(artSetData.zetsuen * energy_recharge, 0, 0.75f);
        burst_bonus += zetsuen_burstBonus;

        // 金メッキ
        int sameElementCount = partyData.members.Count(m => m.ElementType == status.elementType);
        atk_rate += sameElementCount * 0.14f;
        elemental_mastery += (partyData.members.Length - sameElementCount) * 50;

        // 護摩の杖
        atk += hp * weaponData.homa;
        // 赤砂
        atk += elemental_mastery * weaponData.sekisha;
        // 草薙
        float kusanagi_atkRate = Mathf.Clamp((energy_recharge - 1f) * weaponData.kusanagi, 0, weaponData.kusanagi_max);
        atk_rate += kusanagi_atkRate;


        atk += BaseAtk * (1 + atk_rate);
    }





    // public float kusanagi_atkRate { get; private set; }
    // public float zetsuen_burstBonus { get; private set; }

    public void AddAtkRate(float rate)
    {
        atk += BaseAtk * rate;
    }

}

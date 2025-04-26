using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Serializable]
public class Data : BaseData
{
    public BaseDataSet baseDataSet;

    public WeaponData Weapon => baseDataSet.weaponData;
    public ArtMainData ArtMainData => baseDataSet.artMainData;
    public ArtSetData ArtSetData => baseDataSet.artSetData;
    public ArtSubData ArtSubData => baseDataSet.artSubData;
    public PartyData PartyData => baseDataSet.partyData;
    public Status Status => baseDataSet.status;
    public Ascend Ascend => baseDataSet.ascend;
    public float BaseAtk => Status.baseAtk + Weapon.base_atk;

    public Data(BaseDataSet baseDataSet)
    {
        this.baseDataSet = baseDataSet;
        baseDataSet.SetInstance(this);

        SetFields();
        SetCharaData();
    }


    void SetFields()
    {
        heal_bonus += Ascend.heal_bonus;
        hp_rate += Ascend.hpPer;
        energy_recharge += 1 + Ascend.energyRecharge;
        elemental_mastery += Ascend.elemental_mastery;
        def_rate += Ascend.defPer;
        atk_rate += Ascend.atkPer;

        pyro_bonus += Ascend.pyro_bonus;
        hydro_bonus += Ascend.hydro_bonus;
        electro_bonus += Ascend.electro_bonus;
        cryo_bonus += Ascend.cryo_bonus;
        geo_bonus += Ascend.geo_bonus;
        anemo_bonus += Ascend.anemo_bonus;
        dendro_bonus += Ascend.dendro_bonus;
        physics_bonus += Ascend.physics_bonus;

        crit_rate += Status.defaultCritRate + Ascend.critRate;
        crit_dmg += Status.defaultCritDmg + Ascend.critDmg;
    }

    void SetCharaData()
    {
        hp = Status.baseHp * (1 + hp_rate) + hp;
        def = Status.baseDef * (1 + def_rate) + def;

        var dmgAdd_sekikaku = def * Weapon.sekikaku;
        add_normal_atk += dmgAdd_sekikaku;
        add_charged_atk += dmgAdd_sekikaku;

        var dmgAdd_cinnabar = def * Weapon.cinnabar;
        add_skill += dmgAdd_cinnabar;

        var homa_atkAdd = hp * Weapon.homa;
        var sekisa_atkAdd = elemental_mastery * Weapon.sekisha;
        var kusanagi_atkAdd = (energy_recharge - 1) * Weapon.kusanagi;

        atk
            = BaseAtk * (1 + atk_rate)
            + atk
            + homa_atkAdd
            + sekisa_atkAdd
            + kusanagi_atkAdd;
    }

}

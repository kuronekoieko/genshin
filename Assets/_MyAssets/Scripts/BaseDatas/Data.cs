using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Serializable]
public class Data : BaseData
{
    public WeaponData weapon;
    public ArtMainData artMainData;
    public ArtSetData artSetData;
    public ArtSubData artSubData;
    public PartyData partyData;
    public Status status;
    public Ascend ascend;
    public float BaseAtk => status.baseAtk + weapon.base_atk;

    public Data(
        WeaponData weapon,
        ArtifactGroup artifactGroup,
        PartyData partyData,
        Status status,
        Ascend ascend)
    {
        this.weapon = weapon;
        this.artMainData = artifactGroup.artMainData;
        this.artSetData = artifactGroup.artSetData;
        this.artSubData = artifactGroup.artSubData;
        this.partyData = partyData;
        this.status = status;
        this.ascend = ascend;

        var baseDatas = new BaseData[] { weapon, artMainData, artSetData, partyData, artSubData, };
        var baseData = Utils.AddInstances(baseDatas);
        Utils.CopyBaseFields(baseData, this);
        SetFields();
        SetCharaData();
    }


    void SetFields()
    {
        heal_bonus += ascend.heal_bonus;
        hp_rate += ascend.hpPer;
        energy_recharge += 1 + ascend.energyRecharge;
        elemental_mastery += ascend.elemental_mastery;
        def_rate += ascend.defPer;
        atk_rate += ascend.atkPer;
        dmg_bonus += ascend.dmgBonus;
        crit_rate += status.defaultCritRate + ascend.critRate;
        crit_dmg += status.defaultCritDmg + ascend.critDmg;
    }

    void SetCharaData()
    {
        hp = status.baseHp * (1 + hp_rate) + hp;
        def = status.baseDef * (1 + def_rate) + def;

        var dmgAdd_sekikaku = def * weapon.sekikaku;
        add_normal_atk += dmgAdd_sekikaku;
        add_charged_atk += dmgAdd_sekikaku;

        var dmgAdd_cinnabar = def * weapon.cinnabar;
        add_skill += dmgAdd_cinnabar;

        var homa_atkAdd = hp * weapon.homa;
        var sekisa_atkAdd = elemental_mastery * weapon.sekisha;
        var kusanagi_atkAdd = (energy_recharge - 1) * weapon.kusanagi;

        atk
            = BaseAtk * (1 + atk_rate)
            + atk
            + homa_atkAdd
            + sekisa_atkAdd
            + kusanagi_atkAdd;
    }

}

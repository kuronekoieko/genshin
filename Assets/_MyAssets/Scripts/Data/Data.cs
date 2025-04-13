using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Windows;

public class Data : BaseData
{
    public WeaponData weapon;
    public ArtMainData artMainData;
    public ArtSetData artSetData;
    public PartyData partyData;
    public ArtSubData artSub;
    public Status status;
    public Ascend ascend;
    public float BaseAtk => status.baseAtk + weapon.base_atk;

    public Data(
        WeaponData weapon,
        Artifact.ArtifactGroup artifactGroup,
        PartyData partyData,
        Status status,
        Ascend ascend)
    {
        this.weapon = weapon;
        this.artMainData = artifactGroup.artMainData;
        this.artSetData = artifactGroup.artSetData;
        this.partyData = partyData;
        this.artSub = artifactGroup.artSubData;
        this.status = status;
        this.ascend = ascend;

        var baseDatas = new BaseData[] { weapon, artMainData, artSetData, partyData, artSub, };
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

    public bool IsSkip()
    {
        if (artSetData.name == "しめ縄4" && status.notUseShimenawa) return true;

        bool isGakudan = status.weaponType == WeaponType.Catalyst || status.weaponType == WeaponType.Bow;

        if (artSetData.name == "楽団4" && isGakudan == false)
        {
            return true;
        }
        if (artSetData.name == "剣闘士4" && isGakudan == true)
        {
            return true;
        }


        if (artSetData.name == "ファントム4" || artSetData.name == "花海4" || artSetData.name == "辰砂4")
        {
            bool hasSelfHarm = status.hasSelfHarm || partyData.has_self_harm;
            return !hasSelfHarm;
        }

        if (artSetData.name == "劇団4(控え)" && status.isFront)
        {
            return true;
        }
        if (artSetData.name == "劇団4(表)" && !status.isFront)
        {
            return true;
        }
        if (artSetData.name == "深林4" && status.elementType != ElementType.Dendro)
        {
            return true;
        }

        bool isFrozen = partyData.cryo_count > 0 && partyData.hydro_count > 0;

        if (artSetData.name == "氷風4(凍結)" && isFrozen == false)
        {
            return true;
        }
        if (artSetData.name == "氷風4(凍結無し)" && partyData.cryo_count == 0)
        {
            return true;
        }
        if (artSetData.name == "雷4" && partyData.electro_count == 0)
        {
            return true;
        }
        if (artSetData.is_night_soul)
        {
            return !status.isNightSoul;
        }

        if (artMainData.physics_bonus > 0 && status.elementType != ElementType.Physics)
        {
            return true;
        }
        if (artMainData.dmg_bonus > 0 && status.elementType == ElementType.Physics)
        {
            return true;
        }

        bool isSironenn = partyData.members.Count((member) => member.name.Contains("シロネン")) > 0;
        if (isSironenn)
        {
            int eCount = partyData.pyro_count + partyData.cryo_count + partyData.hydro_count + partyData.electro_count;
            if (eCount < 2) return true;
        }

        // TODO:残響

        int skip = base.skip;

        return skip > 0;
    }
}

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

        var baseDatas = new BaseData[] { weapon, artifactGroup.artMainData, artifactGroup.artSetData, partyData, artifactGroup.artSubData, };
        var baseData = Utils.AddInstances(baseDatas);
        Utils.CopyBaseFields(baseData, this);
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

    public float base_atk()
    {
        return status.baseAtk + weapon.base_atk;
    }

    public float heal_bonus()
    {
        return base.heal_bonus + ascend.heal_bonus;
    }

    public float hpPerSum()
    {
        return base.hp_rate + ascend.hpPer;

    }
    public float energy_recharge()
    {
        return base.energy_recharge + ascend.energyRecharge;

    }
    public float elemental_mastery()
    {
        return base.elemental_mastery
        + ascend.elemental_mastery;
    }
    public float def_rate()
    {
        return base.def_rate
        + ascend.defPer;
    }


    public float atk_rate()
    {
        return base.atk_rate
        + ascend.atkPer;
    }


    public float dmg_bonus()
    {
        return base.dmg_bonus
        + ascend.dmgBonus;
    }

    public float crit_rate()
    {
        return base.crit_rate
        + status.defaultCritRate
        + ascend.critRate;
    }

    public float crit_dmg()
    {
        return base.crit_dmg
        + status.defaultCritDmg
        + ascend.critDmg;
    }
}

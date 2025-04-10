using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Data
{
    public WeaponData weapon;
    public ArtMainData artMainData;
    public ArtSetData artSetData;
    public PartyData partyData;
    public ArtSubData artSub;
    public Status status;
    public Ascend ascend;

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

        bool hasSelfHarm = status.hasSelfHarm || partyData.has_self_harm;

        if ((artSetData.name == "ファントム4" ||
             artSetData.name == "花海4" ||
             artSetData.name == "辰砂4") &&
            !hasSelfHarm)
        {
            return true;
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

        // TODO:残響

        int skip = weapon.skip
               + artMainData.skip
               + artSetData.skip
               + partyData.skip
               + artSub.skip;

        return skip > 0;
    }

    public float base_atk()
    {
        return status.baseAtk + weapon.base_atk;
    }

    public float heal_bonus()
    {
        return weapon.heal_bonus
        + artMainData.heal_bonus
        + artSetData.heal_bonus
        + partyData.heal_bonus
        + artSub.heal_bonus
        + ascend.heal_bonus;
    }

    public float hp()
    {
        return weapon.hp
        + artMainData.hp
        + artSetData.hp
        + partyData.hp
        + artSub.hp;
    }
    public float hpPerSum()
    {
        return weapon.hp_rate
        + artMainData.hp_rate
        + artSetData.hp_rate
        + partyData.hp_rate
        + artSub.hp_rate
        + ascend.hpPer;

    }
    public float energy_recharge()
    {
        return weapon.energy_recharge
        + artMainData.energy_recharge
        + artSetData.energy_recharge
        + partyData.energy_recharge
        + artSub.energy_recharge
        + ascend.energyRecharge;

    }
    public float elemental_mastery()
    {
        return weapon.elemental_mastery
        + artMainData.elemental_mastery
        + artSetData.elemental_mastery
        + partyData.elemental_mastery
        + artSub.elemental_mastery
        + ascend.elemental_mastery;
    }
    public float def_rate()
    {
        return weapon.def_rate
        + artMainData.def_rate
        + artSetData.def_rate
        + partyData.def_rate
        + artSub.def_rate
        + ascend.defPer;
    }
    public float def()
    {
        return weapon.def
        + artMainData.def
        + artSetData.def
        + partyData.def
        + artSub.def;
    }
    public float atk_rate()
    {
        return weapon.atk_rate
        + artMainData.atk_rate
        + artSetData.atk_rate
        + partyData.atk_rate
        + artSub.atk_rate
        + ascend.atkPer;
    }
    public float atk()
    {
        return weapon.atk
        + artMainData.atk
        + artSetData.atk
        + partyData.atk
        + artSub.atk;
    }
    public float dmg_bonus()
    {
        return weapon.dmg_bonus
        + artMainData.dmg_bonus
        + artSetData.dmg_bonus
        + partyData.dmg_bonus
        + artSub.dmg_bonus
        + ascend.dmgBonus;
    }

    public float ElementalDmgBonus(ElementType elementType)
    {
        return elementType switch
        {
            ElementType.Pyro => pyro_bonus(),
            ElementType.Hydro => hydro_bonus(),
            ElementType.Electro => electro_bonus(),
            ElementType.Cryo => cryo_bonus(),
            ElementType.Geo => geo_bonus(),
            ElementType.Anemo => anemo_bonus(),
            ElementType.Dendro => dendro_bonus(),
            ElementType.Physics => physics_bonus(),
            _ => 0,
        };
    }

    public float pyro_bonus()
    {
        return weapon.pyro_bonus
        + artMainData.pyro_bonus
        + artSetData.pyro_bonus
        + partyData.pyro_bonus
        + artSub.pyro_bonus;
    }
    public float hydro_bonus()
    {
        return weapon.hydro_bonus
        + artMainData.hydro_bonus
        + artSetData.hydro_bonus
        + partyData.hydro_bonus
        + artSub.hydro_bonus;
    }
    public float electro_bonus()
    {
        return weapon.electro_bonus
        + artMainData.electro_bonus
        + artSetData.electro_bonus
        + partyData.electro_bonus
        + artSub.electro_bonus;
    }
    public float cryo_bonus()
    {
        return weapon.cryo_bonus
        + artMainData.cryo_bonus
        + artSetData.cryo_bonus
        + partyData.cryo_bonus
        + artSub.cryo_bonus;
    }
    public float geo_bonus()
    {
        return weapon.geo_bonus
        + artMainData.geo_bonus
        + artSetData.geo_bonus
        + partyData.geo_bonus
        + artSub.geo_bonus;
    }
    public float anemo_bonus()
    {
        return weapon.anemo_bonus
        + artMainData.anemo_bonus
        + artSetData.anemo_bonus
        + partyData.anemo_bonus
        + artSub.anemo_bonus;
    }
    public float dendro_bonus()
    {
        return weapon.dendro_bonus
        + artMainData.dendro_bonus
        + artSetData.dendro_bonus
        + partyData.dendro_bonus
        + artSub.dendro_bonus;
    }
    public float physics_bonus()
    {
        return weapon.physics_bonus
        + artMainData.physics_bonus
        + artSetData.physics_bonus
        + partyData.physics_bonus
        + artSub.physics_bonus;
    }


    public float normal_atk_bonus()
    {
        return weapon.normal_atk_bonus
        + artMainData.normal_atk_bonus
        + artSetData.normal_atk_bonus
        + partyData.normal_atk_bonus
        + artSub.normal_atk_bonus;
    }
    public float charged_atk_bonus()
    {
        return weapon.charged_atk_bonus
        + artMainData.charged_atk_bonus
        + artSetData.charged_atk_bonus
        + partyData.charged_atk_bonus
        + artSub.charged_atk_bonus;
    }
    public float plugged_atk_bonus()
    {
        return weapon.plugged_atk_bonus
        + artMainData.plugged_atk_bonus
        + artSetData.plugged_atk_bonus
        + partyData.plugged_atk_bonus
        + artSub.plugged_atk_bonus;
    }

    public float skill_bonus()
    {
        return weapon.skill_bonus
        + artMainData.skill_bonus
        + artSetData.skill_bonus
        + partyData.skill_bonus
        + artSub.skill_bonus;
    }
    public float burst_bonus()
    {
        return weapon.burst_bonus
        + artMainData.burst_bonus
        + artSetData.burst_bonus
        + partyData.burst_bonus
        + artSub.burst_bonus;
    }
    public float atk_speed()
    {
        return weapon.atk_speed
        + artMainData.atk_speed
        + artSetData.atk_speed
        + partyData.atk_speed
        + artSub.atk_speed;
    }
    public float crit_rate()
    {
        return weapon.crit_rate
        + artMainData.crit_rate
        + artSetData.crit_rate
        + partyData.crit_rate
        + artSub.crit_rate
        + status.defaultCritRate
        + ascend.critRate;
    }
    public float crit_rate_normal_atk()
    {
        return weapon.crit_rate_normal_atk
        + artMainData.crit_rate_normal_atk
        + artSetData.crit_rate_normal_atk
        + partyData.crit_rate_normal_atk
        + artSub.crit_rate_normal_atk;
    }
    public float crit_rate_charged_atk()
    {
        return weapon.crit_rate_charged_atk
        + artMainData.crit_rate_charged_atk
        + artSetData.crit_rate_charged_atk
        + partyData.crit_rate_charged_atk
        + artSub.crit_rate_charged_atk;
    }
    public float crit_rate_plugged_atk()
    {
        return weapon.crit_rate_plugged_atk
        + artMainData.crit_rate_plugged_atk
        + artSetData.crit_rate_plugged_atk
        + partyData.crit_rate_plugged_atk
        + artSub.crit_rate_plugged_atk;
    }
    public float crit_rate_skill()
    {
        return weapon.crit_rate_skill
        + artMainData.crit_rate_skill
        + artSetData.crit_rate_skill
        + partyData.crit_rate_skill
        + artSub.crit_rate_skill;
    }
    public float crit_rate_burst()
    {
        return weapon.crit_rate_burst
        + artMainData.crit_rate_burst
        + artSetData.crit_rate_burst
        + partyData.crit_rate_burst
        + artSub.crit_rate_burst;
    }

    public float crit_dmg()
    {
        return weapon.crit_dmg
        + artMainData.crit_dmg
        + artSetData.crit_dmg
        + partyData.crit_dmg
        + artSub.crit_dmg
        + status.defaultCritDmg
        + ascend.critDmg;
    }
    public float crit_dmg_burst()
    {
        return weapon.crit_dmg_burst
        + artMainData.crit_dmg_burst
        + artSetData.crit_dmg_burst
        + partyData.crit_dmg_burst
        + artSub.crit_dmg_burst;
    }
    public float crit_dmg_plugged()
    {
        return weapon.crit_dmg_plugged
        + artMainData.crit_dmg_plugged
        + artSetData.crit_dmg_plugged
        + partyData.crit_dmg_plugged
        + artSub.crit_dmg_plugged;
    }
    public float add()
    {
        return weapon.add
        + artMainData.add
        + artSetData.add
        + partyData.add
        + artSub.add;
    }
    public float add_cryo()
    {
        return weapon.add_cryo
        + artMainData.add_cryo
        + artSetData.add_cryo
        + partyData.add_cryo
        + artSub.add_cryo;
    }
    public float add_normal_atk()
    {
        return weapon.add_normal_atk
        + artMainData.add_normal_atk
        + artSetData.add_normal_atk
        + partyData.add_normal_atk
        + artSub.add_normal_atk;
    }

    public float add_charged_atk()
    {
        return weapon.add_charged_atk
        + artMainData.add_charged_atk
        + artSetData.add_charged_atk
        + partyData.add_charged_atk
        + artSub.add_charged_atk;
    }

    public float add_plugged_atk()
    {
        return weapon.add_plugged_atk
        + artMainData.add_plugged_atk
        + artSetData.add_plugged_atk
        + partyData.add_plugged_atk
        + artSub.add_plugged_atk;
    }
    public float add_skill()
    {
        return weapon.add_skill
        + artMainData.add_skill
        + artSetData.add_skill
        + partyData.add_skill
        + artSub.add_skill;
    }

    public float add_burst()
    {
        return weapon.add_burst
        + artMainData.add_burst
        + artSetData.add_burst
        + partyData.add_burst
        + artSub.add_burst;
    }

    public float er_rate()
    {
        return weapon.er_rate
        + artMainData.er_rate
        + artSetData.er_rate
        + partyData.er_rate
        + artSub.er_rate;
    }

    public float er_fixed()
    {
        return weapon.er_fixed
        + artMainData.er_fixed
        + artSetData.er_fixed
        + partyData.er_fixed
        + artSub.er_fixed;
    }
    public float er_aggravate()
    {
        return weapon.er_aggravate
        + artMainData.er_aggravate
        + artSetData.er_aggravate
        + partyData.er_aggravate
        + artSub.er_aggravate;
    }

}

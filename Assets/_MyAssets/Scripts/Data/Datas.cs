using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Datas
{
    public WeaponData weapon;
    public ArtMainData artMain;
    public ArtSetData artSets;
    public PartyData partyData;
    public ArtSubData artSub;
    public Status status;
    public Ascend ascend;

    public float base_atk()
    {
        return status.baseAtk + weapon.base_atk;
    }

    public float heal_bonus()
    {
        return weapon.heal_bonus
        + artMain.heal_bonus
        + artSets.heal_bonus
        + partyData.heal_bonus
        + artSub.heal_bonus
        + ascend.heal_bonus;
    }

    public float hp()
    {
        return weapon.hp
        + artMain.hp
        + artSets.hp
        + partyData.hp
        + artSub.hp;
    }
    public float hpPerSum()
    {
        return weapon.hp_rate
        + artMain.hp_rate
        + artSets.hp_rate
        + partyData.hp_rate
        + artSub.hp_rate
        + ascend.hpPer;

    }
    public float energy_recharge()
    {
        return weapon.energy_recharge
        + artMain.energy_recharge
        + artSets.energy_recharge
        + partyData.energy_recharge
        + artSub.energy_recharge
        + ascend.energyRecharge;

    }
    public float elemental_mastery()
    {
        return weapon.elemental_mastery
        + artMain.elemental_mastery
        + artSets.elemental_mastery
        + partyData.elemental_mastery
        + artSub.elemental_mastery
        + ascend.elemental_mastery;
    }
    public float def_rate()
    {
        return weapon.def_rate
        + artMain.def_rate
        + artSets.def_rate
        + partyData.def_rate
        + artSub.def_rate
        + ascend.defPer;
    }
    public float def()
    {
        return weapon.def
        + artMain.def
        + artSets.def
        + partyData.def
        + artSub.def;
    }
    public float atk_rate()
    {
        return weapon.atk_rate
        + artMain.atk_rate
        + artSets.atk_rate
        + partyData.atk_rate
        + artSub.atk_rate
        + ascend.atkPer;
    }
    public float atk()
    {
        return weapon.atk
        + artMain.atk
        + artSets.atk
        + partyData.atk
        + artSub.atk;
    }
    public float dmg_bonus()
    {
        return weapon.dmg_bonus
        + artMain.dmg_bonus
        + artSets.dmg_bonus
        + partyData.dmg_bonus
        + artSub.dmg_bonus
        + ascend.dmgBonus;
    }

    public float pyro_bonus()
    {
        return weapon.pyro_bonus
        + artMain.pyro_bonus
        + artSets.pyro_bonus
        + partyData.pyro_bonus
        + artSub.pyro_bonus;
    }
    public float hydro_bonus()
    {
        return weapon.hydro_bonus
        + artMain.hydro_bonus
        + artSets.hydro_bonus
        + partyData.hydro_bonus
        + artSub.hydro_bonus;
    }
    public float electro_bonus()
    {
        return weapon.electro_bonus
        + artMain.electro_bonus
        + artSets.electro_bonus
        + partyData.electro_bonus
        + artSub.electro_bonus;
    }
    public float cryo_bonus()
    {
        return weapon.cryo_bonus
        + artMain.cryo_bonus
        + artSets.cryo_bonus
        + partyData.cryo_bonus
        + artSub.cryo_bonus;
    }
    public float geo_bonus()
    {
        return weapon.geo_bonus
        + artMain.geo_bonus
        + artSets.geo_bonus
        + partyData.geo_bonus
        + artSub.geo_bonus;
    }
    public float anemo_bonus()
    {
        return weapon.anemo_bonus
        + artMain.anemo_bonus
        + artSets.anemo_bonus
        + partyData.anemo_bonus
        + artSub.anemo_bonus;
    }
    public float dendro_bonus()
    {
        return weapon.dendro_bonus
        + artMain.dendro_bonus
        + artSets.dendro_bonus
        + partyData.dendro_bonus
        + artSub.dendro_bonus;
    }
    public float physics_bonus()
    {
        return weapon.physics_bonus
        + artMain.physics_bonus
        + artSets.physics_bonus
        + partyData.physics_bonus
        + artSub.physics_bonus;
    }


    public float normal_atk_bonus()
    {
        return weapon.normal_atk_bonus
        + artMain.normal_atk_bonus
        + artSets.normal_atk_bonus
        + partyData.normal_atk_bonus
        + artSub.normal_atk_bonus;
    }
    public float charged_atk_bonus()
    {
        return weapon.charged_atk_bonus
        + artMain.charged_atk_bonus
        + artSets.charged_atk_bonus
        + partyData.charged_atk_bonus
        + artSub.charged_atk_bonus;
    }
    public float plugged_atk_bonus()
    {
        return weapon.plugged_atk_bonus
        + artMain.plugged_atk_bonus
        + artSets.plugged_atk_bonus
        + partyData.plugged_atk_bonus
        + artSub.plugged_atk_bonus;
    }
    
    public float skill_bonus()
    {
        return weapon.skill_bonus
        + artMain.skill_bonus
        + artSets.skill_bonus
        + partyData.skill_bonus
        + artSub.skill_bonus;
    }
    public float burst_bonus()
    {
        return weapon.burst_bonus
        + artMain.burst_bonus
        + artSets.burst_bonus
        + partyData.burst_bonus
        + artSub.burst_bonus;
    }
    public float atk_speed()
    {
        return weapon.atk_speed
        + artMain.atk_speed
        + artSets.atk_speed
        + partyData.atk_speed
        + artSub.atk_speed;
    }
    public float crit_rate()
    {
        return weapon.crit_rate
        + artMain.crit_rate
        + artSets.crit_rate
        + partyData.crit_rate
        + artSub.crit_rate
        + status.defaultCritRate
        + ascend.critRate;
    }
    public float crit_rate_normal_atk()
    {
        return weapon.crit_rate_normal_atk
        + artMain.crit_rate_normal_atk
        + artSets.crit_rate_normal_atk
        + partyData.crit_rate_normal_atk
        + artSub.crit_rate_normal_atk;
    }
    public float crit_rate_charged_atk()
    {
        return weapon.crit_rate_charged_atk
        + artMain.crit_rate_charged_atk
        + artSets.crit_rate_charged_atk
        + partyData.crit_rate_charged_atk
        + artSub.crit_rate_charged_atk;
    }
    public float crit_rate_plugged_atk()
    {
        return weapon.crit_rate_plugged_atk
        + artMain.crit_rate_plugged_atk
        + artSets.crit_rate_plugged_atk
        + partyData.crit_rate_plugged_atk
        + artSub.crit_rate_plugged_atk;
    }
    public float crit_rate_skill()
    {
        return weapon.crit_rate_skill
        + artMain.crit_rate_skill
        + artSets.crit_rate_skill
        + partyData.crit_rate_skill
        + artSub.crit_rate_skill;
    }
    public float crit_rate_burst()
    {
        return weapon.crit_rate_burst
        + artMain.crit_rate_burst
        + artSets.crit_rate_burst
        + partyData.crit_rate_burst
        + artSub.crit_rate_burst;
    }

    public float crit_dmg()
    {
        return weapon.crit_dmg
        + artMain.crit_dmg
        + artSets.crit_dmg
        + partyData.crit_dmg
        + artSub.crit_dmg
        + status.defaultCritDmg
        + ascend.critDmg;

    }
    public float add()
    {
        return weapon.add
        + artMain.add
        + artSets.add
        + partyData.add
        + artSub.add;
    }
    public float add_normal_atk()
    {
        return weapon.add_normal_atk
        + artMain.add_normal_atk
        + artSets.add_normal_atk
        + partyData.add_normal_atk
        + artSub.add_normal_atk;
    }
    
    public float add_plugged_atk()
    {
        return weapon.add_plugged_atk
        + artMain.add_plugged_atk
        + artSets.add_plugged_atk
        + partyData.add_plugged_atk
        + artSub.add_plugged_atk;
    }
    public float add_skill()
    {
        return weapon.add_skill
        + artMain.add_skill
        + artSets.add_skill
        + partyData.add_skill
        + artSub.add_skill;
    }

    public float er_rate()
    {
        return weapon.er_rate
        + artMain.er_rate
        + artSets.er_rate
        + partyData.er_rate
        + artSub.er_rate;
    }

    public float er_fixed()
    {
        return weapon.er_fixed
        + artMain.er_fixed
        + artSets.er_fixed
        + partyData.er_fixed
        + artSub.er_fixed;
    }
    public float er_aggravate()
    {
        return weapon.er_aggravate
        + artMain.er_aggravate
        + artSets.er_aggravate
        + partyData.er_aggravate
        + artSub.er_aggravate;
    }

}

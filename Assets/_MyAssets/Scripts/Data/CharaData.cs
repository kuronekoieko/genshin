public class CharaData : BaseData
{
    public CharaData(Data data)
    {
        heal_bonus = data.heal_bonus();
        hp_rate = data.hpPerSum();
        energy_recharge = 1 + data.energy_recharge();
        elemental_mastery = data.elemental_mastery();
        def_rate = data.def_rate();
        atk_rate = data.atk_rate();
        dmg_bonus = data.dmg_bonus();
        normal_atk_bonus = data.normal_atk_bonus();
        charged_atk_bonus = data.charged_atk_bonus();
        plugged_atk_bonus = data.plugged_atk_bonus();
        skill_bonus = data.skill_bonus();
        burst_bonus = data.burst_bonus();
        atk_speed = data.atk_speed();
        crit_rate = data.crit_rate();
        crit_rate_normal_atk = data.crit_rate_normal_atk();
        crit_rate_charged_atk = data.crit_rate_charged_atk();
        crit_rate_plugged_atk = data.crit_rate_plugged_atk();
        crit_rate_skill = data.crit_rate_skill();
        crit_rate_burst = data.crit_rate_burst();
        crit_dmg = data.crit_dmg();
        crit_dmg_plugged = data.crit_dmg_plugged();
        crit_dmg_burst = data.crit_dmg_burst();
        add = data.add();
        add_normal_atk = data.add_normal_atk();
        add_charged_atk = data.add_charged_atk();
        add_plugged_atk = data.add_plugged_atk();
        add_skill = data.add_skill();
        add_burst = data.add_burst();
        res = data.partyData.res;
    }


}
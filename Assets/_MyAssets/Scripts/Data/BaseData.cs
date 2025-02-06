using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

[Serializable]
public class BaseData
{
    public int skip;
    public string name;
    public float atk;
    public float atk_rate;
    public float def;
    public float def_rate;
    public float hp;
    public float hp_rate;

    public float dmg_bonus;
    public float pyro_bonus;
    public float hydro_bonus;
    public float electro_bonus;
    public float cryo_bonus;
    public float geo_bonus;
    public float anemo_bonus;
    public float dendro_bonus;
    public float physics_bonus;

    public float normal_atk_bonus;
    public float charged_atk_bonus;
    public float plugged_atk_bonus;
    public float skill_bonus;
    public float burst_bonus;
    public float elemental_mastery;
    public float energy_recharge;
    public float crit_rate;
    public float crit_dmg;
    public float crit_rate_normal_atk;
    public float crit_rate_charged_atk;
    public float crit_rate_plugged_atk;
    public float crit_rate_skill;
    public float crit_rate_burst;
    public float crit_dmg_burst;
    public float atk_speed;
    public float heal_bonus;
    public float res;
    public float add;
    public float add_cryo;
    public float add_normal_atk;
    public float add_charged_atk;
    public float add_plugged_atk;
    public float add_skill;
    public float add_burst;
    public bool has_self_harm;
    public float er_rate;
    public float er_fixed;
    public float er_aggravate;


    public BaseData DeepCopy()
    {
        string json = JsonConvert.SerializeObject(this);
        return JsonConvert.DeserializeObject<BaseData>(json);
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArtMainData : BaseData
{
    readonly float artMainHpFixed = 4780;
    readonly float artMainAtkFixed = 311;
    readonly float artMainAtkPer = 0.466f;
    readonly float artMainDefPer = 0.583f;
    readonly float artMainElementalDmgPer = 0.466f;
    readonly float artMainPhysicsDmgPer = 0.583f;
    readonly float artMainHPPer = 0.466f;
    readonly float artMainCritRate = 0.311f;
    readonly float artMainCritDmg = 0.622f;
    readonly float artMainElementalMastery = 187;
    readonly float artMainEnergyRecharge = 0.518f;
    readonly float artMainHealPer = 0.359f;


    public ArtMainData(ArtMainHash artMainHash)
    {
        name = artMainHash.DisplayName;
        atk = artMainAtkFixed;
        atk_rate = artMainHash.GetPartCount("攻撃%") * artMainAtkPer;
        def_rate = artMainHash.GetPartCount("防御%") * artMainDefPer;
        hp = artMainHpFixed;
        hp_rate = artMainHash.GetPartCount("HP%") * artMainHPPer;
        elemental_mastery = artMainHash.GetPartCount("元素熟知") * artMainElementalMastery;
        energy_recharge = artMainHash.GetPartCount("元チャ") * artMainEnergyRecharge;
        pyro_bonus = artMainHash.GetPartCount("炎バフ") * artMainElementalDmgPer;
        hydro_bonus = artMainHash.GetPartCount("水バフ") * artMainElementalDmgPer;
        electro_bonus = artMainHash.GetPartCount("雷バフ") * artMainElementalDmgPer;
        cryo_bonus = artMainHash.GetPartCount("氷バフ") * artMainElementalDmgPer;
        geo_bonus = artMainHash.GetPartCount("岩バフ") * artMainElementalDmgPer;
        anemo_bonus = artMainHash.GetPartCount("風バフ") * artMainElementalDmgPer;
        dendro_bonus = artMainHash.GetPartCount("草バフ") * artMainElementalDmgPer;
        physics_bonus = artMainHash.GetPartCount("物理バフ") * artMainPhysicsDmgPer;
        crit_rate = artMainHash.GetPartCount("会心率") * artMainCritRate;
        crit_dmg = artMainHash.GetPartCount("会心ダメージ") * artMainCritDmg;
        heal_bonus = artMainHash.GetPartCount("治癒効果") * artMainHealPer;

    }


}

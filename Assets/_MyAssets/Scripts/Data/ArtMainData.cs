using System.Collections;
using System.Collections.Generic;
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

    public ArtMainData(ArtMainCombined artMainCombined)
    {
        name = artMainCombined.displayName;
        atk = artMainAtkFixed;
        atk_rate = artMainCombined.artMainDictionaries["攻撃%"] * artMainAtkPer;
        def_rate = artMainCombined.artMainDictionaries["防御%"] * artMainDefPer;
        hp = artMainHpFixed;
        hp_rate = artMainCombined.artMainDictionaries["HP%"] * artMainHPPer;
        elemental_mastery = artMainCombined.artMainDictionaries["元素熟知"] * artMainElementalMastery;
        energy_recharge = artMainCombined.artMainDictionaries["元チャ"] * artMainEnergyRecharge;
        pyro_bonus = artMainCombined.artMainDictionaries["炎バフ"] * artMainElementalDmgPer;
        hydro_bonus = artMainCombined.artMainDictionaries["水バフ"] * artMainElementalDmgPer;
        electro_bonus = artMainCombined.artMainDictionaries["雷バフ"] * artMainElementalDmgPer;
        cryo_bonus = artMainCombined.artMainDictionaries["氷バフ"] * artMainElementalDmgPer;
        geo_bonus = artMainCombined.artMainDictionaries["岩バフ"] * artMainElementalDmgPer;
        anemo_bonus = artMainCombined.artMainDictionaries["風バフ"] * artMainElementalDmgPer;
        dendro_bonus = artMainCombined.artMainDictionaries["草バフ"] * artMainElementalDmgPer;
        physics_bonus = artMainCombined.artMainDictionaries["物理バフ"] * artMainPhysicsDmgPer;
        crit_rate = artMainCombined.artMainDictionaries["会心率"] * artMainCritRate;
        crit_dmg = artMainCombined.artMainDictionaries["会心ダメージ"] * artMainCritDmg;
        heal_bonus = artMainCombined.artMainDictionaries["治癒効果"] * artMainHealPer;

        /*
                if (characterElementType == ElementType.Physics)
                {
                    dmg_bonus = artMainCombined.artMainDictionaries["元素バフ"] * artMainPhysicsBuffPer;
                }
                else
                {
                    dmg_bonus = artMainCombined.artMainDictionaries["元素バフ"] * artMainBuffPer;
                }*/
    }
}

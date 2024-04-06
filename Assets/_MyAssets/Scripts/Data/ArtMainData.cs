using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtMainData : BaseData
{
    readonly float artMainHpFixed = 4780;
    readonly float artMainAtkFixed = 311;
    readonly float artMainAtkPer = 0.466f;
    readonly float artMainDefPer = 0.583f;
    readonly float artMainBuffPer = 0.466f;
    readonly float artMainPhysicsBuffPer = 0.583f;
    readonly float artMainHPPer = 0.466f;
    readonly float artMainCritRate = 0.311f;
    readonly float artMainCritDmg = 0.622f;
    readonly float artMainElementalMastery = 187;
    readonly float artMainEnergyRecharge = 0.518f;
    readonly float artMainHealPer = 0.359f;

    public ArtMainData(Artifacts_Main.ArtMainCount artMainCount)
    {
        name = artMainCount.displayName;
        atk = artMainAtkFixed;
        atk_rate = artMainCount.artMainDictionaries["攻撃%"] * artMainAtkPer;
        def_rate = artMainCount.artMainDictionaries["防御%"] * artMainDefPer;
        hp = artMainHpFixed;
        hp_rate = artMainCount.artMainDictionaries["HP%"] * artMainHPPer;
        elemental_mastery = artMainCount.artMainDictionaries["元素熟知"] * artMainElementalMastery;
        energy_recharge = artMainCount.artMainDictionaries["元チャ"] * artMainEnergyRecharge;
        dmg_bonus = artMainCount.artMainDictionaries["元素バフ"] * artMainBuffPer;
        physics_bonus = artMainCount.artMainDictionaries["物理バフ"] * artMainPhysicsBuffPer;
        crit_rate = artMainCount.artMainDictionaries["会心率"] * artMainCritRate;
        crit_dmg = artMainCount.artMainDictionaries["会心ダメージ"] * artMainCritDmg;
        heal_bonus = artMainCount.artMainDictionaries["治癒効果"] * artMainHealPer;

        /*
                if (characterElementType == ElementType.Physics)
                {
                    dmg_bonus = artMainCount.artMainDictionaries["元素バフ"] * artMainPhysicsBuffPer;
                }
                else
                {
                    dmg_bonus = artMainCount.artMainDictionaries["元素バフ"] * artMainBuffPer;
                }*/
    }
}

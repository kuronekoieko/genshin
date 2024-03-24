using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class BaseCharacter
{

    public abstract Dictionary<string, string> CalcDmg(Datas datas, CharacterSO characterSO);

    protected float GetExpectedDamage(float atk, float talentRate, float dmgAdd, float dmgBonus, float expectedCritDmg, float res, float elementalReaction)
    {
        float dmg = 0;

        dmg += (atk * talentRate + dmgAdd) * (1 + dmgBonus) * expectedCritDmg * res * elementalReaction;

        return dmg;
    }

    protected float GetExpectedDamageSum(float atk, float[] talentRates, float dmgAdd, float dmgBonus, float expectedCritDmg, float res, float elementalReaction)
    {
        float dmg = 0;

        for (int i = 0; i < talentRates.Length; i++)
        {
            dmg += (atk * talentRates[i] + dmgAdd) * (1 + dmgBonus) * expectedCritDmg * res * elementalReaction;
        }

        return dmg;
    }

    protected float GetElementalRes(float decreasingRes)
    {
        float enemyElementalRes = 0.1f + decreasingRes;
        float elementalRes = 1 / (4 * enemyElementalRes + 1);
        if (enemyElementalRes < 0.75f)
            elementalRes = 1 - enemyElementalRes;
        if (enemyElementalRes < 0)
            elementalRes = 1 - enemyElementalRes / 2;
        return elementalRes;
    }

}

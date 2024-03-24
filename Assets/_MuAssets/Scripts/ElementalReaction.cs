using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ElementalReaction
{
    private const int ReactConstLv90 = 1446;

    public static float Aggravate(float elementalMastery, float reactionBonus)
    {
        float reactionCoef = 1.15f;
        return QuickenOrAggravate(elementalMastery, reactionBonus, reactionCoef);
    }

    public static float Quicken(float elementalMastery, float reactionBonus)
    {
        float reactionCoef = 1.25f;
        return QuickenOrAggravate(elementalMastery, reactionBonus, reactionCoef);
    }

    public static float QuickenOrAggravate(float elementalMastery, float reactionBonus, float reactionCoef)
    {
        float elementalMasteryDmgBonus = 5 * elementalMastery / (elementalMastery + 1200);
        float elementalReaction = reactionCoef * ReactConstLv90 * (1 + elementalMasteryDmgBonus + reactionBonus);
        return elementalReaction;
    }

    public static float VaporizeForPyro(float elementalMastery, float reactionBonus)
    {
        float reactionCoef = 1.5f;
        return VaporizeOrMelt(elementalMastery, reactionBonus, reactionCoef);
    }

    public static float VaporizeForHydro(float elementalMastery, float reactionBonus)
    {
        float reactionCoef = 2.0f;
        return VaporizeOrMelt(elementalMastery, reactionBonus, reactionCoef);
    }

    public static float MeltForCryo(float elementalMastery, float reactionBonus)
    {
        float reactionCoef = 1.5f;
        return VaporizeOrMelt(elementalMastery, reactionBonus, reactionCoef);
    }

    public static float MeltForPyro(float elementalMastery, float reactionBonus)
    {
        float reactionCoef = 2.0f;
        return VaporizeOrMelt(elementalMastery, reactionBonus, reactionCoef);
    }

    private static float VaporizeOrMelt(float elementalMastery, float reactionBonus, float reactionCoef)
    {
        float elementalMasteryDmgBonus = 25 * elementalMastery / (9 * (elementalMastery + 1400));
        float elementalReaction = reactionCoef * (1 + elementalMasteryDmgBonus + reactionBonus);
        return elementalReaction;
    }
}

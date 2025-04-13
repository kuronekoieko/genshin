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


    public static ElementalReactionType GetElementalReactionType(ElementType from, ElementType to)
    {
        if (from == to || from == ElementType.None || to == ElementType.None)
            return ElementalReactionType.None;

        if (from == ElementType.Physics || to == ElementType.Physics)
            return ElementalReactionType.None;

        // Vaporize (蒸発)
        if ((from == ElementType.Pyro && to == ElementType.Hydro) ||
            (from == ElementType.Hydro && to == ElementType.Pyro))
            return ElementalReactionType.Vaporize;

        // Melt (溶解)
        if ((from == ElementType.Pyro && to == ElementType.Cryo) ||
            (from == ElementType.Cryo && to == ElementType.Pyro))
            return ElementalReactionType.Melt;

        // Overloaded (過負荷)
        if ((from == ElementType.Pyro && to == ElementType.Electro) ||
            (from == ElementType.Electro && to == ElementType.Pyro))
            return ElementalReactionType.Overloaded;

        // Electro-Charged (感電)
        if ((from == ElementType.Hydro && to == ElementType.Electro) ||
            (from == ElementType.Electro && to == ElementType.Hydro))
            return ElementalReactionType.ElectroCharged;

        // Frozen (凍結)
        if ((from == ElementType.Hydro && to == ElementType.Cryo) ||
            (from == ElementType.Cryo && to == ElementType.Hydro))
            return ElementalReactionType.Frozen;

        // Superconduct (超電導)
        if ((from == ElementType.Cryo && to == ElementType.Electro) ||
            (from == ElementType.Electro && to == ElementType.Cryo))
            return ElementalReactionType.Superconduct;

        // Swirl (拡散)
        if (from == ElementType.Anemo && IsSwirlOrCrystalizeTarget(to))
            return ElementalReactionType.Swirl;

        // Crystalize (結晶)
        if (from == ElementType.Geo && IsSwirlOrCrystalizeTarget(to))
            return ElementalReactionType.Crystalize;

        // Burning (燃焼)
        if ((from == ElementType.Dendro && to == ElementType.Pyro) ||
            (to == ElementType.Dendro && from == ElementType.Pyro))
            return ElementalReactionType.Burning;

        // Bloom (開花)
        if ((from == ElementType.Dendro && to == ElementType.Hydro) ||
            (to == ElementType.Dendro && from == ElementType.Hydro))
            return ElementalReactionType.Bloom;

        // Quicken (激化)
        if ((from == ElementType.Dendro && to == ElementType.Electro) ||
            (to == ElementType.Dendro && from == ElementType.Electro))
            return ElementalReactionType.Quicken;

        // どの反応にも該当しない場合
        return ElementalReactionType.None;
    }
    private static bool IsSwirlOrCrystalizeTarget(ElementType element)
    {
        return element == ElementType.Pyro ||
               element == ElementType.Hydro ||
               element == ElementType.Electro ||
               element == ElementType.Cryo;
    }



}

public enum ElementalReactionType
{
    None,
    Vaporize,     // 蒸発 (Pyro + Hydro)
    Melt,         // 溶解 (Pyro + Cryo)
    Overloaded,   // 過負荷 (Pyro + Electro)
    ElectroCharged, // 感電 (Electro + Hydro)
    Frozen,       // 凍結 (Hydro + Cryo)
    Superconduct, // 超電導 (Cryo + Electro)
    Swirl,        // 拡散 (Anemo + 他元素)
    Crystalize,   // 結晶 (Geo + 他元素)
    Burning,      // 燃焼 (Dendro + Pyro)
    Bloom,        // 開花 (Dendro + Hydro)
    Quicken,      // 激化 (Dendro + Electro)
}

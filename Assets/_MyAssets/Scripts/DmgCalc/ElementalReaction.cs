using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalReaction
{
    readonly Data data;
    public float result_er_add = 0;
    public float result_er_multi = 1;

    public ElementalReaction()
    {
        // 元素反応無しの場合
    }

    public ElementalReaction(ElementType from, ElementType to, Data data)
    {
        if (data.PartyData.ElementCounts[to] == 0)
        {
            return;
        }

        this.data = data;
        ElementalReactionType elementalReactionType = GetElementalReactionType(from, to);
        HandleElementalReaction(elementalReactionType, from, to);
    }

    public void HandleElementalReaction(ElementalReactionType reactionType, ElementType from, ElementType to)
    {
        switch (reactionType)
        {
            case ElementalReactionType.None:
                // 反応なしの処理
                break;
            case ElementalReactionType.Vaporize:
                // 蒸発 (Pyro + Hydro)
                result_er_multi = VaporizeOrMelt(data.er_rate, from, to);
                break;

            case ElementalReactionType.Melt:
                result_er_multi = VaporizeOrMelt(data.er_rate, from, to);
                // 溶解 (Pyro + Cryo)
                break;

            case ElementalReactionType.Overloaded:
                // 過負荷 (Pyro + Electro)
                break;

            case ElementalReactionType.ElectroCharged:
                // 感電 (Electro + Hydro)
                break;

            case ElementalReactionType.Frozen:
                // 凍結 (Hydro + Cryo)
                break;

            case ElementalReactionType.Superconduct:
                // 超電導 (Cryo + Electro)
                break;

            case ElementalReactionType.Swirl:
                // 拡散 (Anemo + 他元素)
                break;

            case ElementalReactionType.Crystalize:
                // 結晶 (Geo + 他元素)
                break;

            case ElementalReactionType.Burning:
                // 燃焼 (Dendro + Pyro)
                break;

            case ElementalReactionType.Bloom:
                // 開花 (Dendro + Hydro)
                break;
            case ElementalReactionType.Quicken:
                // 激化 (Dendro + Electro)
                result_er_add = Quicken(data.er_aggravate);
                break;
            case ElementalReactionType.Aggravate:
                // 超激化 (Dendro + Electro)
                result_er_add = Aggravate(data.er_aggravate);
                break;
            default:
                // 想定外の値の処理
                break;
        }
    }

    private const int ReactConstLv90 = 1446;

    float Aggravate(float reactionBonus)
    {
        float reactionCoef = 1.15f;
        return QuickenOrAggravate(reactionBonus, reactionCoef);
    }

    float Quicken(float reactionBonus)
    {
        float reactionCoef = 1.25f;
        return QuickenOrAggravate(reactionBonus, reactionCoef);
    }

    float QuickenOrAggravate(float reactionBonus, float reactionCoef)
    {
        float elementalMasteryDmgBonus = 5 * data.elemental_mastery / (data.elemental_mastery + 1200);
        float elementalReaction = reactionCoef * ReactConstLv90 * (1 + elementalMasteryDmgBonus + reactionBonus);
        return elementalReaction;
    }

    private float VaporizeOrMelt(float reactionBonus, ElementType from, ElementType to)
    {
        float reactionCoef;
        if ((from == ElementType.Pyro && to == ElementType.Hydro) || (from == ElementType.Cryo && to == ElementType.Pyro))
        {
            reactionCoef = 1.5f;
        }
        else
        {
            reactionCoef = 2.0f;
        }

        float elementalMasteryDmgBonus = 25 * data.elemental_mastery / (9 * (data.elemental_mastery + 1400));
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
    static bool IsSwirlOrCrystalizeTarget(ElementType element)
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
    Aggravate,//超激化
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crit
{
    float CritRate;
    float CritDmg;
    float SubCritRate;
    float SubCritDmg;
    public float ExpectedCritDmg { get => (1 + CritDmg) * CritRate + 1 - CritRate; }
    // この３つ重い
    public string RateDmg { get => $"{CritRate:F2}:{CritDmg:F2}"; }
    public string CritProportion { get => $"{(CritDmg / CritRate):F2}"; }
    public string SubCrit { get => $"{SubCritRate:F2}:{SubCritDmg:F2}"; }

    public Crit(float critRate, float critDmg, ArtSubData artSub)
    {
        if (artSub.Exist == false)
        {
            (SubCritRate, SubCritDmg) = GetSubCrits(critRate, critDmg, artSub.Score);
            critRate += SubCritRate;
            critDmg += SubCritDmg;
        }
        else
        {
            SubCritRate = artSub.crit_rate;
            SubCritDmg = artSub.crit_dmg;
        }
        CritRate = Mathf.Clamp01(critRate);
        CritDmg = critDmg;
    }

    (float, float) GetSubCrits(float critRate, float critDmg, float score)
    {
        float subCritRate = (score - 2f * critRate + critDmg) / 4f;
        subCritRate = Mathf.Clamp(subCritRate, 0, score / 2f);
        float subCritDmg = score - 2f * subCritRate;
        return (subCritRate, subCritDmg);
    }
}

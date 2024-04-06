using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crit
{
    float critRate;
    float critDmg;
    float subCritRate;
    float subCritDmg;
    public float ExpectedCritDmg { get; }
    // この３つ重い
    public string RateDmg { get => $"{critRate:F2}:{critDmg:F2}"; }
    public string CritProportion { get => $"{(critDmg / critRate):F2}"; }
    public string SubCrit { get => $"{subCritRate:F2}:{subCritDmg:F2}"; }

    public Crit(float critRate, float critDmg, ArtSubData artSub)
    {



        if (artSub.Exist == false)
        {
            (subCritRate, subCritDmg) = GetSubCrits(critRate, critDmg, artSub.Score);
            critRate += subCritRate;
            critDmg += subCritDmg;
        }
        else
        {
            subCritRate = artSub.crit_rate;
            subCritDmg = artSub.crit_dmg;
            critRate = Mathf.Clamp01(critRate);
        }


        float expectedCritDmg = (1 + critDmg) * critRate + 1 - critRate;

        this.critRate = critRate;
        this.critDmg = critDmg;

        ExpectedCritDmg = expectedCritDmg;
    }

    (float, float) GetSubCrits(float critRate, float critDmg, float score)
    {
        float subCritRate = (score - 2f * critRate + critDmg) / 4f;
        subCritRate = Mathf.Clamp(subCritRate, -critRate, 1 - critRate);
        float subCritDmg = score - 2f * subCritRate;
        return (subCritRate, subCritDmg);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crit
{
    public float CritRate { get; }
    public float CritDmg { get; }
    public float SubCritRate { get; }
    public float SubCritDmg { get; }
    public float ExpectedCritDmg { get; }
    public string RateDmg { get; }
    public string CritProportion { get; }
    public string SubCrit { get; }

    public Crit(float critRate, float critDmg, ArtSubData artSub)
    {
        float subCritRate;
        float subCritDmg;

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
        }

        if (critRate > 1) critRate = 1;
        if (critRate < 0) critRate = 0;

        float expectedCritDmg = (1 + critDmg) * critRate + 1 - critRate;

        string rateDmg = $"{critRate:F2}:{critDmg:F2}";
        string critProportion = $"{(critDmg / critRate):F2}";
        string subCrit = $"{subCritRate:F2}:{subCritDmg:F2}";


        CritRate = critRate;
        CritDmg = critDmg;
        SubCritRate = subCritRate;
        SubCritDmg = subCritDmg;
        ExpectedCritDmg = expectedCritDmg;
        RateDmg = rateDmg;
        CritProportion = critProportion;
        SubCrit = subCrit;
    }

    (float, float) GetSubCrits(float critRate, float critDmg, float score)
    {
        float subCritRate = (score - 2 * critRate + critDmg) / 4;
        if (critRate + subCritRate >= 1) subCritRate = 1 - critRate;
        if (subCritRate < 0) subCritRate = 0;  // 下限
        if (subCritRate > score / 2) subCritRate = score / 2;    // 上限
        float subCritDmg = score - 2 * subCritRate;


        return (subCritRate, subCritDmg);
    }
}

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
    static bool isSub = false;

    public Crit(float critRate, float critDmg, float subCritRate, float subCritDmg, float expectedCritDmg, string rateDmg, string critProportion, string subCrit)
    {
        CritRate = critRate;
        CritDmg = critDmg;
        SubCritRate = subCritRate;
        SubCritDmg = subCritDmg;
        ExpectedCritDmg = expectedCritDmg;
        RateDmg = rateDmg;
        CritProportion = critProportion;
        SubCrit = subCrit;
    }

    public static Crit GetCrit(float critRate, float critDmg, object artSub)
    {
        (float subCritRate, float subCritDmg) = GetSubCrits(critRate, critDmg, artSub, 1.6f);

        critRate += subCritRate;
        critDmg += subCritDmg;

        if (critRate > 1) critRate = 1;
        if (critRate < 0) critRate = 0;

        float expectedCritDmg = (1 + critDmg) * critRate + 1 - critRate;

        string rateDmg = $"{critRate:F2}:{critDmg:F2}";
        string critProportion = $"{(critDmg / critRate):F2}";
        string subCrit = $"{subCritRate:F2}:{subCritDmg:F2}";

        return new Crit(critRate, critDmg, subCritRate, subCritDmg, expectedCritDmg, rateDmg, critProportion, subCrit);
    }

    public static (float, float) GetSubCrits(float critRate, float critDmg, object artSub, float score)
    {
        float subCritRate = 0;
        float subCritDmg = 0;

        if (isSub)
        {
            // subCritRate = Convert.ToSingle(((dynamic)artSub)["会心率"]);
            // subCritDmg = Convert.ToSingle(((dynamic)artSub)["会心ダメージ"]);
        }
        else
        {
            subCritRate = (score - 2 * critRate + critDmg) / 4;
            if (critRate + subCritRate >= 1) subCritRate = 1 - critRate;
            if (subCritRate < 0) subCritRate = 0;  // 下限
            if (subCritRate > score / 2) subCritRate = score / 2;    // 上限
            subCritDmg = score - 2 * subCritRate;
        }

        return (subCritRate, subCritDmg);
    }
}

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
}

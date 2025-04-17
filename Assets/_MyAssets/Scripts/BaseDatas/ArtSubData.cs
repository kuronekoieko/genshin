using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ArtSubData : BaseData
{
    public float Score { get; private set; }
    public bool Exist { get; private set; }

    public ArtSubData(ArtifactData artifactData, float score = 1.6f)
    {
        if (artifactData == null)
        {
            Exist = false;
            Score = score;
            return;
        }
        Exist = true;

        name = artifactData.id;
        crit_rate = artifactData.crit_rate * 0.01f;
        crit_dmg = artifactData.crit_dmg * 0.01f;
        hp = artifactData.hp;
        hp_rate = artifactData.hp_rate * 0.01f;
        atk = artifactData.atk;
        atk_rate = artifactData.atk_rate * 0.01f;
        def = artifactData.def;
        def_rate = artifactData.def_rate * 0.01f;
        energy_recharge = artifactData.energy_recharge * 0.01f;
        elemental_mastery = artifactData.elemental_mastery;

        Score = crit_rate * 2 + crit_dmg + atk_rate;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ArtSubData : BaseData
{
    public float score;

    public ArtSubData(ArtifactData artifactData)
    {
        name = artifactData.id;
        skip = artifactData.skip;
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

        score = artifactData.crit_rate * 2 + artifactData.crit_dmg + atk_rate;
    }


}

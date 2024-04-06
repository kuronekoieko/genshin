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
        crit_rate = artifactData.crit_rate * 0.001f;
        crit_dmg = artifactData.crit_dmg * 0.001f;
        hp = artifactData.hp;
        hp_rate = artifactData.hp_rate * 0.001f;
        atk = artifactData.atk;
        atk_rate = artifactData.atk_rate * 0.001f;
        def = artifactData.def;
        def_rate = artifactData.def_rate * 0.001f;
        energy_recharge = artifactData.energy_recharge * 0.001f;
        elemental_mastery = artifactData.elemental_mastery;
    }


}

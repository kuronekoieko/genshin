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
        crit_rate = artifactData.crit_rate;
        crit_dmg = artifactData.crit_dmg;
        hp = artifactData.hp;
        hp_rate = artifactData.hp_rate;
        atk = artifactData.atk;
        atk_rate = artifactData.atk_rate;
        def = artifactData.def;
        def_rate = artifactData.def_rate;
        energy_recharge = artifactData.energy_recharge;
        elemental_mastery = artifactData.elemental_mastery;
    }


}

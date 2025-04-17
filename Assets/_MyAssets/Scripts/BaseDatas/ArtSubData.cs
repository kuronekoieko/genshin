using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ArtSubData : BaseData
{
    public float Score { get; private set; }
    public bool Exist { get; private set; }
    readonly public ArtifactData flower, plume, sands, goblet, circlet;
    public ArtifactData[] ArtifactCombination { get; private set; }


    public ArtSubData(ArtifactData flower, ArtifactData plume, ArtifactData sands, ArtifactData goblet, ArtifactData circlet)
    {
        this.flower = flower;
        this.plume = plume;
        this.sands = sands;
        this.goblet = goblet;
        this.circlet = circlet;

        Exist = true;

        ArtifactCombination = new[] { flower, plume, sands, goblet, circlet };

        ArtifactData artifactData = FastInstanceAdder.AddInstances(ArtifactCombination);

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

        Score = crit_rate * 2f + crit_dmg + atk_rate;
    }

    public ArtSubData(float score = 1.6f)
    {
        this.flower = new();
        this.plume = new();
        this.sands = new();
        this.goblet = new();
        this.circlet = new();

        Exist = false;
        Score = score;
    }


    public ArtMainData GetArtMainData()
    {
        ArtMainHash artMainHash = new(new string[] { sands.art_main, goblet.art_main, circlet.art_main });
        return new(artMainHash);
    }


}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Artifact
{


    public static List<ArtifactGroup> GetFixedScoreArtifactGroups(ArtSetData[] csvArtSetDatas, ArtMainHeader header, float score)
    {

        List<ArtifactGroup> artifactGroups = new();

        // Debug.Log("not sub");
        var artifacts_Main = new Artifacts_Main(header);
        var artMainDatas = artifacts_Main.GetArtMainDatas();
        var artSetDatas = Artifacts_Set.GetArtSetDatas(csvArtSetDatas).ToArray();

        foreach (var artSets in artSetDatas)
        {
            foreach (var artMain in artMainDatas)
            {
                ArtifactGroup artifactGroup = new()
                {
                    artSetData = artSets,
                    artMainData = artMain,
                    artSubData = new ArtSubData(score),
                };
                artifactGroups.Add(artifactGroup);
            }
        }
        return artifactGroups;
    }

    public static List<ArtifactGroup> GetSubArtifactGroups(ArtSetData[] ArtSetDatas_notSkipped, ArtifactData[] artifactDatas, ArtifactConfig artifactConfig)
    {
        List<ArtifactGroup> artifactGroups = Artifacts_Sub.GetSubArtifactGroups(ArtSetDatas_notSkipped, artifactDatas, artifactConfig);

        return artifactGroups;
    }

}
public class ArtifactGroup
{
    public ArtMainData artMainData = null;
    public ArtSetData artSetData = null;
    public ArtSubData artSubData = null;
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Artifact
{


    public static List<ArtifactGroup> GetFixedScoreArtifactGroups(ArtSetData[] csvArtSetDatas, Test test)
    {

        List<ArtifactGroup> artifactGroups = new();

        // Debug.Log("not sub");
        Artifacts_Main artifacts_Main = new(test);
        var artMainDatas = artifacts_Main.GetArtMainDatas();
        var artSetDatas = Artifacts_Set.GetArtSetDatas(csvArtSetDatas).ToArray();


        //var artSetDatas = CSVManager.artSetDatas.Where(artSetData => artSetData.skip != 1).ToArray();

        foreach (var artSets in artSetDatas)
        {
            foreach (var artMain in artMainDatas)
            {
                ArtifactGroup artifactGroup = new()
                {
                    artSetData = artSets,
                    artMainData = artMain,
                    artSubData = new ArtSubData(null),
                };
                artifactGroups.Add(artifactGroup);
            }
        }
        return artifactGroups;
    }

    public static List<ArtifactGroup> GetSubArtifactGroups(ArtSetData[] ArtSetDatas_notSkipped, ArtifactData[] artifactDatas)
    {
        List<ArtifactGroup> artifactGroups = Artifacts_Sub.GetSubArtifactGroups(ArtSetDatas_notSkipped, artifactDatas);

        return artifactGroups;
    }

}
public class ArtifactGroup
{
    public ArtMainData artMainData = null;
    public ArtSetData artSetData = null;
    public ArtSubData artSubData = null;
}
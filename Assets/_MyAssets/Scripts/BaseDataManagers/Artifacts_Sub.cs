using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Artifacts_Sub
{
    public static List<ArtifactGroup> GetSubArtifactGroups(ArtSetData[] ArtSetDatas_notSkipped, ArtifactData[] artifactDatas, ArtifactConfig artifactConfig)
    {
        List<ArtifactGroup> artifactGroups = new();
        var flowerList = artifactDatas.Where(artifactData => artifactData.part == "花").ToList();
        var plumeList = artifactDatas.Where(artifactData => artifactData.part == "羽").ToList();
        var sandsList = artifactDatas.Where(artifactData => artifactData.part == "時計").ToList();
        var gobletList = artifactDatas.Where(artifactData => artifactData.part == "杯").ToList();
        var circletList = artifactDatas.Where(artifactData => artifactData.part == "冠").ToList();


        foreach (var flower in flowerList)
        {
            foreach (var plume in plumeList)
            {
                foreach (var sands in sandsList)
                {
                    foreach (var goblet in gobletList)
                    {
                        foreach (var circlet in circletList)
                        {
                            ArtSubData artSubData = new(flower, plume, sands, goblet, circlet);
                            ArtSetData artSetData = GetArtSetData(artSubData.ArtifactCombination, ArtSetDatas_notSkipped);
                            if (artSetData == null) continue;

                            if (artifactConfig.isOnly4Set && artSetData.set == 2) continue;
                            //  Debug.Log($"{flower.art_set_name} {plume.art_set_name} {sands.art_set_name} {goblet.art_set_name} {circlet.art_set_name} {artSetData.set}");
                            ArtifactGroup artifactGroup = GetArtifactGroup(artSubData, ArtSetDatas_notSkipped);
                            if (artifactGroup != null) artifactGroups.Add(artifactGroup);
                        }
                    }
                }
            }
        }


        return artifactGroups;
    }





    static ArtifactGroup GetArtifactGroup(ArtSubData artSubData, ArtSetData[] ArtSetDatas_notSkipped)
    {
        ArtifactGroup artifactGroup = new()
        {
            artSubData = artSubData,
            artMainData = artSubData.GetArtMainData(),
            artSetData = GetArtSetData(artSubData.ArtifactCombination, ArtSetDatas_notSkipped)
        };

        if (artifactGroup.artSetData == null) return null;



        return artifactGroup;
    }


    static ArtSetData GetArtSetData(ArtifactData[] artifactCombination, ArtSetData[] ArtSetDatas_notSkipped)
    {
        var setNameGroup = artifactCombination
            .Select(artifactData => artifactData.art_set_name)
            .GroupBy(x => x);

        List<string> twoSetList = setNameGroup
            .Where(x => x.Count() >= 2)
            .Select(x => x.Key)
            .ToList();

        List<string> fourSetList = setNameGroup
            .Where(x => x.Count() >= 4)
            .Select(x => x.Key)
            .ToList();

        if (twoSetList.Count == 0) return null;

        // var  artSetDatas= Artifacts_Set.GetArtSetDatas();

        var artSetDatas_2set = ArtSetDatas_notSkipped
             .Where(artSetData => artSetData.set == 2);

        ArtSetData artSetData_2set = artSetDatas_2set
            .Where(artSetData => artSetData.name.Contains(twoSetList[0]))
            .FirstOrDefault();

        if (artSetData_2set == null)
        {
            Debug.LogError($"{twoSetList[0]} 2セットが見つかりません");
        }

        if (twoSetList.Count == 2)
        {
            ArtSetData artSetData_2set_2 = artSetDatas_2set
                .Where(artSetData => artSetData.name.Contains(twoSetList[1]))
                .FirstOrDefault();

            if (artSetData_2set_2 == null)
            {
                // Debug.LogError($"{twoSetList[1]} 2セットが見つかりません");
                return null;
            }

            var artSetData = FastInstanceAdder.AddInstances(new[] { artSetData_2set, artSetData_2set_2 });
            artSetData.set = 2;
            return artSetData;
        }

        if (fourSetList.Count == 1)
        {

            ArtSetData artSetData_4set = ArtSetDatas_notSkipped
                .Where(artSetData => artSetData.set == 4)
                .Where(artSetData => artSetData.name.Contains(fourSetList[0]))
                .FirstOrDefault();

            if (artSetData_4set == null)
            {
                Debug.LogError($"{fourSetList[0]} 4セットが見つかりません");
            }

            var artSetData = FastInstanceAdder.AddInstances(new[] { artSetData_2set, artSetData_4set });
            artSetData.set = 4;
            artSetData.name = $"{artSetData_4set.name}4";

            return artSetData;
        }

        return null;
    }
}

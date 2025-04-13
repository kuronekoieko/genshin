using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Artifacts_Sub
{
    public static List<ArtifactGroup> GetSubArtifactGroups(ArtSetData[] ArtSetDatas_notSkipped, ArtifactData[] artifactDatas)
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
                            ArtifactGroup artifactGroup = GetArtifactGroup(flower, plume, sands, goblet, circlet, ArtSetDatas_notSkipped);
                            if (artifactGroup != null) artifactGroups.Add(artifactGroup);
                        }
                    }
                }
            }
        }


        return artifactGroups;
    }


    static ArtifactGroup GetArtifactGroup(ArtifactData flower, ArtifactData plume, ArtifactData sands, ArtifactData goblet, ArtifactData circlet, ArtSetData[] ArtSetDatas_notSkipped)
    {
        ArtifactGroup artifactGroup = new();

        //サブステ================
        ArtifactData[] artifactCombination = new[] { flower, plume, sands, goblet, circlet };

        ArtifactData combinedArtifactData = Utils.AddInstances(artifactCombination);

        artifactGroup.artSubData = new(combinedArtifactData);

        //Debug.Log(JsonConvert.SerializeObject(artifactGroup.artSubData, Formatting.Indented));

        //メインステ================
        var artMainCombined = Artifacts_Main.GetArtMainCombined(new string[] { sands.art_main, goblet.art_main, circlet.art_main });
        artifactGroup.artMainData = new(artMainCombined);

        //セット================

        artifactGroup.artSetData = GetArtSetData(artifactCombination, ArtSetDatas_notSkipped);

        if (artifactGroup.artSetData == null) return null;

        // string a = string.Join("/", artifactCombination.Select(artifactData => artifactData.art_set_name).ToArray());
        //  Debug.Log(a + "\n" + setName);


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

        ArtSetData artSetData_1 = artSetDatas_2set
            .Where(artSetData => artSetData.name.Contains(twoSetList[0]))
            .FirstOrDefault();

        if (artSetData_1 == null)
        {
            Debug.LogError($"{twoSetList[0]} 2セットが見つかりません");
        }

        if (twoSetList.Count == 2)
        {
            ArtSetData artSetData_2 = artSetDatas_2set
                .Where(artSetData => artSetData.name.Contains(twoSetList[1]))
                .FirstOrDefault();

            if (artSetData_2 == null)
            {
                Debug.LogError($"{twoSetList[1]} 2セットが見つかりません");
            }

            var artSetData = Utils.AddInstances(new[] { artSetData_1, artSetData_2 });

            return artSetData;
        }

        if (fourSetList.Count == 1)
        {

            ArtSetData artSetData_2 = ArtSetDatas_notSkipped
                .Where(artSetData => artSetData.set == 4)
                .Where(artSetData => artSetData.name.Contains(fourSetList[0]))
                .FirstOrDefault();

            if (artSetData_2 == null)
            {
                Debug.LogError($"{fourSetList[0]} 4セットが見つかりません");
            }

            var artSetData = Utils.AddInstances(new[] { artSetData_1, artSetData_2 });
            artSetData.name = $"{artSetData_2.name}4";

            return artSetData;
        }

        return null;
    }
}

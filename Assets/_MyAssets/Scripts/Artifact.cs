using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Artifact
{


    public static List<ArtifactGroup> GetArtifactGroups(bool isSub)
    {

        List<ArtifactGroup> artifactGroups = new();

        if (isSub == false)
        {
            var artMainDatas = Artifacts_Main.GetArtMainDatas();
            var artSetDatas = CSVManager.artSetDatas.Where(artSetData => artSetData.skip != 1).ToArray();

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



        var artifactDatas = CSVManager.artifactDatas;


        var flowerList = artifactDatas.Where(artifactData => artifactData.part == "花" && artifactData.skip != 1).ToList();
        var plumeList = artifactDatas.Where(artifactData => artifactData.part == "羽" && artifactData.skip != 1).ToList();
        var sandsList = artifactDatas.Where(artifactData => artifactData.part == "時計" && artifactData.skip != 1).ToList();
        var gobletList = artifactDatas.Where(artifactData => artifactData.part == "杯" && artifactData.skip != 1).ToList();
        var circletList = artifactDatas.Where(artifactData => artifactData.part == "冠" && artifactData.skip != 1).ToList();


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
                            ArtifactGroup artifactGroup = GetArtifactGroup(flower, plume, sands, goblet, circlet);
                            if (artifactGroup != null) artifactGroups.Add(artifactGroup);
                        }
                    }
                }
            }
        }


        return artifactGroups;
    }

    static ArtifactGroup GetArtifactGroup(ArtifactData flower, ArtifactData plume, ArtifactData sands, ArtifactData goblet, ArtifactData circlet)
    {
        ArtifactGroup artifactGroup = new();

        //サブステ================
        ArtifactData[] artifactCombination = new[] { flower, plume, sands, goblet, circlet };

        ArtifactData combinedArtifactData = Utils.AddInstances(artifactCombination);

        artifactGroup.artSubData = new(combinedArtifactData);

        //Debug.Log(JsonConvert.SerializeObject(artifactGroup.artSubData, Formatting.Indented));

        //メインステ================
        var artMainCount = Artifacts_Main.GetArtMainCount(new string[] { sands.art_main, goblet.art_main, circlet.art_main });
        artifactGroup.artMainData = new(artMainCount);

        //セット================

        artifactGroup.artSetData = GetArtSetData(artifactCombination);

        if (artifactGroup.artSetData == null) return null;

        // string a = string.Join("/", artifactCombination.Select(artifactData => artifactData.art_set_name).ToArray());
        //  Debug.Log(a + "\n" + setName);


        return artifactGroup;
    }


    static ArtSetData GetArtSetData(ArtifactData[] artifactCombination)
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

        var artSetDatas_2set = CSVManager.artSetDatas
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

            ArtSetData artSetData_2 = CSVManager.artSetDatas
                .Where(artSetData => artSetData.set == 4)
                .Where(artSetData => artSetData.name.Contains(fourSetList[0]))
                .FirstOrDefault();

            if (artSetData_2 == null)
            {
                Debug.LogError($"{fourSetList[0]} 4セットが見つかりません");
            }

            var artSetData = Utils.AddInstances(new[] { artSetData_1, artSetData_2 });

            return artSetData;
        }

        return null;
    }



    public class ArtifactGroup
    {
        public ArtMainData artMainData = null;
        public ArtSetData artSetData = null;
        public ArtSubData artSubData = null;
    }

}

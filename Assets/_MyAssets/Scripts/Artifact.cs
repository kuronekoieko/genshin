using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Artifact
{


    public static List<ArtifactGroup> GetArtifactGroups()
    {
        List<ArtifactGroup> artifactGroups = new();

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
                            artifactGroups.Add(artifactGroup);
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

        ArtifactData combinedArtifactData = AddInstances(artifactCombination);

        artifactGroup.artSubData = new(combinedArtifactData);
        //メインステ================
        var artMainCount = Artifacts_Main.GetArtMainCount(new string[] { sands.art_main, goblet.art_main, circlet.art_main });
        artifactGroup.artMainData = new(artMainCount);

        //セット================


        var setNameGroup = artifactCombination
            .Select(artifactData => artifactData.art_set_name)
            .GroupBy(x => x);

        List<string> twoSetList = setNameGroup
            .Where(x => x.Count() >= 2)
            .Select(x => x.Key + "2")
            .ToList();

        List<string> fourSetList = setNameGroup
            .Where(x => x.Count() >= 4)
            .Select(x => x.Key + "4")
            .ToList();

        string setName = string.Join("+", twoSetList);
        if (fourSetList.Count > 0) setName = fourSetList[0];

        artifactGroup.artSetData = new() { name = setName };

        // string a = string.Join("/", artifactCombination.Select(artifactData => artifactData.art_set_name).ToArray());
        //  Debug.Log(a + "\n" + setName);


        return artifactGroup;
    }

    public static T AddInstances<T>(T[] instances) where T : new()
    {
        Type type = typeof(T);
        T newInstance = new(); // デフォルトの値をセットする

        foreach (FieldInfo fieldInfo in type.GetFields())
        {
            if (fieldInfo.FieldType == typeof(int))
            {
                int sum = 0;
                foreach (T instance in instances)
                {
                    sum += (int)fieldInfo.GetValue(instance);
                }
                fieldInfo.SetValue(newInstance, sum);
            }
            if (fieldInfo.FieldType == typeof(float))
            {
                float sum = 0;
                foreach (T instance in instances)
                {
                    sum += (float)fieldInfo.GetValue(instance);
                }
                fieldInfo.SetValue(newInstance, sum);
            }
            if (fieldInfo.FieldType == typeof(bool))
            {
                bool flag = false;
                foreach (T instance in instances)
                {
                    flag = (bool)fieldInfo.GetValue(instance);
                    if (flag) break;
                }
                fieldInfo.SetValue(newInstance, flag);
            }
            if (fieldInfo.FieldType == typeof(string))
            {
                string name = "";
                foreach (T instance in instances)
                {
                    name += (string)fieldInfo.GetValue(instance) + "+";
                }
                fieldInfo.SetValue(newInstance, name.TrimEnd('+'));
            }
        }
        return newInstance;
    }


    public class ArtifactGroup
    {
        public ArtMainData artMainData = null;
        public ArtSetData artSetData = null;
        public ArtSubData artSubData = null;
    }

}

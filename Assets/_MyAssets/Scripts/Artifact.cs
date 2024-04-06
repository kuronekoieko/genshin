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


    List<ArtifactGroup> ArtifactGroups()
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
                            ArtifactData[] artifactCombination = new[] { flower, plume, sands, goblet, circlet };

                            ArtifactData combinedArtifactData = AddInstances(artifactCombination);

                            ArtSubData artSubData = GetArtSubData(combinedArtifactData);

                            

                        }
                    }
                }
            }
        }


        return artifactGroups;
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


    public ArtSubData GetArtSubData(ArtifactData artifactData)
    {

        ArtSubData artSubData = new()
        {
            skip = artifactData.skip,
            crit_rate = artifactData.crit_rate,
            crit_dmg = artifactData.crit_dmg,
            hp = artifactData.hp,
            hp_rate = artifactData.hp_rate,
            atk = artifactData.atk,
            atk_rate = artifactData.atk_rate,
            def = artifactData.def,
            def_rate = artifactData.def_rate,
            energy_recharge = artifactData.energy_recharge,
            elemental_mastery = artifactData.elemental_mastery,
        };

        return artSubData;
    }


    class ArtifactGroup
    {
        public ArtMainData artMainData = null;
        public ArtSetData artSetData = null;
        public ArtSubData artSubData = null;
    }

}

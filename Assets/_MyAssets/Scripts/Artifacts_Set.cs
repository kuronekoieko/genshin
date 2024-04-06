using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Artifacts_Set
{

    public static List<ArtSetData> GetArtSetDatas()
    {
        HashSet<ArtSetData> artSetDatas = new();

        var artSetDatas_2set = CSVManager.artSetDatas.Where(artSetData => artSetData.set == 2).ToArray();
        var artSetDatas_4set = CSVManager.artSetDatas.Where(artSetData => artSetData.set == 4).ToArray();

        foreach (var artSetData_1 in artSetDatas_2set)
        {
            foreach (var artSetData_2 in artSetDatas_2set)
            {
                if (artSetData_1.name == artSetData_2.name) continue;

                var sorted = new[] { artSetData_1, artSetData_2 }.OrderBy(artSetData => artSetData.name).ToArray();

                var artSetData = Utils.AddInstances(sorted);
                artSetDatas.Add(artSetData);
            }
        }


        foreach (var artSetData_4set in artSetDatas_4set)
        {
            var artSetData_2set = artSetDatas_2set.FirstOrDefault(artSetData => artSetData.name == artSetData_4set.name);
            if (artSetData_2set == null)
            {
                Debug.LogError($"{artSetData_4set.name} 2セットが見つかりません");
                continue;
            }

            var sorted = new[] { artSetData_2set, artSetData_4set }.OrderBy(artSetData => artSetData.name).ToArray();

            var artSetData = Utils.AddInstances(sorted);

            artSetData.name = $"{artSetData_4set.name}4";
            if (string.IsNullOrEmpty(artSetData_4set.option) == false)
            {
                artSetData.name += $"({artSetData_4set.option})";
            }

            artSetDatas.Add(artSetData);
        }

        return artSetDatas.ToList();
    }

}

using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;


public class Main : MonoBehaviour
{
    [SerializeField] BaseCharacter character;
    [SerializeField] bool isSub;


    async void Start()
    {
        await CSVManager.InitializeAsync();

        var texts = await Calc();

        FileWriter.Save(character.Name, texts);
    }





    async UniTask<List<string>> Calc()
    {
        List<Data> datas = GetDatas();

        var results = await GetResultsAsync(datas);

        Debug.Log("計算終了");

        var sortKey = results[0].Keys.ToArray()[4];

        results = results
            .OrderByDescending(result =>
            {

                // float.TryParse(result["通常期待値"], out float val);
                float.TryParse(result[sortKey], out float val);
                return val;
            })
            .Take(100)
            .ToList();

        List<string> texts = new();
        string header = string.Join(",", results[0].Keys.ToArray());
        texts.Add(header);
        foreach (var result in results)
        {
            string line = string.Join(",", result.Values.ToArray());
            texts.Add(line);
        }

        return texts;
    }


    List<Data> GetDatas()
    {
        List<Data> datas = new();

        var weaponDatas = CSVManager.weaponDatas.Where(weaponData => weaponData.skip != 1).ToArray();
        var partyDatas = Party.GetPartyDatas(CSVManager.partyDatas, character.status.elementType);

        var artifactGroups = Artifact.GetArtifactGroups(isSub);

        Debug.Log("ダメージ計算開始");

        foreach (var weapon in weaponDatas)
        {
            foreach (var partyData in partyDatas)
            {
                foreach (var artifactGroup in artifactGroups)
                {
                    if (weapon.type != character.WeaponType) continue;
                    if (artifactGroup.artSetData.name == "しめ縄4" && character.status.notUseShimenawa) continue;
                    if (artifactGroup.artSetData.name == "楽団4")
                    {
                        bool isGakudan = character.status.weaponType == WeaponType.Catalyst || character.status.weaponType == WeaponType.Bow;
                        if (isGakudan == false) continue;
                    }

                    if (artifactGroup.artSetData.name == "ファントム4")
                    {
                        bool hasSelfHarm = character.status.hasSelfHarm || partyData.has_self_harm;
                        if (hasSelfHarm == false) continue;
                    }


                    Data data = new()
                    {
                        weapon = weapon,
                        artMain = artifactGroup.artMainData,
                        artSets = artifactGroup.artSetData,
                        partyData = partyData,
                        artSub = artifactGroup.artSubData,
                        status = character.status,
                        ascend = character.ascend,
                    };
                    datas.Add(data);
                }
            }

        }
        return datas;
    }

    async Task<List<Dictionary<string, string>>> GetResultsAsync(List<Data> datas)
    {
        List<Dictionary<string, string>> results = new();

        int progress = 0;
        int max = datas.Count;

        foreach (var data in datas)
        {
            Dictionary<string, string> result = character.CalcDmg(data);
            if (result != null) results.Add(result);

            progress++;
            if (progress % 200000 == 0)
            {
                await UniTask.DelayFrame(1);
                Debug.Log("progress: " + progress + "/" + max);
            }
        }

        return results;
    }

}

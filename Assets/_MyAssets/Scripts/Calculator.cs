using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;


public static class Calculator
{

    public static async UniTask Calc(BaseCharacter character, bool isSub)
    {
        await CSVManager.InitializeAsync();

        var partyDatas = Party.GetPartyDatas(character.status.elementType, CSVManager.MemberDatas);
        var weaponDatas = GetWeaponDatas(character);
        var artifactGroups = GetArtifactGroups(isSub);

        List<Data> datas = GetDatas(character, weaponDatas, partyDatas, artifactGroups);

        var results = await GetResultsAsync(datas, character);
        var texts = ResultsToList(results);
        FileWriter.Save(character.Name, texts);
    }

    public static List<string> ResultsToList(List<Dictionary<string, string>> results)
    {
        Debug.Log("計算終了: " + results.Count + " 件");

        if (results.Count == 0)
        {
            Debug.LogError("結果が0件");
            return null;
        }

        var sortKey = results[0].Keys.ToArray()[4];

        results = results
            .OrderByDescending(result =>
            {

                // float.TryParse(result["通常期待値"], out float val);
                float.TryParse(result[sortKey], out float val);
                return val;
            })
            .Take(1000)
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


    static WeaponData[] GetWeaponDatas(BaseCharacter character)
    {
        var weaponDatas = CSVManager.WeaponDatas
            .Where(weaponData => weaponData.WeaponType == character.WeaponType)
            .ToArray();
        return weaponDatas;
    }

    static List<ArtifactGroup> GetArtifactGroups(bool isSub)
    {
        if (isSub)
        {
            return Artifact.GetSubArtifactGroups(CSVManager.ArtSetDatas_notSkipped, CSVManager.ArtifactDatas);
        }
        else
        {
            return Artifact.GetFixedScoreArtifactGroups(CSVManager.ArtSetDatas, new());
        }
    }


    public static List<Data> GetDatas(BaseCharacter character, WeaponData[] weaponDatas, PartyData[] partyDatas, List<ArtifactGroup> artifactGroups)
    {
        return GetDatas(character.status, character.ascend, weaponDatas, partyDatas, artifactGroups);
    }

    public static List<Data> GetDatas(BaseCharacterSO character, WeaponData[] weaponDatas, PartyData[] partyDatas, List<ArtifactGroup> artifactGroups)
    {
        return GetDatas(character.status, character.ascend, weaponDatas, partyDatas, artifactGroups);
    }

    static List<Data> GetDatas(Status status, Ascend ascend, WeaponData[] weaponDatas, PartyData[] partyDatas, List<ArtifactGroup> artifactGroups)
    {
        Debug.Log("組み合わせ作成開始");

        List<Data> datas = new();

        foreach (var weapon in weaponDatas)
        {
            foreach (var partyData in partyDatas)
            {
                foreach (var artifactGroup in artifactGroups)
                {
                    Data data = new(weapon, artifactGroup, partyData, status, ascend);

                    if (data.IsSkip() == false)
                    {
                        datas.Add(data);
                    }
                    else
                    {
                        Utils.LogJson(data.partyData.name);
                        // Debug.Log("===========================");
                    }
                }
            }

        }
        return datas;
    }


    public static async Task<List<Dictionary<string, string>>> GetResultsAsync<T>(List<Data> datas, T character) where T : ICalcDmg
    {
        await UniTask.DelayFrame(1);

        Debug.Log("ダメージ計算開始");

        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();


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
                int per = (int)((float)progress / (float)max * 100f);

                Debug.Log("progress: " + progress + "/" + max + " " + per + "%");
            }
        }


        sw.Stop();
        Debug.Log("処理時間 " + sw.ElapsedMilliseconds / 1000f + "s");

        return results;
    }
}
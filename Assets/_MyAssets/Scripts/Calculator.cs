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
        List<Data> datas = GetDatas(character, isSub, CSVManager.WeaponDatas, CSVManager.MemberDatas, CSVManager.ArtSetDatas, CSVManager.ArtSetDatas_notSkipped, CSVManager.ArtifactDatas);
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


    public static List<Data> GetDatas(
        BaseCharacter character,
        bool isSub,
        WeaponData[] WeaponDatas,
        MemberData[] MemberDatas,
        ArtSetData[] ArtSetDatas,
        ArtSetData[] ArtSetDatas_notSkipped,
        ArtifactData[] artifactDatas)
    {
        Debug.Log("組み合わせ作成開始");

        List<Data> datas = new();

        var weaponDatas = WeaponDatas
            .Where(weaponData => weaponData.type == character.WeaponType)
            .ToArray();
        var partyDatas = Party.GetPartyDatas(character.status.elementType, MemberDatas);

        var artifactGroups = Artifact.GetArtifactGroups(ArtSetDatas);
        if (isSub) artifactGroups = Artifact.GetSubArtifactGroups(ArtSetDatas_notSkipped, artifactDatas);

        foreach (var weapon in weaponDatas)
        {
            foreach (var partyData in partyDatas)
            {
                foreach (var artifactGroup in artifactGroups)
                {
                    Data data = new()
                    {
                        weapon = weapon,
                        artMainData = artifactGroup.artMainData,
                        artSetData = artifactGroup.artSetData,
                        partyData = partyData,
                        artSub = artifactGroup.artSubData,
                        status = character.status,
                        ascend = character.ascend,
                    };

                    if (data.IsSkip() == false) datas.Add(data);
                }
            }

        }
        return datas;
    }


    public static async Task<List<Dictionary<string, string>>> GetResultsAsync(List<Data> datas, BaseCharacter character)
    {
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

    public static async Task<List<Dictionary<string, string>>> GetResultsAsync(List<Data> datas, BaseCharacterSO character)
    {
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
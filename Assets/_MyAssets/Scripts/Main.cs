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


    List<Data> GetDatas()
    {
        Debug.Log("組み合わせ作成開始");

        List<Data> datas = new();

        var weaponDatas = CSVManager.WeaponDatas
            .Where(weaponData => weaponData.type == character.WeaponType)
            .ToArray();
        var partyDatas = Party.GetPartyDatas(character.status.elementType);

        var artifactGroups = Artifact.GetArtifactGroups(isSub);


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


    async Task<List<Dictionary<string, string>>> GetResultsAsync(List<Data> datas)
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

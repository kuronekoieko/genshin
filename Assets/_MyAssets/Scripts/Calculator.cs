using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;


public static class Calculator
{


    async public static void Calc(List<Data> datas, BaseCharacterSO baseCharacterSO)
    {
        var results = await GetResultsAsync(datas, baseCharacterSO);
        var texts = ResultsToList(results);
        Export(baseCharacterSO.name, texts);
    }

    static void Export(string fileName, List<string> texts)
    {
        FileWriter.Save(fileName, texts);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    static List<string> ResultsToList(List<Dictionary<string, string>> results)
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



    static async Task<List<Dictionary<string, string>>> GetResultsAsync<T>(List<Data> datas, T character) where T : ICalcDmg
    {
        await UniTask.DelayFrame(1);

        Debug.Log("ダメージ計算開始");

        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        int progress = 0;
        int max = datas.Count;

        List<Dictionary<string, string>> results = new();

        foreach (var data in datas)
        {
            Dictionary<string, string> result = character.CalcDmg(data);
            if (result != null) results.Add(result);

            if (sw.ElapsedMilliseconds % 1000 == 0)
            {
                await UniTask.DelayFrame(1);

                progress++;
                //await UniTask.DelayFrame(1);
                int per = (int)((float)progress / (float)max * 100f);

                Debug.Log("progress: " + progress + "/" + max + " " + per + "%");
            }
        }


        sw.Stop();
        Debug.Log("処理時間 " + sw.ElapsedMilliseconds / 1000f + "s");

        return results;
    }
}
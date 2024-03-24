using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Linq;
using System.IO;
using UnityEditor;

public class Main
{

    [MenuItem("Genshin/Start")]
    async static void Start()
    {
        await CSVManager.InitializeAsync();

        var texts = await Calc();

        Save("YaeMiko", texts);
    }



    static bool isSub;

    static async UniTask<List<string>> Calc()
    {
        List<Dictionary<string, string>> results = new();

        // List<Dictionary<string, string>> getArtSubConbinations(Dictionary<string, string> artMain);

        var artMainArray = Artifacts_Main.GetArtMainDatas();

        if (isSub)
        {
            // artSubArray = GetArtSubConbinations(artMainArray[0]);
        }

        Debug.Log("ダメージ計算開始");

        int progress = 0;
        int max = CSVManager.weaponDatas.Length * CSVManager.artSetDatas.Length * CSVManager.partyDatas.Length * CSVManager.artSubDatas.Length * artMainArray.Length;

        foreach (var weapon in CSVManager.weaponDatas)
        {
            foreach (var artSets in CSVManager.artSetDatas)
            {
                foreach (var chara in CSVManager.partyDatas)
                {
                    foreach (var artSub in CSVManager.artSubDatas)
                    {
                        ArtMainData artMain;
                        if (isSub)
                        {
                            //artMain = artSub["聖遺物メイン"];

                            // List<string> result = CalcDmg(weapon, artMain, artSets, chara, artSub);
                            // results.Add(result);
                        }
                        else
                        {
                            foreach (var artMainItem in artMainArray)
                            {
                                artMain = artMainItem;
                                Dictionary<string, string> result = YaeMiko.CalcDmg(weapon, artMain, artSets, chara, artSub);
                                results.Add(result);
                                progress++;

                                if (progress % 100000 == 0)
                                {
                                    await UniTask.DelayFrame(1);
                                    Debug.Log("progress: " + progress + "/" + max);
                                }
                            }

                        }
                    }
                }
            }

        }

        Debug.Log("計算終了");

        results = results
            .OrderByDescending(result => result["スキル期待値"])
            .Take(100)
            .ToList();

        List<string> texts = new();
        string header = string.Join(",", results[0].Keys.ToArray());
        texts.Add(header);
        foreach (var result in results)
        {
            string line = string.Join(",", results[0].Values.ToArray());
            texts.Add(line);
        }

        foreach (var text in texts)
        {
            Debug.Log(text);
        }
        return texts;
    }

    static void Save(string fileName, List<string> list)
    {
        Debug.Log("書き込み開始");

        string directoryPath = Application.dataPath + @"/_MyAssets/CSV/YaeMiko/";
        fileName += ".csv";
        string path = directoryPath + fileName;
        // ディレクトリが存在しない場合は作成
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        using StreamWriter sw = File.CreateText(path);


        foreach (var line in list)
        {
            sw.WriteLine(line);
        }

        AssetDatabase.Refresh();
        Debug.Log("生成完了 " + fileName);
    }


}

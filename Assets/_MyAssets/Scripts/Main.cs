using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Linq;
using System.IO;
using Newtonsoft.Json;

public class Main : MonoBehaviour
{
    [SerializeField] BaseCharacter character;


    async void Start()
    {
        Debug.Log("CSVロード開始");

        await CSVManager.InitializeAsync();

        var texts = await Calc();

        Save(character.Name, texts);
    }




    bool isSub;

    async UniTask<List<string>> Calc()
    {
        List<Dictionary<string, string>> results = new();

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
            if (weapon.skip == 1) continue;
            if (weapon.type != character.WeaponType) continue;

            foreach (var artSets in CSVManager.artSetDatas)
            {
                if (artSets.skip == 1) continue;

                foreach (var chara in CSVManager.partyDatas)
                {
                    if (chara.skip == 1) continue;

                    foreach (var artSub in CSVManager.artSubDatas)
                    {
                        if (artSub.skip == 1) continue;

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
                                if (artMainItem.skip == 1) continue;

                                artMain = artMainItem;
                                Datas datas = new()
                                {
                                    weapon = weapon,
                                    artMain = artMain,
                                    artSets = artSets,
                                    partyData = chara,
                                    artSub = artSub,
                                    status = character.status,
                                    ascend = character.ascend,
                                };
                                Dictionary<string, string> result = character.CalcDmg(datas);
                                results.Add(result);
                                progress++;

                                if (progress % 200000 == 0)
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

        foreach (var result in results)
        {
            //  Debug.Log(JsonConvert.SerializeObject(result, Formatting.Indented));
        }

        results = results
            .OrderByDescending(result =>
            {
                float.TryParse(result["通常期待値"], out float val);
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

    async void Save(string fileName, List<string> list)
    {
        Debug.Log("書き込み開始");

        string directoryPath = Application.dataPath + $"/_MyAssets/CSV/{fileName}/";
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

        //AssetDatabase.Refresh();
        Debug.Log("生成完了 " + fileName);

        await UniTask.DelayFrame(1);
        UnityEditor.EditorApplication.isPlaying = false;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    async void Start()
    {
        await CSVManager.InitializeAsync();

        ReadAndWrite();
    }



    private bool isSub;
    private List<Dictionary<string, string>> results = new();

    public void ReadAndWrite()
    {
        // List<Dictionary<string, string>> getArtSubConbinations(Dictionary<string, string> artMain);

        var artMainArray = Artifacts_Main.GetArtMainDatas();

        if (isSub)
        {
            // artSubArray = GetArtSubConbinations(artMainArray[0]);
        }

        Debug.Log("ダメージ計算開始");

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
                            }
                        }
                    }
                }
            }
        }

        // results = GetTopResults(results, 4);
        foreach (var result in results)
        {
            Debug.Log(string.Join(", ", result));
        }
        // Debug.Log("完了");
    }


}

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


    async void Start()
    {
        await CSVManager.InitializeAsync();

        var texts = await Calc();

        FileWriter.Save(character.Name, texts);
    }




    bool isSub;

    async UniTask<List<string>> Calc()
    {
        var results = await GetResultsAsync();

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


    async Task<List<Dictionary<string, string>>> GetResultsAsync()
    {
        List<Dictionary<string, string>> results = new();

        var artMainDatas = Artifacts_Main.GetArtMainDatas();
        var weaponDatas = CSVManager.weaponDatas;
        var artSetDatas = CSVManager.artSetDatas;
        var partyDatas = Party.GetPartyDatas(CSVManager.partyDatas, character.status.elementType);
        var artSubDatas = CSVManager.artSubDatas;

        if (isSub)
        {
            // artSubArray = GetArtSubConbinations(artMainArray[0]);
        }

        Debug.Log("ダメージ計算開始");

        int progress = 0;
        int max = weaponDatas.Length * artSetDatas.Length * partyDatas.Length * artSubDatas.Length * artMainDatas.Length;

        foreach (var weapon in weaponDatas)
        {
            if (weapon.skip == 1) continue;
            if (weapon.type != character.WeaponType) continue;

            foreach (var artSets in artSetDatas)
            {
                if (artSets.skip == 1) continue;
                if (artSets.name == "しめ縄4" && character.status.notUseShimenawa) continue;

                foreach (var partyData in partyDatas)
                {
                    if (partyData.skip == 1) continue;
                    if (artSets.name == "ファントムハンター")
                    {
                        bool hasSelfHarm = character.status.hasSelfHarm || partyData.has_self_harm;
                        if (hasSelfHarm == false) continue;
                    }

                    foreach (var artSub in artSubDatas)
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
                            foreach (var artMainItem in artMainDatas)
                            {
                                if (artMainItem.skip == 1) continue;

                                artMain = artMainItem;
                                Datas datas = new()
                                {
                                    weapon = weapon,
                                    artMain = artMain,
                                    artSets = artSets,
                                    partyData = partyData,
                                    artSub = artSub,
                                    status = character.status,
                                    ascend = character.ascend,
                                };
                                Dictionary<string, string> result = character.CalcDmg(datas);
                                if (result != null) results.Add(result);

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
        return results;
    }

}

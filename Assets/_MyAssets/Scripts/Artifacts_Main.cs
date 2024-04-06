using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Artifacts_Main
{
    //聖遺物メイン
    static readonly float artMainHpFixed = 4780;
    static readonly float artMainAtkFixed = 311;
    static readonly float artMainAtkPer = 0.466f;
    static readonly float artMainDefPer = 0.583f;
    static readonly float artMainBuffPer = 0.466f;
    static readonly float artMainPhysicsBuffPer = 0.583f;
    static readonly float artMainHPPer = 0.466f;
    static readonly float artMainCritRate = 0.311f;
    static readonly float artMainCritDmg = 0.622f;
    static readonly float artMainElementalMastery = 187;
    static readonly float artMainEnergyRecharge = 0.518f;
    static readonly float artMainHealPer = 0.359f;

    static readonly string[] artMainTokeiArray = { "攻撃%", "防御%", "HP%", "元素熟知", "元チャ" };
    static readonly string[] artMainSakadukiArray = { "攻撃%", "防御%", "HP%", "元素熟知", "元素バフ", "物理バフ" };
    static readonly string[] artMainKanmuriArray = { "攻撃%", "防御%", "HP%", "元素熟知", "会心率", "会心ダメージ", "治癒効果" };

    static readonly List<string[]> artMainCombinations = new()
    {
            new string[] {
            "攻撃%",
            "防御%",
            "HP%",
            "元素熟知",
            "元チャ",
            "元素バフ",
            "物理バフ",
            "会心率",
            "会心ダメージ",
            "治癒効果"
        }
    };


    static public ArtMainData[] GetArtMainDatas()
    {
        Debug.Log("聖遺物メイン計算開始");

        List<ArtMainData> artMainDatas = new List<ArtMainData>();
        var artMainCounts = GetArtMainDictionaries();
        foreach (var item in artMainCounts)
        {
            // Debug.Log(item.artMainDictionaries["攻撃%"]);
            ArtMainData artMainData = new(item);
            artMainDatas.Add(artMainData);
        }

        return artMainDatas.ToArray();
    }

    static List<ArtMainCount> GetArtMainDictionaries()
    {

        Debug.Log("聖遺物メイン計算開始");

        List<ArtMainCount> artMainCounts = new();

        for (int k = 0; k < artMainTokeiArray.Length; k++)
        {
            for (int l = 0; l < artMainSakadukiArray.Length; l++)
            {
                for (int m = 0; m < artMainKanmuriArray.Length; m++)
                {
                    string artMainTokei = artMainTokeiArray[k];
                    string artMainSakaduki = artMainSakadukiArray[l];
                    string artMainKanmuri = artMainKanmuriArray[m];

                    string[] nameCombinations = { artMainTokei, artMainSakaduki, artMainKanmuri };

                    ArtMainCount artMainCount = GetArtMainCount(nameCombinations);

                    artMainCounts.Add(artMainCount);
                }
            }
        }

        return artMainCounts;
    }

    public static ArtMainCount GetArtMainCount(string[] nameCombinations)
    {

        string name = string.Join("+", nameCombinations);


        int[] artMainCombination = new int[artMainCombinations[0].Length];

        for (int i = 0; i < artMainCombinations[0].Length; i++)
        {
            int count = 0;

            foreach (string nameCombination in nameCombinations)
            {
                count += artMainCombinations[0][i] == nameCombination ? 1 : 0;
            }

            artMainCombination[i] = count;
        }


        ArtMainCount artMainCount = new()
        {
            name = name,
            artMainDictionaries = ArrayToDictionary(artMainCombinations[0], artMainCombination)
        };
        return artMainCount;
    }

    static private Dictionary<string, int> ArrayToDictionary(string[] keys, int[] values)
    {
        Dictionary<string, int> dictionary = new();

        for (int i = 0; i < keys.Length; i++)
        {
            dictionary[keys[i]] = values[i];
        }

        return dictionary;
    }

    public class ArtMainCount
    {
        public string name;
        public Dictionary<string, int> artMainDictionaries = new();
    }
}

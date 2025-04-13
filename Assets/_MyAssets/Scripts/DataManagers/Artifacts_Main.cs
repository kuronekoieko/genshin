using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Artifacts_Main
{
    static readonly string[] artMainTokeiArray = { "攻撃%", "防御%", "HP%", "元素熟知", "元チャ" };
    static readonly string[] artMainSakadukiArray = { "攻撃%", "防御%", "HP%", "元素熟知", "炎バフ", "水バフ", "雷バフ", "氷バフ", "岩バフ", "風バフ", "草バフ", "物理バフ" };
    static readonly string[] artMainKanmuriArray = { "攻撃%", "防御%", "HP%", "元素熟知", "会心率", "会心ダメージ", "治癒効果" };

    static readonly string[] artMainCombinations = new string[] { "攻撃%", "防御%", "HP%", "元素熟知", "元チャ", "炎バフ", "水バフ", "雷バフ", "氷バフ", "岩バフ", "風バフ", "草バフ", "物理バフ", "会心率", "会心ダメージ", "治癒効果" };



    static ArtMainData[] GetArtMainDatas_Test()
    {
        Debug.Log("聖遺物メイン計算開始 テスト");

        string artMainTokei = artMainTokeiArray[0];
        string artMainSakaduki = artMainSakadukiArray[0];
        string artMainKanmuri = artMainKanmuriArray[0];

        string[] nameCombinations = { artMainTokei, artMainSakaduki, artMainKanmuri };

        ArtMainCombined artMainCombined = GetArtMainCombined(nameCombinations);
        ArtMainData artMainData = new(artMainCombined);





        return new[] { artMainData };
    }

    static public ArtMainData[] GetArtMainDatas(bool isTest)
    {
        if (isTest) return GetArtMainDatas_Test();


        Debug.Log("聖遺物メイン計算開始");

        List<ArtMainData> artMainDatas = new();
        var artMainCombines = GetArtMainDictionaries();
        foreach (var artMainCombined in artMainCombines)
        {
            ArtMainData artMainData = new(artMainCombined);
            artMainDatas.Add(artMainData);
        }

        return artMainDatas.ToArray();
    }

    static List<ArtMainCombined> GetArtMainDictionaries()
    {
        Debug.Log("聖遺物メイン計算開始");

        HashSet<ArtMainCombined> artMainCombines = new();

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

                    ArtMainCombined artMainCombined = GetArtMainCombined(nameCombinations);

                    artMainCombines.Add(artMainCombined);
                }
            }
        }

        return artMainCombines.ToList();
    }

    public static ArtMainCombined GetArtMainCombined(string[] nameCombinations)
    {

        string name = string.Join("+", nameCombinations);


        int[] artMainCombination = new int[artMainCombinations.Length];

        for (int i = 0; i < artMainCombinations.Length; i++)
        {
            int count = 0;

            foreach (string nameCombination in nameCombinations)
            {
                count += artMainCombinations[i] == nameCombination ? 1 : 0;
            }

            artMainCombination[i] = count;
        }

        ArtMainCombined artMainCombined = new()
        {
            displayName = name,
            compareName = string.Join("+", nameCombinations.OrderBy(n => n).ToArray()),
            artMainDictionaries = ArrayToDictionary(artMainCombinations, artMainCombination)
        };
        return artMainCombined;
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
}

public class ArtMainCombined
{
    public string displayName;
    public string compareName;
    public Dictionary<string, int> artMainDictionaries = new();

    public ArtMainCombined()
    {

    }

    // https://www.mum-meblog.com/entry/tyr-utility/csharp-hashset
    public override bool Equals(object obj)
    {
        ArtMainCombined other = obj as ArtMainCombined;
        if (other == null) return false;
        return this.compareName == other.compareName;
    }

    public override int GetHashCode()
    {
        // hashsetは通るが、sortedsetは通らない
        return compareName.GetHashCode();
    }
}
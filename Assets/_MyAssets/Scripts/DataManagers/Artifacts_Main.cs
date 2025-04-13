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


    static ArtMainData[] GetArtMainDatas_Test()
    {
        Debug.Log("聖遺物メイン計算開始 テスト");

        string artMainTokei = artMainTokeiArray[0];
        string artMainSakaduki = artMainSakadukiArray[0];
        string artMainKanmuri = artMainKanmuriArray[0];

        string[] nameCombinations = { artMainTokei, artMainSakaduki, artMainKanmuri };

        ArtMainHash artMainHash = new(nameCombinations);
        ArtMainData artMainData = new(artMainHash);
        return new[] { artMainData };
    }

    public static ArtMainData[] GetArtMainDatas(bool isTest)
    {
        if (isTest) return GetArtMainDatas_Test();

        Debug.Log("聖遺物メイン計算開始");

        List<ArtMainData> artMainDatas = new();
        var artMainHashes = GetArtMainHashes();
        foreach (var artMainHash in artMainHashes)
        {
            ArtMainData artMainData = new(artMainHash);
            artMainDatas.Add(artMainData);
        }

        return artMainDatas.ToArray();
    }

    static List<ArtMainHash> GetArtMainHashes()
    {
        Debug.Log("聖遺物メイン計算開始");

        // 同じ組み合わせの重複削除
        HashSet<ArtMainHash> artMainCombines = new();

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

                    ArtMainHash artMainHash = new(nameCombinations);

                    artMainCombines.Add(artMainHash);
                }
            }
        }

        return artMainCombines.ToList();
    }
}

public class ArtMainHash
{
    readonly Dictionary<string, int> artMainPartCounts = new();
    public string CompareName { get; private set; }
    public string DisplayName { get; private set; }


    public ArtMainHash(string[] partNames)
    {
        CompareName = string.Join("+", partNames.OrderBy(n => n).ToArray());
        DisplayName = CompareName;

        foreach (var partName in partNames)
        {
            if (artMainPartCounts.TryGetValue(partName, out int count))
            {
                artMainPartCounts[partName]++;
            }
            else
            {
                artMainPartCounts.Add(partName, 1);
            }
        }

    }

    public int GetPartCount(string partName)
    {
        if (artMainPartCounts.TryGetValue(partName, out int count))
        {
            return count;
        }
        else
        {
            return 0;
        }
    }

    // https://www.mum-meblog.com/entry/tyr-utility/csharp-hashset
    // hashsetに必須
    public override bool Equals(object obj)
    {
        ArtMainHash other = obj as ArtMainHash;
        if (other == null) return false;
        return this.CompareName == other.CompareName;
    }
    // hashsetに必須
    public override int GetHashCode()
    {
        // hashsetは通るが、sortedsetは通らない
        return CompareName.GetHashCode();
    }
}
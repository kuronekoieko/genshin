using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;




public class Artifacts_Main
{

    readonly ArtMainHeader header;

    public Artifacts_Main(ArtMainHeader header)
    {
        this.header = header;
    }


    ArtMainData[] GetArtMainDatas_Test()
    {
        Debug.Log("聖遺物メイン計算開始 テスト");

        string artMainTokei = header.sands[0];
        string artMainSakaduki = header.goblets[0];
        string artMainKanmuri = header.circlets[0];

        string[] nameCombinations = { artMainTokei, artMainSakaduki, artMainKanmuri };

        ArtMainHash artMainHash = new(nameCombinations);
        ArtMainData artMainData = new(artMainHash);
        return new[] { artMainData };
    }

    public ArtMainData[] GetArtMainDatas()
    {
        //  if (isTest) return GetArtMainDatas_Test();

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

    List<ArtMainHash> GetArtMainHashes()
    {
        // 同じ組み合わせの重複削除
        HashSet<ArtMainHash> artMainCombines = new();


        foreach (var sand in header.sands)
        {
            foreach (var goblet in header.goblets)
            {
                foreach (var circlet in header.circlets)
                {

                    string[] nameCombinations = { sand, goblet, circlet };

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
        // 並べ替えすると、会心ダメージが先に来て見づらくなる
        DisplayName = string.Join("+", partNames);

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
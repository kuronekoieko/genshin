using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifacts_Main
{
    public List<Dictionary<string, float>> GetArtMainCombinations()
    {
        string[] artMainTokeiArray = { "攻撃%", "防御%", "HP%", "元素熟知", "元チャ" };
        string[] artMainSakadukiArray = { "攻撃%", "防御%", "HP%", "元素熟知", "元素バフ", "物理バフ" };
        string[] artMainKanmuriArray = { "攻撃%", "防御%", "HP%", "元素熟知", "会心率", "会心ダメージ", "治癒効果" };

        List<string[]> artMainCombinations = new List<string[]>();
        artMainCombinations.Add(new string[] {
            "聖遺物メイン",
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
        });

        Debug.Log("聖遺物メイン計算開始");

        List<Dictionary<string, float>> ad = new List<Dictionary<string, float>>();

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

                    string name = $"{artMainTokei}、{artMainSakaduki}、{artMainKanmuri}";
                    float[] artMainCombination = new float[artMainCombinations[0].Length];

                    for (int i = 1; i < artMainCombinations[0].Length; i++)
                    {
                        int count = 0;

                        foreach (string nameCombination in nameCombinations)
                        {
                            count += artMainCombinations[0][i] == nameCombination ? 1 : 0;
                        }

                        artMainCombination[i] = count;
                    }

                    ad.Add(ArrayToDictionary(artMainCombinations[0], artMainCombination));
                }
            }
        }

        return ad;
    }

    private Dictionary<string, float> ArrayToDictionary(string[] keys, float[] values)
    {
        Dictionary<string, float> dictionary = new Dictionary<string, float>();

        for (int i = 0; i < keys.Length; i++)
        {
            dictionary[keys[i]] = values[i];
        }

        return dictionary;
    }
}

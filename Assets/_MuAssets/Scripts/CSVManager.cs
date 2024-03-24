using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Cysharp.Threading.Tasks;
using System.IO;

public class CSVManager
{
    /*
    public static HomeText[] HomeTexts { get; private set; }

    public static async UniTask InitializeAsync()
    {

    }

    public static async UniTask<T[]> DeserializeAsync<T>(string fileName)
    {
        // パスに拡張子つけない
        string path = "CSV/" + fileName;
        var result = await Resources.LoadAsync<TextAsset>(path);
        var textAsset = result as TextAsset;
        if (textAsset == null)
        {
            Debug.LogError("csv読み込みに失敗しました: " + path);
            return null;
        }

        var ary = CSVSerializer.Deserialize<T>(textAsset.text);
        if (ary == null)
        {
            Debug.LogError("csvデシリアライズに失敗しました: " + fileName);
            return null;
        }
        return ary;
    }



    static async UniTask<List<Dictionary<string, string>>> DeserializeAsync_StringDics(string fileName)
    {
        // パスに拡張子つけない
        string path = "CSV/" + fileName;
        var result = await Resources.LoadAsync<TextAsset>(path);
        var textAsset = result as TextAsset;
        if (textAsset == null) Debug.LogError("csv読み込みに失敗しました: " + path);

        var stringLists = CSVToStringLists(textAsset.text);
        var stringDics = CSVToStringDics(stringLists);

        //if (ary == null) Debug.LogError("csvデシリアライズに失敗しました: " + fileName);
        return stringDics;
    }
*/
    static List<Dictionary<string, string>> CSVToStringDics(List<string[]> stringLists)
    {
        List<Dictionary<string, string>> datas = new();
        string[] keys = stringLists[0];

        for (int y = 1; y < stringLists.Count; y++)
        {
            Dictionary<string, string> dic = new();

            for (int x = 0; x < keys.Length; x++)
            {
                dic[keys[x]] = stringLists[y][x];
            }
            datas.Add(dic);
        }

        return datas;
    }

    static List<string[]> CSVToStringLists(string csvText)
    {
        StringReader reader = new(csvText);
        List<string[]> csvDatas = new(); // CSVの中身を入れるリスト;

        // , で分割しつつ一行ずつ読み込み
        // リストに追加していく
        while (reader.Peek() != -1) // reader.Peaekが-1になるまで
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            csvDatas.Add(line.Split(',')); // , 区切りでリストに追加
        }
        return csvDatas;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.IO;

public static class CSVManager
{

    public static PartyData[] partyDatas { get; private set; }
    public static ArtSetData[] artSetDatas { get; private set; }
    public static ArtSubData[] artSubDatas { get; private set; }
    public static WeaponData[] weaponDatas { get; private set; }

    public static async UniTask InitializeAsync()
    {
        partyDatas = await DeserializeAsync<PartyData>("Chara");
        artSetDatas = await DeserializeAsync<ArtSetData>("ArtSet");
        artSubDatas = await DeserializeAsync<ArtSubData>("ArtSub");
        weaponDatas = await DeserializeAsync<WeaponData>("Weapon");
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
}

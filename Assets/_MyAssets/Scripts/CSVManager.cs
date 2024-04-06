using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.IO;
using System.Linq;

public static class CSVManager
{

    public static PartyData[] partyDatas { get; private set; }
    public static ArtSetData[] artSetDatas { get; private set; }
    public static ArtSetData[] artSetDatas_notSkipped { get; private set; }
    public static WeaponData[] weaponDatas { get; private set; }
    public static ArtifactData[] artifactDatas { get; private set; }

    public static async UniTask InitializeAsync()
    {
        partyDatas = await DeserializeAsync<PartyData>("Chara");
        partyDatas = partyDatas.Where(data => data.skip != 1).ToArray();

        artSetDatas_notSkipped = await DeserializeAsync<ArtSetData>("ArtSet");
        artSetDatas = artSetDatas_notSkipped.Where(data => data.skip != 1).ToArray();

        weaponDatas = await DeserializeAsync<WeaponData>("Weapon");
        weaponDatas = weaponDatas.Where(data => data.skip != 1).ToArray();

        artifactDatas = await DeserializeAsync<ArtifactData>("Artifacts");
        artifactDatas = artifactDatas.Where(data => data.skip != 1).ToArray();

    }

    public static async UniTask<T[]> DeserializeAsync<T>(string fileName)
    {
        // パスに拡張子つけない
        string path = "CSV/" + fileName;
        Debug.Log("CSVロード開始 " + path);

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

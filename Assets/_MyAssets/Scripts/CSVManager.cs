using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.IO;
using System.Linq;

public static class CSVManager
{

    public static MemberData[] MemberDatas { get; private set; }
    public static ArtSetData[] ArtSetDatas { get; private set; }
    public static WeaponData[] WeaponDatas { get; private set; }
    public static ArtifactData[] ArtifactDatas { get; private set; }

    public static async UniTask InitializeAsync()
    {
        MemberDatas = await DeserializeAsync<MemberData>("Members");

        ArtSetDatas = await DeserializeAsync<ArtSetData>("ArtSet");

        WeaponDatas = await DeserializeAsync<WeaponData>("Weapon");

        ArtifactDatas = await DeserializeAsync<ArtifactData>("Artifacts");

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

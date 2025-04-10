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
    public static ArtSetData[] ArtSetDatas_notSkipped { get; private set; }
    public static WeaponData[] WeaponDatas { get; private set; }
    public static ArtifactData[] ArtifactDatas { get; private set; }

    public static async UniTask InitializeAsync()
    {
        MemberDatas = await DeserializeAsync<MemberData>("Members");
        MemberDatas = MemberDatas.Where(data => data.skip != 1).ToArray();

        ArtSetDatas_notSkipped = await DeserializeAsync<ArtSetData>("ArtSet");
        ArtSetDatas = ArtSetDatas_notSkipped.Where(data => data.skip != 1).ToArray();

        WeaponDatas = await DeserializeAsync<WeaponData>("Weapon");
        WeaponDatas = WeaponDatas.Where(data => data.skip != 1).ToArray();

        ArtifactDatas = await DeserializeAsync<ArtifactData>("Artifacts");
        ArtifactDatas = ArtifactDatas.Where(data => data.skip != 1).ToArray();

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

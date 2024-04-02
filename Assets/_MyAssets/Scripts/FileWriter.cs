using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;
using System;
using System.Reflection;

public class FileWriter
{
    public static async void Save(string fileName, List<string> list)
    {
        if (list == null) return;
        Debug.Log("書き込み開始");

        string directoryPath = Application.dataPath + $"/_MyAssets/CSV/{fileName}/";
        fileName += ".csv";
        string path = directoryPath + fileName;
        // ディレクトリが存在しない場合は作成
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        using StreamWriter sw = File.CreateText(path);


        foreach (var line in list)
        {
            sw.WriteLine(line);
        }

        //AssetDatabase.Refresh();
        Debug.Log("生成完了 " + fileName);

        await UniTask.DelayFrame(1);
        UnityEditor.EditorApplication.isPlaying = false;
    }
}

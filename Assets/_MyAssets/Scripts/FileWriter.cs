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

        string directoryPath = DirectoryPath(folderName: fileName);
        // ディレクトリが存在しない場合は作成
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }


        try
        {
            string fullPath = FullPath(directoryPath, fileName);
            using StreamWriter sw = File.CreateText(fullPath);
            foreach (var line in list)
            {
                sw.WriteLine(line);
            }

            Debug.Log("生成完了 " + fileName);

            await UniTask.DelayFrame(1);
            UnityEditor.EditorApplication.isPlaying = false;
        }
        catch (System.Exception)
        {
            string fullPath = FullPath(directoryPath, fileName + "_1");
            using StreamWriter sw = File.CreateText(fullPath);
            foreach (var line in list)
            {
                sw.WriteLine(line);
            }

            Debug.Log("生成完了 " + fileName);

            await UniTask.DelayFrame(1);
            UnityEditor.EditorApplication.isPlaying = false;

        }




    }


    static string DirectoryPath(string folderName)
    {
        string directoryPath = Application.dataPath + $"/_MyAssets/CSV/{folderName}/";
        return directoryPath;
    }

    static string FullPath(string directoryPath, string fileName)
    {
        return directoryPath + fileName + ".csv";
    }


}

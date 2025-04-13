using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Threading.Tasks;


namespace PackageImporter
{
    [InitializeOnLoad]//エディター起動時に初期化されるように
    public class UnityPackageEvents
    {
        //コンストラクタ(InitializeOnLoad属性によりエディター起動時に呼び出される)
        static UnityPackageEvents()
        {
            AssetDatabase.importPackageCompleted += ImportCompleted;
            AssetDatabase.importPackageCancelled += ImportCancelled;
            AssetDatabase.importPackageFailed += ImportCallBackFailed;
            AssetDatabase.importPackageStarted += ImportStarted;
        }

        private static void ImportStarted(string packageName)
        {
            // Debug.Log(packageName + "のインポート開始");
        }

        private static void ImportCancelled(string packageName)
        {
            // Debug.Log(packageName + "のインポートキャンセル");
        }

        private static void ImportCallBackFailed(string packageName, string _error)
        {
            // Debug.Log(packageName + "のインポート失敗 : " + _error);
        }

        private static async void ImportCompleted(string packageName)
        {
            PackageSO packageSO = PackageSO.GetInstance();
            if (packageSO == null) return;
            if (packageSO.IsInitialized) return;
            packageSO.IsInitialized = true;
            await PackagesImporter.ImportAllPackagesAsync();
        }

    }
}
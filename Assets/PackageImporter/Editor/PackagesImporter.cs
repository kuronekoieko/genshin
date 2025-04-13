using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager;
using System.Threading.Tasks;
using UnityEditor.PackageManager.Requests;


namespace PackageImporter
{
    public class PackagesImporter
    {

        public static void FocusOnAsset()
        {
            PackageSO packageSO = PackageSO.GetInstance();

            if (packageSO != null)
            {
                Selection.activeObject = packageSO; // アセットを選択状態にする
                EditorGUIUtility.PingObject(packageSO); // ハイライト表示
            }
            else
            {
                Debug.LogError("Asset not found: ");
            }
        }

        public async static Task ImportAllPackagesAsync()
        {
            Debug.Log("パッケージインポート開始");

            PackageSO packageSO = PackageSO.GetInstance();
            await ImportPackagesAsync(packageSO.packageNames);
        }


        async static Task ImportPackagesAsync(List<string> packageNames)
        {
            var packageInfos = await PackagesGetter.GetPackageInfosAsync();

            foreach (var packageName in packageNames)
            {
                //Debug.Log("========================");

                if (string.IsNullOrEmpty(packageName)) continue;

                if (Contains(packageInfos, packageName)) continue;


                //  Debug.Log($"Installing package: {package}");
                await ClientAddAsync(packageName);
                // await Task.Yield();これだとログ消える
                await Task.Delay(1000);
            }
            Debug.Log("パッケージインポート終了");
            // GetWindow<PackageImporter>("MonoScript Selector");
        }


        static bool Contains(UnityEditor.PackageManager.PackageInfo[] packageInfos, string packageName)
        {
            foreach (var packageInfo in packageInfos)
            {
                if (packageInfo.name == packageName)
                {
                    // Debug.Log($"インストール済み: {packageName}");
                    return true;
                }

                string packageUrl = packageInfo.packageId.Replace(packageInfo.name + "@", "");
                if (packageUrl == packageName)
                {
                    // Debug.Log($"インストール済み: {packageName}");
                    return true;
                }
                // Debug.Log(packageInfo.version + " " + packageName);
            }
            return false;
        }

        public static async Task ClientAddAsync(string tgzFilePath)
        {

            Debug.Log("インポート開始: " + tgzFilePath);
            // パッケージを追加
            AddRequest request = Client.Add(tgzFilePath);

            // 非同期でリクエストの完了を待機
            while (!request.IsCompleted)
            {
                // Debug.Log("インポート中: " + tgzFilePath);

                await Task.Yield(); // 次のフレームまで待機
            }

            // 結果を確認
            if (request.Status == StatusCode.Success)
            {
                Debug.Log("パッケージが正常にインポートされました: " + tgzFilePath);
            }
            else
            {
                Debug.LogError("パッケージインポート中にエラーが発生しました: " + request.Error.message);
            }
        }


        async void A()
        {
            await ClientAddAsync("https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask");
            await Task.Yield();

            await ClientAddAsync("file:" + Application.dataPath + "/_IekoLibraryImporter/com.ba.ieko-library-1.0.0.tgz");


            string[] packages = new string[] {
                "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask",
                "com.unity.addressables",
                "com.unity.nuget.newtonsoft-json" };

            // Client.Add(packagePath + "/com.ba.ieko-library-1.0.0.tgz");
            AssetDatabase.Refresh();

        }
    }
}
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace PackageImporter
{
    public class PackageCreator
    {


        // パッケージのパス（package.json があるディレクトリ）
        static string packagePath = "Assets/_IekoLibrary";

        // [MenuItem("Test/export")]
        public static async Task B()
        {
            string packagePath = "Assets/_IekoLibrary";
            string targetFolder = "Assets/_IekoLibraryImporter";

            await ClientPackAsync(packagePath, targetFolder);
            AssetDatabase.Refresh();
        }

        public static async Task ClientPackAsync(string packageFolder, string targetFolder)
        {

            Debug.Log("エクスポート開始");
            // パッケージを追加
            PackRequest request = Client.Pack(packageFolder, targetFolder);

            // 非同期でリクエストの完了を待機
            while (!request.IsCompleted)
            {
                // Debug.Log("エクスポート中: " + packageFolder);

                await Task.Yield(); // 次のフレームまで待機
            }

            // 結果を確認
            if (request.Status == StatusCode.Success)
            {
                Debug.Log("パッケージが正常にエクスポートされました: " + packageFolder);
            }
            else
            {
                Debug.LogError("パッケージエクスポート中にエラーが発生しました: " + request.Error.message);
            }
        }



    }
}

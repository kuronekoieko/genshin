using UnityEngine;
using UnityEditor.PackageManager;
using System.Threading.Tasks;
using System.Linq;
//using IekoLibrary;

namespace PackageImporter
{
    public class PackagesGetter
    {
        /*
                public static async Task SetPackagesList()
                {
                    PackageSO.GetInstance().packageNames = await ClientListAsync();
                    EditorUtility.SetDirty(PackageSO.GetInstance());
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }*/

        public static async Task<PackageInfo[]> GetPackageInfosAsync()
        {
            // Debug.Log("インストール済みパッケージ: リスト取得");

            var request = Client.List();

            while (!request.IsCompleted)
            {
                await Task.Yield(); // 次のフレームまで待機
            }

            if (request.Status == StatusCode.Success)
            {
                PackageInfo[] packageInfos = request.Result.ToArray();

                string text = "";
                foreach (var packageInfo in packageInfos)
                {
                    // DebugUtility.LogJson(packageInfo);
                    //  Debug.Log(packageInfo.packageId.Replace(packageInfo.name + "@", ""));

                    text += packageInfo + "\n";
                }
                // Debug.Log("インストール済みパッケージ: 正常に取得されました");
                // Debug.Log(text);
                // DebugUtility.LogJson(packageInfos, isNewtonSoft: true);
                // DebugUtility.LogJson(packageInfos);

                //  DebugUtility.LogJson(packageInfos);


                return packageInfos;
            }
            else
            {
                Debug.LogError("インストール済みパッケージ: 取得中にエラーが発生しました: " + request.Error.message);
                return null;
            }
        }
    }
}
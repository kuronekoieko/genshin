using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace PackageImporter
{
    // [CreateAssetMenu(menuName = "PackageImporter/Create " + nameof(PackageSO), fileName = nameof(PackageSO))]
    public class PackageSO : ScriptableObject
    {
        public List<string> packageNames = new();
        public bool IsInitialized { get; set; }
        static PackageSO packageSO;
        public static PackageSO GetInstance()
        {
            if (packageSO) return packageSO;
            packageSO = Resources.Load<PackageSO>("PackageSO");
            return packageSO;
        }

        public void SetDefaultPackages()
        {
            List<string> defaultPackageNames = new() {
                "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask",
                "com.unity.addressables",
                "com.unity.nuget.newtonsoft-json",
                "https://github.com/neuecc/UniRx.git?path=Assets/Plugins/UniRx/Scripts",
                "com.unity.recorder",
                };
            packageNames = defaultPackageNames;
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public async Task SetPackagesList()
        {
            var packageInfos = await PackagesGetter.GetPackageInfosAsync();
            packageNames = packageInfos.Select(p => p.name).ToList();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

    }
}
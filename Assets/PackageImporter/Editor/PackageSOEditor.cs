using UnityEditor;
using UnityEngine;

using System.Collections.Generic;

namespace PackageImporter
{
    [CustomEditor(typeof(PackageSO))]//拡張するクラスを指定
    public class PackageSOEditor : Editor
    {
        bool isButtonShow = true;
        /// <summary>
        /// InspectorのGUIを更新
        /// </summary>
        public override async void OnInspectorGUI()
        {
            //元のInspector部分を表示
            base.OnInspectorGUI();

            if (isButtonShow == false) return;

            //ボタンを表示
            if (GUILayout.Button("Import All Packages"))
            {
                isButtonShow = false;
                // ArgumentException: You can only call GUI functions from inside OnGUI.
                // のエラーが出るため
                EditorApplication.delayCall += async () =>
                {
                    await PackagesImporter.ImportAllPackagesAsync();
                    isButtonShow = true;
                };
            }

            if (GUILayout.Button("Set Default Packages"))
            {
                isButtonShow = false;
                PackageSO.GetInstance().SetDefaultPackages();
                isButtonShow = true;
            }

            if (GUILayout.Button("Set Packages List"))
            {
                isButtonShow = false;
                await PackageSO.GetInstance().SetPackagesList();
                isButtonShow = true;
            }

        }



    }
}
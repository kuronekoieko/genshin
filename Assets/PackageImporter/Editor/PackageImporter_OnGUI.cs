using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;


namespace PackageImporter
{
    public class PackageImporter_OnGUI : EditorWindow
    {

        private Vector2 scrollPos;
        List<string> packageNames = new() {
                "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask",
                "com.unity.addressables",
                "com.unity.nuget.newtonsoft-json" };

        async void OnGUI()
        {
            EditorGUILayout.LabelField("Select a MonoScript:");



            // スクロールビューの開始
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(150));

            for (int i = 0; i < packageNames.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                packageNames[i] = EditorGUILayout.TextField($"Item {i + 1}:", packageNames[i]);
                if (GUILayout.Button("Remove", GUILayout.Width(70)))
                {
                    packageNames.RemoveAt(i);
                    break; // リストを変更したのでループを抜ける
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView(); // スクロールビューの終了
            if (GUILayout.Button("Add Item"))
            {
                packageNames.Add("");
            }

            //  余白を作りボタンを下に寄せる
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Import Packages", GUILayout.Height(50)))
            {
                Close();
                foreach (var package in packageNames)
                {
                    if (string.IsNullOrEmpty(package)) continue;
                    Debug.Log($"Installing package: {package}");
                    await PackagesImporter.ClientAddAsync(package);
                    await Task.Yield();
                }
                Debug.Log("インポート終了: ");
            }

        }

    }
}
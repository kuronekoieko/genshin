using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEditor;
using System.IO;


namespace IekoLibrary
{
	public class AddNameSpace : EditorWindow
	{
		private static DefaultAsset folder;
		private MonoScript selectedScript;


		// readonly static string namespaceName = Application.identifier;
		static string namespaceName = new_namespace;
		const string new_namespace = "new namespace";
		static bool isTest = false;

		public static void ExecTest()
		{
			Debug.Log("開始");
			isTest = true;
			GetWindow<AddNameSpace>("MonoScript Selector");
		}


		public static void Exec()
		{
			isTest = false;
			GetWindow<AddNameSpace>("Folder Selector");
		}


		void OnGUI()
		{
			if (isTest)
			{
				OnGUI_Script();
			}
			else
			{
				OnGUI_Folder();
			}

		}
		async void OnGUI_Script()
		{
			EditorGUILayout.LabelField("Select a MonoScript:");
			selectedScript = (MonoScript)EditorGUILayout.ObjectField(selectedScript, typeof(MonoScript), false);

			if (namespaceName == new_namespace) namespaceName = selectedScript ? selectedScript.name : new_namespace;

			EditorGUILayout.LabelField("Input a namespace:");

			namespaceName = EditorGUILayout.TextField(namespaceName);

			if (selectedScript == null) return;

			string ScriptPath = AssetDatabase.GetAssetPath(selectedScript);
			ScriptPath = Application.dataPath.Replace("Assets", "") + ScriptPath;

			EditorGUILayout.LabelField("Selected ScriptPath Path: " + ScriptPath);

			Vector2 buttonSize = new(200, 40);
			if (GUILayout.Button("Change All Font Assets", GUILayout.Width(buttonSize.x), GUILayout.Height(buttonSize.y)))
			{
				await RewriteFile(ScriptPath);
				AssetDatabase.Refresh();
				// EditorUtility.SetDirty();
				Debug.Log("完了 ");
			}


		}

		void OnGUI_Folder()
		{
			// テキストフィールドの表示と入力の取得
			EditorGUILayout.LabelField("Select a folder:");
			folder = EditorGUILayout.ObjectField(folder, typeof(DefaultAsset), false) as DefaultAsset;

			if (namespaceName == new_namespace) namespaceName = folder ? folder.name : new_namespace;

			EditorGUILayout.LabelField("Input a namespace:");
			namespaceName = EditorGUILayout.TextField(namespaceName);

			if (folder == null) return;

			string folderPath = AssetDatabase.GetAssetPath(folder);
			folderPath = Application.dataPath.Replace("Assets", "") + folderPath;
			EditorGUILayout.LabelField("Selected Folder Path: " + folderPath);

			Vector2 buttonSize = new Vector2(200, 40);
			if (GUILayout.Button("Change All Font Assets", GUILayout.Width(buttonSize.x), GUILayout.Height(buttonSize.y)))
			{
				RunAllDirectory(folderPath);
			}
		}



		static async void RunAllDirectory(string folderPath)
		{
			Debug.Log("開始");

			// folderPath = Application.dataPath + @"/_MyAssets/Scripts";
			// Debug.Log(Application.dataPath + @"/_MyAssets/Scripts");

			//return;

			List<string> fileNames = GetFileNames(folderPath);

			foreach (var path in fileNames)
			{
				await RewriteFile(path);
			}
			AssetDatabase.Refresh();
			Debug.Log("完了 ");
		}


		static List<string> GetFileNames(string folderPath)
		{
			// フォルダが存在しない場合は処理を中止
			if (!Directory.Exists(folderPath))
			{
				Debug.LogError("Folder does not exist: " + folderPath);
				return null;
			}
			List<string> fileNames = new();

			string[] directories = Directory.GetDirectories(folderPath);


			foreach (var directory in directories)
			{
				fileNames.AddRange(GetFileNames(directory));
				// Debug.Log(directory);
				/// フォルダ内の全ファイルのパスを取得
				string[] filePaths = Directory.GetFiles(directory);

				foreach (string filePath in filePaths)
				{
					string fileName = directory + "/" + Path.GetFileName(filePath);
					if (fileName.Contains(".meta")) continue;
					fileNames.Add(fileName);
					//  Debug.Log(fileName);
				}
			}


			return fileNames;
		}


		async static UniTask RewriteFile(string path)
		{
			using StreamReader sr = new(path);
			Debug.Log(path);

			string result = sr.ReadToEnd();
			// Debug.Log(result);

			var lines = result.Split("\n");

			string combined = "";
			bool isStartClass = false;
			for (int i = 0; i < lines.Length; i++)
			{
				string line = lines[i];

				// すでに追加済みなら何もしない
				if (line.Contains("namespace " + namespaceName))
				{
					return;
				}

				if (isStartClass == false)
				{
					bool isClass = line.Contains("class");
					//  Debug.Log(i + " " + line);
					// Debug.Log(i + " " + line.Length);

					bool isAttribute = line.Length > 0 && line[0] == '[';
					bool isOtherNamespace = line.Contains("namespace");
					// TODO: namespaceがネストした場合に、usingを書き換える

					if (isClass || isAttribute || isOtherNamespace)
					{
						isStartClass = true;
						combined += "namespace " + namespaceName + "\n{\n";
					}
				}

				string tab = "\t";
				if (isStartClass == false)
				{
					tab = "";
				}

				combined += tab + line + "\n";
			}
			combined = combined.TrimEnd();
			combined += "\n}";

			Debug.Log(path + "\n\n" + combined);

			sr.Dispose();
			using StreamWriter sw = new(path, false);
			await sw.WriteLineAsync(combined);
			sw.Dispose();
		}
	}
}

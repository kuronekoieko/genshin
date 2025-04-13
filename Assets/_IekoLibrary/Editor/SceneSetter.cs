using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.Reflection;


namespace IekoLibrary
{
	public class SceneSetter : EditorWindow
	{
		static void Save<T>(string fileName, List<T> list)
		{
			Debug.Log("書き込み開始");

			string path = Application.dataPath + @"/_Addressables/CSV/" + fileName + ".csv";
			using StreamWriter sw = File.CreateText(path);

			string titleLine = "";


			// クラスの変数を配列で取得
			FieldInfo[] fields = typeof(T).GetFields();

			// 配列を反復処理して各変数にアクセス
			foreach (FieldInfo field in fields)
			{
				//object value = field.GetValue(instance);
				//Debug.Log(field.Name + ": " + value);
				titleLine += field.Name + ",";
			}



			sw.WriteLine(titleLine);


			for (int i = 0; i < list.Count; i++)
			{
				var instance = list[i];
				string line = "";
				foreach (FieldInfo field in fields)
				{
					// Debug.Log(field.FieldType.IsArray);
					// メンバが配列の場合は要素ごとに処理
					if (field.FieldType.IsArray)
					{
						Array arrayValue = (Array)field.GetValue(instance);
						for (int j = 0; j < arrayValue.Length; j++)
						{
							line += arrayValue.GetValue(j) + ",";
						}
					}
					else
					{
						// ダブルクオートは、改行コード対策のため
						// https://teratail.com/questions/339086
						line += "\"" + field.GetValue(instance) + "\"" + ",";
					}
				}

				sw.WriteLine(line);
			}


			AssetDatabase.Refresh();
			Debug.Log("生成完了 " + fileName);
		}
	}
}

using UnityEditor;
using UnityEngine;
using System.Linq;
using System;
using UnityEditor.SceneManagement;


namespace IekoLibrary
{
	public class ATTChecker
	{
		static readonly string targetClassName = "ATTManager, Assembly-CSharp";

		public static void Check()
		{
			Type attManagerType = Type.GetType(targetClassName);
			if (attManagerType == null)
			{
				Debug.LogError($"ATTManagerがインポートされていません");
				return;
			}

			if (!ExistsATTManagerInScenes(attManagerType))
			{
				Debug.LogError("ATTManagerがシーンに存在しません");
				return;
			}
		}


		static bool ExistsATTManagerInScenes(Type attManagerType)
		{

			// ビルド設定に含まれるシーンを取得
			var scenes = EditorBuildSettings.scenes
											.Where(scene => scene.enabled)
											.Select(scene => scene.path)
											.ToArray();


			foreach (var scenePath in scenes)
			{
				var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
				var rootGameObjects = scene.GetRootGameObjects();

				foreach (var gameObject in rootGameObjects)
				{
					var components = gameObject.GetComponentsInChildren(attManagerType, true);
					if (components.Any())
					{
						// シーン内にATTManagerが見つかった場合、シーンを閉じてtrueを返す
						EditorSceneManager.CloseScene(scene, true);
						return true;
					}
				}

				// シーンを閉じる
				EditorSceneManager.CloseScene(scene, true);
			}

			return false;
		}
	}
}

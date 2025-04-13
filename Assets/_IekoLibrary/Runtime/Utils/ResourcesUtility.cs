using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.IO;


namespace IekoLibrary
{
	public static class ResourcesUtility
	{
		/// <summary>
		/// 頭の/不要。拡張子ついててもついてなくてもOK
		/// </summary>
		public static async UniTask<T> LoadAsync<T>(string path) where T : UnityEngine.Object
		{
			path = GetPathWithoutExtension(path);
			UnityEngine.Object obj = await Resources.LoadAsync<T>(path);
			T asset = obj as T;
			if (asset == null)
			{
				Debug.LogError("Resources" + path);
			}
			return asset;
		}

		/// <summary>
		/// 頭の/不要。拡張子ついててもついてなくてもOK
		/// </summary>
		public static T Load<T>(string path) where T : UnityEngine.Object
		{
			path = GetPathWithoutExtension(path);
			UnityEngine.Object obj = Resources.Load<T>(path);
			T asset = obj as T;
			return asset;
		}

		static string GetPathWithoutExtension(string path)
		{
			var extension = Path.GetExtension(path);
			if (string.IsNullOrEmpty(extension))
			{
				return path;
			}
			return path.Replace(extension, string.Empty);
		}
	}
}

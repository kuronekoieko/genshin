using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.AddressableAssets;
using System.Linq;


namespace IekoLibrary
{
	public static class AddressablesLoader
	{
		static readonly string pathHeader = "Assets/_Addressables/";
		static readonly string remoteLoadPath = "";

		public static async UniTask<T> LoadAsync<T>(string address, bool excludeHeader = false) where T : UnityEngine.Object
		{
			//Addressables.WebRequestOverride = EditWebRequestURL;

			// Debug.Log("ロード開始 " + address);

			string path = excludeHeader ? address : pathHeader + address;
			bool existsPath = await ExistsPathAsync(path);

			if (!existsPath)
			{
				Debug.LogWarning("パスが存在しません :\n" + path + "\n");
				return null;
			}

			T asset = null;

			try
			{
				asset = await Addressables.LoadAssetAsync<T>(path);
				// Debug.Log("ロード成功 " + address);
			}
			catch (Exception e)
			{
				Debug.LogWarning("ロード失敗\n" + path + "\n" + e);
			}

			return asset;
		}

		// プレハブはGameObject型じゃないとエラー
		public static async UniTask<T[]> LoadAllAsync<T>(string label) where T : UnityEngine.Object
		{
			// Debug.Log("ロード開始 " + address);

			bool existsPath = await ExistsPathAsync(label);

			if (!existsPath)
			{
				Debug.LogWarning("ラベルが存在しません :\n" + label + "\n");
				return null;
			}

			IList<T> IList = await Addressables.LoadAssetsAsync<T>(label, null).Task;
			T[] asset = null;

			if (IList == null)
			{
				Debug.LogWarning("ロード失敗\n" + label);
			}
			else
			{
				asset = IList.ToArray();
			}

			return asset;
		}

		public static async UniTask<bool> ExistsPathAsync(string path)
		{
			var a = await Addressables.LoadResourceLocationsAsync(path);
			bool exists = a.Any();
			return exists;
		}


		// https://gist.github.com/anmq0502/a229a048f27c91e775aeabf40517a1bd
		// firebaseはそのままダウンロードできない
		static void EditWebRequestURL(UnityWebRequest request)
		{
			Debug.Log("EditWebRequestURL " + request.url);

			bool isBundle = request.url.EndsWith(".bundle", StringComparison.OrdinalIgnoreCase);
			bool isJson = request.url.EndsWith(".json", StringComparison.OrdinalIgnoreCase);
			bool isHash = request.url.EndsWith(".hash", StringComparison.OrdinalIgnoreCase);

			if (isBundle || isJson || isHash)
			{
				string fileName = System.IO.Path.GetFileName(request.url);
				request.url = remoteLoadPath + fileName + "?alt=media";
				Debug.Log("EditWebRequestURL " + request.url);
			}
		}

		public static class Label
		{
			public static string Default = "default";
			public static string Screens = "Screens";


		}
	}
}

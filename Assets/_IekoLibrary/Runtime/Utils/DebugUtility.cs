using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Linq;


namespace IekoLibrary
{
	public static class DebugUtility
	{
		public static void LogJson<T>(T obj, bool isIndented = true, bool isNewtonSoft = false)
		{

			string json = GetJson(obj, isIndented, isNewtonSoft);
			Debug.Log(json);
		}


		public static void LogJson<T>(string text, T obj, bool isIndented = true, bool isNewtonSoft = false)
		{
			string json = GetJson(obj, isIndented, isNewtonSoft);
			Debug.Log(text + "\n" + json);
		}

		public static void LogJsonError<T>(string text, T obj, bool isIndented = true, bool isNewtonSoft = false)
		{
			string json = GetJson(obj, isIndented, isNewtonSoft);
			Debug.LogError(text + "\n" + json);

		}

		public static void LogJson<T>(T[] objs, bool isIndented = true, bool isNewtonSoft = false)
		{
			if (isNewtonSoft)
			{
				Debug.Log(GetJson(objs, isIndented, isNewtonSoft));
			}
			else
			{
				Debug.Log(JsonUtilityToJson(objs, isIndented));
			}
		}

		public static void LogJson<T>(List<T> objs, bool isIndented = true, bool isNewtonSoft = false)
		{
			LogJson(objs.ToArray(), isIndented, isNewtonSoft);
		}

		public static void LogJsonForEach<T>(T[] objs, bool isIndented = true, bool isNewtonSoft = false)
		{
			foreach (var obj in objs)
			{
				LogJson(obj, isIndented, isNewtonSoft);
			}
		}

		public static void LogJsonForEach<T>(List<T> objs, bool isIndented = true, bool isNewtonSoft = false)
		{
			LogJsonForEach(objs.ToArray(), isIndented, isNewtonSoft);
		}

		static string GetJson<T>(T obj, bool isIndented, bool isNewtonSoft)
		{
			if (isNewtonSoft)
			{
				return JsonConvert.SerializeObject(obj, isIndented ? Formatting.Indented : Formatting.None);
			}
			else
			{
				// 配列対応してない
				return JsonUtility.ToJson(obj, isIndented);
			}
		}

		static string JsonUtilityToJson<T>(T[] objs, bool isIndented = true)
		{
			string str = "[\n";
			foreach (T obj in objs)
			{
				string json = JsonUtility.ToJson(obj, isIndented);
				str += json + ",\n";
			}
			str += "\n]";
			return str;
		}




		public static void DrawRay2D(Ray2D ray, float distance = 100f)
		{
			Debug.DrawLine(ray.origin, ray.origin + ray.direction * distance, Color.red);
		}
	}
}

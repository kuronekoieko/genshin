using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;



namespace IekoLibrary
{
	public static class ListExtensions
	{
		public static List<T> ReverseList<T>(this List<T> self)
		{
			if (self == null) return null;

			List<T> items = new();
			for (int i = self.Count - 1; i >= 0; i--)
			{
				items.Add(self[i]);
			}
			return items;
		}

		public static List<T> Where<T>(this List<T> self, Func<T, bool> predicate)
		{
			if (self == null) return null;

			List<T> list = new();
			foreach (var item in self)
			{
				if (predicate(item))
				{
					list.Add(item);
				}
			}
			return list;
		}


		public static T Pop<T>(this List<T> self)
		{
			if (self == null) return default;
			if (self.Count == 0) return default;

			var t = self[0];
			self.Remove(t);
			return t;
		}

		public static T PopRandom<T>(this List<T> self, Func<T, bool> ignore)
		{
			if (self == null) return default;

			if (self.Count == 0) return default;
			bool invalid = self.All(item => ignore(item));
			if (invalid)
			{
				Debug.LogWarning("抽選に失敗");
				return default;
			}

			T t = self.GetRandom();
			while (ignore(t))
			{
				t = self.GetRandom();
			}

			self.Remove(t);
			return t;
		}

		public static T PopRandom<T>(this List<T> self)
		{
			if (self == null) return default;

			if (self.Count == 0) return default;
			T t = self.GetRandom();
			self.Remove(t);
			return t;
		}

		public static List<T> PopList<T>(this List<T> self, int count)
		{
			if (self == null) return default;
			if (self.Count == 0) return default;
			if (count == -1) return default;

			var list = self.Take(count).ToList();
			self.RemoveRange(0, count);
			return list;
		}

		public static T GetRandom<T>(this List<T> self, out int index)
		{
			index = default;
			if (self == null) return default;

			index = UnityEngine.Random.Range(0, self.Count);
			return self[index];
		}

		public static T GetRandom<T>(this List<T> self)
		{
			if (self == null) return default;

			int index = UnityEngine.Random.Range(0, self.Count);
			return self[index];
		}

		public static List<T> GetRandomList<T>(this List<T> self, int count = -1)
		{
			if (self == null) return default;
			if (count == -1)
			{
				return self.OrderBy(a => Guid.NewGuid()).ToList();
			}
			else
			{
				return self.OrderBy(a => Guid.NewGuid()).Take(count).ToList();
			}
		}

		public static T GetRandom<T>(this T[] self, out int index)
		{
			index = default;
			if (self == null) return default;

			index = UnityEngine.Random.Range(0, self.Length);
			return self[index];
		}

		public static T GetRandom<T>(this T[] self)
		{
			if (self == null) return default;

			int index = UnityEngine.Random.Range(0, self.Length);
			return self[index];
		}


		public static T[] GetRandomArray<T>(this List<T> self, int count = -1)
		{
			if (self == null) return default;
			if (count == -1)
			{
				return self.OrderBy(a => Guid.NewGuid()).ToArray();
			}
			else
			{
				return self.OrderBy(a => Guid.NewGuid()).Take(count).ToArray();
			}
		}
		public static T[] GetRandomArray<T>(this T[] self, int count = -1)
		{
			if (self == null) return default;
			if (count == -1)
			{
				return self.OrderBy(a => Guid.NewGuid()).ToArray();
			}
			else
			{
				return self.OrderBy(a => Guid.NewGuid()).Take(count).ToArray();
			}
		}

		public static bool TryGetValue<T>(this T[] self, int index, out T value)
		{
			if (self == null)
			{
				value = default;
				return false;
			}
			if (self.IsIndexOutOfRange<T>(index))
			{
				value = default;
				return false;
			}
			else
			{
				value = self[index];
				return true;
			}
		}

		public static bool TryGetValue<T>(this T[,] self, int indexX, int indexY, out T value)
		{
			if (self == null)
			{
				value = default;
				return false;
			}
			if (self.IsIndexOutOfRange<T>(indexX, indexY))
			{
				value = default;
				return false;
			}
			else
			{
				value = self[indexX, indexY];
				return true;
			}
		}

		public static bool IsIndexOutOfRange<T>(this T[,] self, int indexX, int indexY)
		{
			if (self == null) return true;

			bool isOutX = indexX < 0 || self.GetLength(0) <= indexX;
			bool isOutY = indexY < 0 || self.GetLength(1) <= indexY;

			return isOutX || isOutY;
		}

		public static bool IsIndexOutOfRange<T>(this T[] self, int index)
		{
			if (self == null) return true;

			return index < 0 || self.Length <= index;
		}

		public static bool TryGetValue<T>(this List<T> self, int index, out T value)
		{
			if (self == null)
			{
				value = default;
				return false;
			}
			if (self.IsIndexOutOfRange<T>(index))
			{
				value = default;
				return false;
			}
			else
			{
				value = self[index];
				return true;
			}
		}

		public static bool IsIndexOutOfRange<T>(this List<T> self, int index)
		{
			if (self == null) return true;

			return index < 0 || self.Count <= index;
		}

		/// <summary>
		/// インデックスが要素数を超えていたらクランプする
		/// </summary>
		/// <param name="self"></param>
		/// <param name="index"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T ClampIndex<T>(this List<T> self, int index)
		{
			if (self == null) return default;

			if (self.Count == 0) return default;

			if (index <= 0)
			{
				return self[0];
			}

			if (self.Count <= index)
			{
				return self[self.Count - 1];
			}

			return self[index];
		}

		public static bool IsLast<T>(this List<T> self, int index)
		{
			if (self == null) return default;

			return self.Count - 1 == index;
		}

		public static float Median(this IEnumerable<float> source)
		{
			if (source is null || !source.Any())
			{
				return 0;
			}

			var sortedList =
				source.OrderBy(number => number).ToList();

			int itemIndex = sortedList.Count / 2;

			if (sortedList.Count % 2 == 0)
			{
				// Even number of items.
				return (sortedList[itemIndex] + sortedList[itemIndex - 1]) / 2;
			}
			else
			{
				// Odd number of items.
				return sortedList[itemIndex];
			}
		}

		public static float Median(this IEnumerable<int> source)
		{
			if (source is null || !source.Any())
			{
				return 0;
			}

			var sortedList =
				source.OrderBy(number => number).ToList();

			int itemIndex = sortedList.Count / 2;

			if (sortedList.Count % 2 == 0)
			{
				// Even number of items.
				return (sortedList[itemIndex] + sortedList[itemIndex - 1]) / 2;
			}
			else
			{
				// Odd number of items.
				return sortedList[itemIndex];
			}
		}
	}
}

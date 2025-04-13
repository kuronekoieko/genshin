using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IekoLibrary
{
	public class ObjectPooling<T> : MonoBehaviour where T : ObjectPoolingElement
	{
		[SerializeField] T prefab;
		[SerializeField] protected Transform parent;

		[NonSerialized] public List<T> List = new();// ゲッターでnew()すると、リストが常に空になる

		public virtual void OnStart()
		{
			// 保存されてるプレハブが変更される
			// this.prefab.Hide();
			HideDummies();
		}

		void HideDummies()
		{
			T[] dummies = parent.GetComponentsInChildren<T>();
			foreach (var dummy in dummies)
			{
				dummy.Hide();
			}
		}

		public void Clear()
		{
			foreach (var item in List)
			{
				item.Hide();
			}
		}

		protected T GetInstance()
		{
			T instance = null;
			foreach (var item in List)
			{
				if (item.gameObject.activeSelf == false)
				{
					instance = item;
					break;
				}
			}
			if (instance == null)
			{
				instance = Instantiate(prefab, parent ? parent : transform);
				instance.OnInstantiate();
				instance.gameObject.name += " " + List.Count;
				List.Add(instance);
			}
			instance.gameObject.SetActive(true);
			return instance;
		}
	}
}

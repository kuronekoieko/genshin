using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IekoLibrary
{
	public class SingletonMonoBehaviour<T> : MonoBehaviour where T : Component
	{
		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindFirstObjectByType<T>(FindObjectsInactive.Include);
				}
				return instance;
			}
		}
		private static T instance;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IekoLibrary
{
	public abstract class ObjectPoolingElement : MonoBehaviour
	{
		public abstract void OnInstantiate();
		public abstract void Hide();

	}
}

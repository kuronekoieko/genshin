using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IekoLibrary
{
	public static class TransformExtensions
	{
		public static void SetTagInChildren(this Transform parent, string tag)
		{
			parent.tag = tag;
			foreach (Transform child in parent)
			{
				child.SetTagInChildren(tag);
			}
		}
	}
}

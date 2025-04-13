using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IekoLibrary
{
	public static class Vector3Extensions
	{
		public static float DistanceFromZero(this Vector3 self)
		{
			return Vector3.Distance(self, Vector3.zero);
		}
	}
}

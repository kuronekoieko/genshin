using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IekoLibrary
{
	public static class RigidBodyExtensions
	{
		public static void SetVelocityX(this Rigidbody self, float x)
		{
			if (float.IsNaN(x))
			{
				Debug.LogError("RigidBody.velocity.x is NaN");
				return;
			}
			var vel = self.linearVelocity;
			vel.x = x;
			self.linearVelocity = vel;
		}
		public static void SetVelocityY(this Rigidbody self, float y)
		{
			if (float.IsNaN(y))
			{
				Debug.LogError("RigidBody.velocity.y is NaN");
				return;
			}
			var vel = self.linearVelocity;
			vel.y = y;
			self.linearVelocity = vel;
		}

		public static void SetVelocityZ(this Rigidbody self, float z)
		{
			if (float.IsNaN(z))
			{
				Debug.LogError("RigidBody.velocity.z is NaN");
				return;
			}
			var vel = self.linearVelocity;
			vel.z = z;
			self.linearVelocity = vel;
		}
	}
}

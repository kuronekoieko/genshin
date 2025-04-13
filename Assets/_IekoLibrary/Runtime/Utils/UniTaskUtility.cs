using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

namespace IekoLibrary
{
	public class UniTaskUtility
	{
		public async static UniTask DelaySecond(float second, CancellationTokenSource _tokenSource = null)
		{
			float f = second * 1000;
			if (_tokenSource == null)
			{
				await UniTask.Delay((int)f);
			}
			else
			{
				try
				{
					await UniTask.Delay((int)f, cancellationToken: _tokenSource.Token);
				}
				catch (System.Exception)
				{

				}
			}
		}
	}
}

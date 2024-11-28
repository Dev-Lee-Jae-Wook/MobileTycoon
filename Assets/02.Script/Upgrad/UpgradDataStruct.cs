using System;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Upgrad
{
	[Serializable]
	public struct UpgradDataStruct<T>
	{
		public int Cost;
		public T Value;
	}
}
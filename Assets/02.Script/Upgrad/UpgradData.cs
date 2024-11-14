using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Upgrad
{
	[CreateAssetMenu(fileName = "NewUpgradData", menuName = "CustomData/UpgradData")]
	public class UpgradData : ScriptableObject
	{
		public  List<UpgradDataStruct> UpgradList;
	}
}
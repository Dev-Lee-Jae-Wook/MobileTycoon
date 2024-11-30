using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Upgrad
{
	[CreateAssetMenu(fileName = "NewUpgradDataFloat", menuName = "CustomData/UpgradDataFloat")]
	public class UpgradData_Float : ScriptableObject
	{
		public  List<UpgradDataStruct<float>> UpgradList;
	}
}
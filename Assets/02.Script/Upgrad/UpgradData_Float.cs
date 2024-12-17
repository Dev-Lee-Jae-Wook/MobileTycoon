using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Upgrad
{
	[CreateAssetMenu(fileName = "NewUpgradDataFloat", menuName = "CustomData/UpgradDataFloat")]
	public class UpgradData_Float : ScriptableObject
	{
		[SerializeField] private  List<UpgradDataStruct<float>> _upgradList;

		public UpgradDataStruct<float> GetUpgradData(int lv)
		{
			return _upgradList[lv];
		}

		public int GetMaxLv()
		{
			return _upgradList.Count - 1;
		}
	}
}
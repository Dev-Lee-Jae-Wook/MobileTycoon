using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Upgrad
{
	[CreateAssetMenu(fileName = "NewUpgradDataInt", menuName = "CustomData/UpgradDataInt")]
	public class UpgradData_Int : ScriptableObject
	{
		 [SerializeField] private  List<UpgradDataStruct<int>> _upgradList;

		public UpgradDataStruct<int> GetUpgradData(int lv)
		{
			return _upgradList[lv];
		}

		public int GetMaxLv()
		{
			return _upgradList.Count - 1;
		}
	}
}
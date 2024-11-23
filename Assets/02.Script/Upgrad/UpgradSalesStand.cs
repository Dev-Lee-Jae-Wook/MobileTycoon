using EverythingStore.AssetData;
using EverythingStore.InteractionObject;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EverythingStore.Upgrad
{
	public class UpgradSalesStand : MonoBehaviour
	{
		#region Field
		[SerializeField] private UpgradData _data;
		[SerializeField] private SaleStandPivotData[] _pivotDatas;
		[SerializeField] private UpgradArea _upgradArea;
		private SalesStand _salesStand;
		private int _upgradCount = 0;
		#endregion

		#region Property
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Start()
		{
			_salesStand = GetComponent<SalesStand>();
			_upgradArea.SetupTarget(_data.Name, _data.UpgradList[_upgradCount].Cost, Upgrad);
		}
		#endregion

		#region Public Method
		#endregion

		#region Private Method
		[Button("Upgrad")]
		private void Upgrad()
		{
			int capacity = _data.UpgradList[_upgradCount].Value;
			var pivotData = _pivotDatas[_upgradCount];
			_salesStand.Upgrad(_upgradCount, capacity, pivotData);
			_upgradCount++;
			
			if(_upgradCount < _data.UpgradList.Count)
			{
				_upgradArea.SetupTarget(_data.Name, _data.UpgradList[_upgradCount].Cost, Upgrad);
			}
			else
			{
				_upgradArea.Max();
			}

		}
		#endregion

		#region Protected Method
		#endregion

	}
}

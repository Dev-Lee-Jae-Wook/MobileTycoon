using EverythingStore.InteractionObject;
using EverythingStore.Upgrad;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Manger
{
    public class SalesStandManager : MonoBehaviour
    {
		#region Field
		private int _count;
		[SerializeField] private SalesStand[] _salesStands;
		[SerializeField] private UpgradArea[] _upgradArea;
		[SerializeField] private LockArea[] _lockAreas;
		#endregion

		#region Property
		public SalesStand[] SalesStands => _salesStands;
		#endregion

		#region Event
		#endregion

		#region UnityCycle

		private void Start()
		{
			foreach (var item in _upgradArea)
			{
				item.OnMax += LockArea;
			}

			foreach(var item in _lockAreas)
			{
				item.gameObject.SetActive(false);
			}
		}
		#endregion

		#region Public Method
		public SalesStand EnterSalesStand()
		{
			SalesStand saleStand = null;
			for(int i = 0; i < _salesStands.Length && saleStand == null; i++)
			{
				if (_salesStands[i].IsEnterable() == true)
				{
					saleStand = _salesStands[i];
				}
			}
			return saleStand;
		}
		#endregion

		#region Private Method
		private void LockArea()
		{
			_lockAreas[_count].gameObject.SetActive(true);
			_count++;
		}
		#endregion

		#region Protected Method
		#endregion

	}
}

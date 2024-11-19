using EverythingStore.InteractionObject;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Upgrad
{
	public class BoxUpgrad : MonoBehaviour
	{
		#region Field
		[SerializeField] private UpgradArea _subtractMoneyArea;
		[SerializeField] private UpgradData _data;
		private int _lv = 0;
		#endregion

		#region Property
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Awake()
		{
			_subtractMoneyArea.SetupTarget(_data.Name,_lv, _data.UpgradList[_lv].Cost, Upgrad);
		}
		#endregion

		#region Public Method

		#endregion

		#region Private Method
		private void Upgrad()
		{
			_lv++;
			Debug.Log($"Upgrad {_lv}");
			if(_lv == _data.UpgradList.Count )
			{
				return;
			}

			_subtractMoneyArea.SetupTarget(_data.Name, _lv, _data.UpgradList[_lv].Cost, Upgrad);
		}
		#endregion

		#region Protected Method
		#endregion
	}
}
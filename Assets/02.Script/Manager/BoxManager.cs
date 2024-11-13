using EverythingStore.InteractionObject;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Manager
{
	public class BoxManger : MonoBehaviour
	{
		#region Field
		[SerializeField] private SubtractMoneyArea _subtractMoneyArea;
		[SerializeField] private SubtrackMoneyData _data;
		private int _lv = 0;
		#endregion

		#region Property
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Awake()
		{
			_subtractMoneyArea.SetupTarget(_data.TargetMoneyList[_lv], Upgrad);
		}
		#endregion

		#region Public Method

		#endregion

		#region Private Method
		private void Upgrad()
		{
			_lv++;
			Debug.Log($"Upgrad {_lv}");
			if(_lv == _data.TargetMoneyList.Count )
			{
				return;
			}

			_subtractMoneyArea.SetupTarget(_data.TargetMoneyList[_lv], Upgrad);
		}
		#endregion

		#region Protected Method
		#endregion
	}
}
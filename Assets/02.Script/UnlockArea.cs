using EverythingStore.InteractionObject;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace EverythingStore.InputMoney
{
	public class UnlockArea : MonoBehaviour
	{
		#region Field
		[SerializeField] private bool _isUnlock = false;
		[SerializeField] private InputMoneyArea _area;
		[SerializeField] private int _targetMoney;
		[SerializeField] private GameObject _unlockTarget;
		#endregion

		#region Property
		public event Action OnUnlock;
		public bool IsUnlock => _isUnlock;
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Start()
		{
			if (_isUnlock == false)
			{
				_area.SetUp(_targetMoney);
				_area.OnCompelte += () => _unlockTarget.SetActive(true);
				_area.OnCompelte += () => gameObject.SetActive(false);
				_area.OnCompelte += () => OnUnlock?.Invoke();

				_unlockTarget.SetActive(false);
			}
			else
			{
				_area.gameObject.SetActive(false);
				_unlockTarget.SetActive(true);
			}
		}
		#endregion
	}
}

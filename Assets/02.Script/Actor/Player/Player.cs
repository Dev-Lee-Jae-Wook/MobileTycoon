using EverythingStore.InteractionObject;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Actor.Player
{
    public class Player : MonoBehaviour
	{
		#region Field
		private PickupAndDrop _pickupAndDrop;
		[SerializeField] private Transform _getItemPoint;
		[SerializeField] private int _money;
		#endregion

		#region Property
		public Transform GetItemPoint => _getItemPoint;
		public PickupAndDrop PickupAndDrop => _pickupAndDrop;
		public int Money => _money;
		#endregion

		#region Event
		public event Action<int> OnMoneyChange;
		#endregion

		#region UnityCycle
		private void Awake()
		{
			_pickupAndDrop = GetComponent<PickupAndDrop>();
		}
		#endregion

		#region Public Method
		public void AddMoney(int money)
		{
			_money += money;
			OnMoneyChange?.Invoke(_money);
		}

		
		public void SubtractMoney(int money)
		{
			_money -= money;
			OnMoneyChange?.Invoke(_money);
		}
		#endregion

		#region Private Method
		#endregion

		#region Protected Method
		#endregion

	}
}

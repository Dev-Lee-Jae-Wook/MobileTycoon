using EverythingStore.InteractionObject;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.Port;

namespace EverythingStore.Actor.Player
{
    public class Player : MonoBehaviour
	{
		#region Field
		[SerializeField] private int _initMoney; 
		[SerializeField] private Transform _getItemPoint;
		private PlayerCharacterMovement _movement;
		private PickupAndDrop _pickupAndDrop;
		private Wallet _wallet;
		#endregion

		#region Property
		public Transform GetItemPoint => _getItemPoint;
		public PickupAndDrop PickupAndDrop => _pickupAndDrop;
		public Wallet Wallet => _wallet;
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Awake()
		{
			_pickupAndDrop = GetComponent<PickupAndDrop>();
			_movement = GetComponent<PlayerCharacterMovement>();
			_wallet = new (_initMoney);
		}
		#endregion

		#region Public Method

		public void SetSpeed(float speed)
		{
			_movement.Speed = speed;
		}

		public void SetPickupCapcity(int capacity)
		{
			_pickupAndDrop.maxPickup = capacity;
		}
		#endregion

		#region Private Method
		#endregion

		#region Protected Method
		#endregion

	}
}

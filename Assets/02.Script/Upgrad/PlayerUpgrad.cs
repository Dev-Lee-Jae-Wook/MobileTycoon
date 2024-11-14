using EverythingStore.Actor.Player;
using EverythingStore.InteractionObject;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace EverythingStore.Upgrad
{
	public class PlayerUpgrad : MonoBehaviour
	{
		#region Field
		[Title("Target")]
		[SerializeField]private Player _player;
		
		[Title("Speed")]
		[SerializeField] private SubtractMoneyArea _speedUpgradArea;
		[SerializeField] private UpgradData _speed;

		[Title("Pickup")]
		[SerializeField] private SubtractMoneyArea _pickupUpgradArea;
		[SerializeField] private UpgradData _pickup;

		private int _speedLv;
		private int _pickupLv;
		#endregion

		#region Property
		#endregion
		
		#region UnityCycle
		private void Start()
		{
			_player.SetSpeed(3.0f);
			//_player.SetPickupCount(1);

			_speedUpgradArea.SetupTarget(_speedLv,_speed.UpgradList[_speedLv].Cost, UpgradSpeed);
			//_pickupUpgradArea.SetupTarget(_pickupLv,_pickup.UpgradList[_pickupLv].Cost, UpgradPickupCount);
		}
		#endregion

		#region Private Method
		private void UpgradSpeed()
		{
			_speedLv++;
			_player.SetSpeed(_speed.UpgradList[_speedLv].Value);
			_speedUpgradArea.SetupTarget(_speedLv, _speed.UpgradList[_speedLv].Cost, UpgradSpeed);
		}

		//private void UpgradPickupCount()
		//{
		//	_pickupLv++;
		//	_player.SetPickupCount((int)_pickup.UpgradList[_pickupLv].Value);
		//	_speedUpgradArea.SetupTarget(_pickupLv, _pickup.UpgradList[_pickupLv].Cost, UpgradPickupCount);
		//}
		#endregion
	}
}

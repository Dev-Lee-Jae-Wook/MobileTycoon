using EverythingStore.Actor.Player;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Upgrad
{
    public class UpgradPlayer : PopupUIBase
    {
		#region Field
		[Title("Collaboration")]
		[SerializeField] private Player _player;

		[Title("Speed")]
		[SerializeField] private Sprite _speedIcon;
		[SerializeField] private UpgreadItem _speed;
		[SerializeField] private UpgradData_Float _speedData;
		[ReadOnly][SerializeField] private int _lvSpeed = 0;
		private int _speedNextCost;

		[Title("Pickup")]
		[SerializeField] private Sprite _pickupIcon;
		[SerializeField] private UpgreadItem _pickup;
		[SerializeField] private UpgradData_Int _pickupData;
		[ReadOnly][SerializeField] private int _lvPickup;
		private int _pickUpNextCost;

		private Wallet _wallet;
		#endregion

		#region Property
		#endregion

		#region Event

		#endregion

		#region UnityCycle
		#endregion

		#region Public Method

		#endregion

		#region Private Method
		private string GetSpeedDescriptaion()
		{
			return $"Speed : {_speedData.UpgradList[_lvSpeed].Value}";
		}	
		
		private string GetPickupDescriptaion()
		{
			//return $"Max Pickup : {_pickupData._upgradList[_lvPickup].Value}";
			return null;
		}

		private string GetNextSpeedCost()
		{
			if(IsSpeedMax() == true)
			{
				return "Max";
			}

			return _speedNextCost.ToString();
		}
		
		private string GetNextPickupCost()
		{
			if(IsPickUpMax() == true)
			{
				return "Max";
			}

			return _pickUpNextCost.ToString();
		}

		private void TrySpeedUpgrad()
		{
			if (IsSpeedMax() == true)
			{
				return;
			}

			if (_wallet.Money < _speedNextCost)
			{
				return;
			}

			_lvSpeed++;
			_player.SetSpeed(_speedData.UpgradList[_lvSpeed].Value);
			_player.SetSpeedLV(_lvSpeed);
			UpdateNextSpeedCost();

			_speed.UpdateItem(GetSpeedDescriptaion(), GetNextSpeedCost());
			_player.Save();
		}

		private void TryPickupUpgrad()
		{
			if (IsPickUpMax() == true)
			{
				return;
			}

			if (_wallet.Money < _pickUpNextCost)
			{
				return;
			}

			_lvPickup++;
			//_player.SetPickupCapcity(_pickupData._upgradList[_lvPickup].Value);
			_player.SetPickupLV(_lvPickup);
			UpdateNextPickupCost();

			_pickup.UpdateItem(GetPickupDescriptaion(), GetNextPickupCost());
			_player.Save();
		}

		private void UpdateNextSpeedCost()
		{
			if (IsSpeedMax() == false)
			{
				_speedNextCost = _speedData.UpgradList[_lvSpeed + 1].NextUpgradCost;
			}
		}

		private void UpdateNextPickupCost()
		{
			if (IsPickUpMax() == false)
			{
			//	_pickUpNextCost = _pickupData._upgradList[_lvPickup + 1].NextUpgradCost;
			}
		}

		private bool IsSpeedMax()
		{
			return _lvSpeed == _speedData.UpgradList.Count -1;
		}
		
		private bool IsPickUpMax()
		{
			return false;
			//return _lvPickup == _pickupData.UpgradList.Count - 1;
		}


		#endregion

		#region Protected Method
		protected override void StartInit()
		{
			_wallet = _player.Wallet;
			_lvPickup = _player.PickupLv;
			_lvSpeed = _player.SpeedLv;

			//_player.SetPickupCapcity(_pickupData._upgradList[_lvPickup].Value);
			_player.SetSpeed(_speedData.UpgradList[_lvSpeed].Value);
			UpdateNextPickupCost();
			UpdateNextSpeedCost();

			_speed.SetUp(_speedIcon, "Speed", GetSpeedDescriptaion(), GetNextSpeedCost(), TrySpeedUpgrad);
			_pickup.SetUp(_pickupIcon, "Pickup", GetPickupDescriptaion(), GetNextPickupCost(), TryPickupUpgrad);
		}
		#endregion

	}
}

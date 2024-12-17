using EverythingStore.Actor.Player;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace EverythingStore.Upgrad
{
    public class UpgradPlayer : BottomUpPopup
    {
		#region Field
		private Player _player;

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

		[Title("Money")]
		[SerializeField] private TMP_Text _money;
 
		private Wallet _wallet;
		#endregion

		#region Property
		#endregion

		#region Event

		#endregion

		#region UnityCycle
		#endregion

		#region Public Method
		public void Initialize(Player player,int speedLv, int pickupLv)
		{
			_player = player;
			_lvSpeed = speedLv;
			_lvPickup = pickupLv;

			float speed = GetSpeed(speedLv);
			int pickupCapacity = GetPickupCapacity(pickupLv);

			_player.SetSpeed(speed);
			_player.SetPickupCapcity(pickupCapacity);

			UpdateNextPickupCost();
			UpdateNextSpeedCost();

			_wallet = _player.Wallet;

			//UI 초기화 진행
			_speed.SetUp(_speedIcon, "Speed", GetSpeedDescriptaion(), GetNextSpeedCost(), TrySpeedUpgrad);
			_pickup.SetUp(_pickupIcon, "Pickup", GetPickupDescriptaion(), GetNextPickupCost(), TryPickupUpgrad);
			_money.text = _wallet.GetFormatSuffix();
			_wallet.OnUpdateString += UpdateMoney;
		}


		#endregion

		#region Private Method
		private void UpdateMoney(string money)
		{
			_money.text = money;
		}
		private int GetPickupCapacity(int pickupLv)
		{
			return _pickupData.GetUpgradData(pickupLv).Value;
		}

		private float GetSpeed(int speedLv)
		{
			return _speedData.GetUpgradData(speedLv).Value;
		}

		private string GetSpeedDescriptaion()
		{
			return $"Speed : {_speedData.GetUpgradData(_lvSpeed).Value}";
		}	
		
		private string GetPickupDescriptaion()
		{
			return $"Max Pickup : {_pickupData.GetUpgradData(_lvPickup).Value}";
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
			_player.SetSpeed(_speedData.GetUpgradData(_lvSpeed).Value);
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
			_player.SetPickupCapcity(_pickupData.GetUpgradData(_lvPickup).Value);
			_player.SetPickupLV(_lvPickup);
			UpdateNextPickupCost();

			_pickup.UpdateItem(GetPickupDescriptaion(), GetNextPickupCost());
			_player.Save();
		}

		private void UpdateNextSpeedCost()
		{
			if (IsSpeedMax() == false)
			{
				_speedNextCost = _speedData.GetUpgradData(_lvSpeed + 1).NextUpgradCost;
			}
			else
			{
				_speedNextCost = _speedData.GetUpgradData(_lvSpeed).NextUpgradCost;
			}
		}

		private void UpdateNextPickupCost()
		{
			if (IsPickUpMax() == false)
			{
				_pickUpNextCost = _pickupData.GetUpgradData(_lvPickup + 1).NextUpgradCost;
			}
			else
			{
				_pickUpNextCost = _pickupData.GetUpgradData(_lvPickup).NextUpgradCost;
			}
		}

		private bool IsSpeedMax()
		{
			return _lvSpeed == _speedData.GetMaxLv();
		}
		
		private bool IsPickUpMax()
		{
			return _lvPickup == _pickupData.GetMaxLv();
		}


		#endregion

		#region Protected Method
		protected override void Start_Intilaize()
		{
			base.Start_Intilaize();
		}
		#endregion

	}
}

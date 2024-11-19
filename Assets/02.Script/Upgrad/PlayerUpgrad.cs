using EverythingStore.Actor.Player;
using EverythingStore.InteractionObject;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EverythingStore.Upgrad
{
	public class PlayerUpgrad : MonoBehaviour
	{
		#region Field
		[Title("Target")]
		[SerializeField] private Player _player;

		[Title("Speed")]
		[SerializeField] private UpgradArea _speedUpgradArea;
		[SerializeField] private UpgradData _speed;

		[Title("Pickup")]
		[SerializeField] private UpgradArea _pickupUpgradArea;
		[SerializeField] private UpgradData _pickup;

		private int _speedLv;
		private int _pickupLv;
		#endregion

		#region Property
		#endregion

		#region UnityCycle
		private void Start()
		{
			_speedUpgradArea.SetupTarget(_speed.Name, _speedLv, _speed.UpgradList[_speedLv].Cost, UpgradSpeed);
			_pickupUpgradArea.SetupTarget(_pickup.Name, _pickupLv, _pickup.UpgradList[_pickupLv].Cost, UpgradPickupCount);
		}
		#endregion

		#region Private Method
		private void UpgradSpeed()
		{
			_speedLv++;

			if (_speedLv == _speed.UpgradList.Count)
			{
				_speedUpgradArea.Max();
			}
			else
			{
				_player.SetSpeed(_speed.UpgradList[_speedLv].Value);
				_speedUpgradArea.SetupTarget(_speed.Name, _speedLv, _speed.UpgradList[_speedLv].Cost, UpgradSpeed);
			}
		}

		private void UpgradPickupCount()
		{
			_pickupLv++;

			if (_pickupLv == _pickup.UpgradList.Count)
			{
				_pickupUpgradArea.Max();
			}
			else
			{
				_player.SetPickupCapcity((int)_pickup.UpgradList[_pickupLv].Value);
				_pickupUpgradArea.SetupTarget(_pickup.Name, _pickupLv, _pickup.UpgradList[_pickupLv].Cost, UpgradPickupCount);
			}
		}
		#endregion
	}
}

using Codice.CM.Common;
using EverythingStore.InteractionObject;
using EverythingStore.Save;
using EverythingStore.Upgrad;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.Port;

namespace EverythingStore.Actor.Player
{
    public class Player : MonoBehaviour, ISave
	{
		#region Field
		[SerializeField] private Transform _getItemPoint;
		[SerializeField] private UpgradPlayer _upgrad;
		private PlayerCharacterMovement _movement;
		private PickupAndDrop _pickupAndDrop;
		private Wallet _wallet;
		private PlayerData _savePlayerData;

		private int _speedLv;
		private int _pickupLv;

		#endregion

		#region Property
		public Transform GetItemPoint => _getItemPoint;
		public PickupAndDrop PickupAndDrop => _pickupAndDrop;
		public Wallet Wallet => _wallet;

		public int SpeedLv => _speedLv;
		public int PickupLv=> _pickupLv;

		public string SaveFileName => "PlayerData";
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Awake()
		{
			_pickupAndDrop = GetComponent<PickupAndDrop>();
			_movement = GetComponent<PlayerCharacterMovement>();
			_wallet = new();

			InitSaveData();
			SaveManager.Instance.RegisterSave(this);
		}

		private void Start()
		{
			SetupSaveData();
		}

		/// <summary>
		/// Save Data에 맞추어서 설정합니다.
		/// </summary>
		private void SetupSaveData()
		{
			transform.position = new Vector3(_savePlayerData.worldPos_X, _savePlayerData.worldPos_Y, _savePlayerData.worldPos_Z);
			_upgrad.Initialize(this, _speedLv, _pickupLv);
			_wallet.SetMoney(_savePlayerData.money);
			if (_savePlayerData.PickupObjects.Length > 0)
			{
				_pickupAndDrop.LoadPickupObject(_savePlayerData.PickupObjects);
			}
		}
		#endregion

		#region Public Method

		public void SetSpeedLV(int lv)
		{
			_speedLv = lv;
		}

		public void SetPickupLV(int lv)
		{
			_pickupLv = lv;
		}

		public void SetSpeed(float speed)
		{
			Debug.Log($"[Player] Speed : {speed}");
			_movement.Speed = speed;
		}

		public void SetPickupCapcity(int capacity)
		{
			Debug.Log($"[Player] PickupCapcity : {capacity}");
			_pickupAndDrop.SetMaxPickup(capacity);
		}

		public void InitSaveData()
		{
			if (SaveSystem.HasSaveData(SaveFileName) == false)
			{
				_savePlayerData = new(transform.position.x, transform.position.y, transform.position.z);
				Save();
			}
			else
			{
				_savePlayerData = SaveSystem.LoadData<PlayerData>(SaveFileName);
				Debug.Log("[Save] Load Player Data");
			}

		_speedLv = _savePlayerData.speedLv;
		_pickupLv  = _savePlayerData.pickupLv;

		}

		[Button("Debug Save")]
		public async void Save()
		{
			_savePlayerData.speedLv = _speedLv;
			_savePlayerData.pickupLv = _pickupLv;
			_savePlayerData.money = _wallet.Money;
			_savePlayerData.worldPos_X = transform.position.x;
			_savePlayerData.worldPos_Y = transform.position.y;
			_savePlayerData.worldPos_Z = transform.position.z;
			_savePlayerData.PickupObjects = _pickupAndDrop.GetPickupObjects();

			 await SaveSystem.SaveData<PlayerData>(_savePlayerData, SaveFileName);
		}
		#endregion

		#region Private Method
		#endregion

		#region Protected Method
		#endregion

	}
}

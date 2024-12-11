using EverythingStore.InteractionObject;
using EverythingStore.Save;
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
		private PlayerCharacterMovement _movement;
		private PickupAndDrop _pickupAndDrop;
		private Wallet _wallet;

		private PlayerData _savePlayerData;
		#endregion

		#region Property
		public Transform GetItemPoint => _getItemPoint;
		public PickupAndDrop PickupAndDrop => _pickupAndDrop;
		public Wallet Wallet => _wallet;

		public int SpeedLv => _savePlayerData.speedLv;
		public int PickupLv=> _savePlayerData.pickupLv;

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

			if(SaveSystem.HasSaveData(SaveFileName) == false)
			{
				InitSaveData();
				Save();
			}
			else
			{
				_savePlayerData = SaveSystem.LoadData<PlayerData>(SaveFileName);
				Debug.Log("[Save] Load Player Data");
			}

			Debug.Log(Application.persistentDataPath);

			SaveManager.Instance.RegisterSave(this);
		}

		private void Start()
		{
			_wallet.SetMoney(_savePlayerData.money);
		}
		#endregion

		#region Public Method

		public void SetSpeedLV(int lv)
		{
			_savePlayerData.speedLv = lv;
			Save();
		}

		public void SetPickupLV(int lv)
		{
			_savePlayerData.pickupLv = lv;
			Save();
		}

		public void SetSpeed(float speed)
		{
			_movement.Speed = speed;
		}

		public void SetPickupCapcity(int capacity)
		{
			_pickupAndDrop.SetMaxPickup(capacity);
		}

		public void InitSaveData()
		{
			_savePlayerData = new PlayerData();
		}

		public async void Save()
		{
			 await SaveSystem.SaveData<PlayerData>(_savePlayerData, SaveFileName);
		}
		#endregion

		#region Private Method
		#endregion

		#region Protected Method
		#endregion

	}
}

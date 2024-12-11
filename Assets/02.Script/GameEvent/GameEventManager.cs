using EverythingStore.InteractionObject;
using EverythingStore.Save;
using System;
using System.Collections.Generic;

namespace EverythingStore.GameEvent
{
	public enum GameTargetType
	{
		None =								0,
		Tutorial_GameStart =			1 << 0,
		Tutorial_Pickup =				1 << 1,
		Tutorial_Counter =				1 << 2,
		Tutorial_Money =				1 << 3,
		Tutorial_BoxOrder =			1 << 4,
		Tutorial_EnterBoxOrder =	1 << 5,
		Tutorial_Delivery =				1 << 6,
		Product_UnlockAuction =	1 << 7,
		Auction =							1 << 8,
		EndTarget =						1 << 9,
	}

	public class GameEventManager : Singleton<GameEventManager>, ISave
	{
		#region Field
		private Dictionary<GameTargetType, GameEventBase> _gameEventTable = new();

		private GameTarget _targetData;

		public string SaveFileName => "GameTarget";

		private GameTargetType _targetDataUpdateMask = GameTargetType.Tutorial_BoxOrder | GameTargetType.Auction | GameTargetType.EndTarget;
		#endregion

		#region Property
		public GameTargetType GameTarget => _targetData.Type;
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		protected override void AwakeInit()
		{
			GameEventBase[] events = transform.GetComponentsInChildren<GameEventBase>();
			foreach (var item in events)
			{
				_gameEventTable.Add(item.Type, item);
			}

			if (SaveSystem.HasSaveData(SaveFileName) == false)
			{
				InitSaveData();
				Save();
			}
			else
			{
				_targetData = SaveSystem.LoadData<GameTarget>(SaveFileName);
			}
		}


		private void Start()
		{
			OnEvent(_targetData.Type);
		}
		#endregion

		#region Public Method

		public void OnEvent(GameTargetType type)
		{
			if (type == GameTargetType.None)
			{
				return;
			}

			_gameEventTable[type].OnEvent();

			CheckSave(type);
		}

		private void CheckSave(GameTargetType type)
		{
			_targetData.Type = type;

			if ((type & _targetDataUpdateMask) > 0)
			{
				Save();
			}
		}

		public void InitSaveData()
		{
			_targetData = new GameTarget();
		}

		public async void Save()
		{
			await SaveSystem.SaveData<GameTarget>(_targetData, SaveFileName);
		}

		#endregion

		#region Private Method
		#endregion

		#region Protected Method

		#endregion

	}
}

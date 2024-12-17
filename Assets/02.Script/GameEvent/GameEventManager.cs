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
		Tutorial_End =					1 << 8,
		Auction =							1 << 9,
		EndTarget =						1 << 10,
	}

	public class GameEventManager : Singleton<GameEventManager>, ISave
	{
		#region Field
		private Dictionary<GameTargetType, GameEventBase> _gameEventTable = new();
		private GameTargetData _targetData;
		private GameTargetType _targetDataUpdateMask = GameTargetType.Tutorial_BoxOrder  | GameTargetType.Auction;
		private GameTargetType _currentType;
		#endregion

		#region Property
		public string SaveFileName => "GameTarget";
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

			InitSaveData();
		}

		private void Start()
		{
			OnEvent(_currentType);
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

		public void OnEventCallback(GameTargetType type, Action callback)
		{
			_gameEventTable[type].OnEvented += callback;
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
			if (SaveSystem.HasSaveData(SaveFileName) == false)
			{
				_targetData = new GameTargetData();
				Save();
			}
			else
			{
				_targetData = SaveSystem.LoadData<GameTargetData>(SaveFileName);
			}

			_currentType = _targetData.Type;
		}

		public async void Save()
		{
			await SaveSystem.SaveData<GameTargetData>(_targetData, SaveFileName);
		}
		#endregion

	}
}

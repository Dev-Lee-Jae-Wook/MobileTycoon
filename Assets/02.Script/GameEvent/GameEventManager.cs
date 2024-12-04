using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.GameEvent
{
	public enum GameEventType
	{
		Tutorial_GameStart,
		Totorial_Pickup,
		Totorial_Counter,
		Totorial_Money,
		Totorial_BoxOrder,
		First_BoxOrder,
		Tutorial_EnterBoxOrder,
		Tutorial_Delivery,
		UnlockableAuction
	}

	public class GameEventManager : Singleton<GameEventManager>
	{
		#region Field
		private Dictionary<GameEventType, GameEventBase> _gameEventTable = new();
		#endregion

		#region Property
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		#endregion

		#region Public Method
		public void OnEvent(GameEventType type)
		{
			_gameEventTable[type].OnEvent();
		}

		#endregion

		#region Private Method
		#endregion

		#region Protected Method
		protected override void AwakeInit()
		{
			GameEventBase[] events = transform.GetComponentsInChildren<GameEventBase>();
			foreach (var item in events)
			{
				_gameEventTable.Add(item.Type, item);
			}
		}
		#endregion

	}
}

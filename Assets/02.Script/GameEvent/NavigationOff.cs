using EverythingStore.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.GameEvent
{
	public class NavigationOff : GameEventBase
	{
		public override GameEventType Type => _eventType;

		[SerializeField] private NavigationUI _navigation;
		[SerializeField] private GameEventType _eventType;

		public override void OnEvent()
		{
			_navigation.OffNavigation();
		}
	}
}

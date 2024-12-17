using EverythingStore.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.GameEvent
{
	public class NavigationOff : GameEventBase
	{
		public override GameTargetType Type => _eventType;

		[SerializeField] private Navigation _navigation;
		[SerializeField] private GameTargetType _eventType;

		public override void OnEvent()
		{
			_navigation.OffNavigation();
			base.OnEvent();
		}
	}
}

using EverythingStore.UI;
using UnityEngine;

namespace EverythingStore.GameEvent
{
	public class Totorial_SimpleNavigation : GameEventBase
	{
		#region Field
		[SerializeField] private Navigation _navigation;
		[SerializeField] private GameTargetType _type;
		#endregion

		#region Property
		public override GameTargetType Type => _type;
		#endregion

		#region Public Method
		public override void OnEvent()
		{
			_navigation.SetTarget(transform);
			base.OnEvent();
		}
		#endregion
	}
}
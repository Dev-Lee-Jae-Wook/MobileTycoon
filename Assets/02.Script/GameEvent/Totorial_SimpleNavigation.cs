using EverythingStore.UI;
using UnityEngine;

namespace EverythingStore.GameEvent
{
	public class Totorial_SimpleNavigation : GameEventBase
	{
		#region Field
		[SerializeField] private NavigationUI _navigation;
		[SerializeField] private GameEventType _type;
		#endregion

		#region Property
		public override GameEventType Type => _type;
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		#endregion

		#region Public Method
		public override void OnEvent()
		{
			_navigation.SetTarget(transform);
		}
		#endregion

		#region Private Method
		#endregion

		#region Protected Method
		#endregion
	}
}
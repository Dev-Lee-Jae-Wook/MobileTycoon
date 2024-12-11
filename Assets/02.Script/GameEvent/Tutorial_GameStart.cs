using EverythingStore.InteractionObject;
using EverythingStore.UI;
using UnityEngine;

namespace EverythingStore.GameEvent
{
	public class Tutorial_GameStart : GameEventBase
	{
		#region Field
		[SerializeField] private Navigation _navigation;
		[SerializeField] private BoxStorage _boxStorage;
		#endregion

		#region Property
		public override GameTargetType Type => GameTargetType.Tutorial_GameStart;
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		#endregion

		#region Public Method
		public override void OnEvent()
		{
			_navigation.SetTarget(transform);
			_boxStorage.TutorialSpawnBox();
		}
		#endregion

		#region Private Method
		#endregion

		#region Protected Method
		#endregion
	}
}
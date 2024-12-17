using EverythingStore.Save;
using EverythingStore.UI;
using UnityEngine;

namespace EverythingStore.GameEvent
{
	public class Totorial_End : GameEventBase
	{

		[SerializeField] private GameTargetType _nextType;

		#region Property
		public override GameTargetType Type => GameTargetType.Tutorial_End;
		#endregion

		#region Public Method
		public override void OnEvent()
		{
			SaveManager.Instance.StartAutoSave();
			GameEventManager.Instance.OnEvent(_nextType);
			base.OnEvent();
		}
		#endregion
	}
}
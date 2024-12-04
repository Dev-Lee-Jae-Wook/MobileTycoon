using EverythingStore.Actor.Player;
using EverythingStore.InteractionObject;
using EverythingStore.UI;
using UnityEngine;
using UnityEngine.Playables;

namespace EverythingStore.GameEvent
{
	public class Tutorial_DeilveryTruck : GameEventBase
	{
		#region Field
		[SerializeField] private PlayerInput _input;
		[SerializeField] private PlayableDirector _product;
		#endregion

		#region Property
		public override GameEventType Type => GameEventType.Tutorial_Delivery;
		#endregion



		#region Public Method
		public override void OnEvent()
		{
			_input.SetProductFixControl(true);
			_product.Play();
			_product.stopped += (tmp) =>
			{
				_input.SetProductFixControl(false);
				GameEventManager.Instance.OnEvent(GameEventType.UnlockableAuction);
			};
		}
		#endregion


	}
}
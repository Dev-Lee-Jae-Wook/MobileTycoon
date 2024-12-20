using EverythingStore.Actor.Player;
using UnityEngine;
using UnityEngine.Playables;

namespace EverythingStore.GameEvent
{
	public class UnlockableAuction : GameEventBase
	{
		#region Field
		[SerializeField] private PlayerInput _input;
		[SerializeField] private PlayableDirector _product;
		[SerializeField] private GameTargetType _nextType;
		#endregion

		#region Property
		public override GameTargetType Type => GameTargetType.Product_UnlockAuction;
		#endregion

		#region Public Method
		public override void OnEvent()
		{
			_input.SetProductFixControl(true);
			_product.Play();
			_product.stopped += (tmp) =>
			{
				_input.SetProductFixControl(false);
				GameEventManager.Instance.OnEvent(_nextType);
			};
			base.OnEvent();
		}
		#endregion
	}
}
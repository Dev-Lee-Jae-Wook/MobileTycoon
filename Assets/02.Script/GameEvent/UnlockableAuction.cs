using EverythingStore.Actor.Player;
using EverythingStore.InputMoney;
using EverythingStore.InteractionObject;
using EverythingStore.UI;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace EverythingStore.GameEvent
{
	public class UnlockableAuction : GameEventBase
	{
		#region Field
		[SerializeField] private PlayerInput _input;
		[SerializeField] private PlayableDirector _product;
		[SerializeField] private NavigationUI _navigation;
		#endregion

		#region Property
		public override GameEventType Type => GameEventType.UnlockableAuction;
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		#endregion

		#region Public Method
		public override void OnEvent()
		{
			_input.SetProductFixControl(true);
			_product.Play();
			_product.stopped += (tmp) =>
			{
				_navigation.SetTarget(transform);
				_input.SetProductFixControl(false);
			};
		}
		#endregion

		#region Private Method
		#endregion

		#region Protected Method
		#endregion
	}
}
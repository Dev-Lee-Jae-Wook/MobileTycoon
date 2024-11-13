using EverythingStore.Actor;
using EverythingStore.Actor.Customer;
using EverythingStore.Actor.Player;
using UnityEngine;

namespace EverythingStore.RayInteraction
{
	[RequireComponent(typeof(PickupAndDrop))]
	public class CustomerRayInteraction : ActorRayInteractionBase
	{
		#region Field
		private PickupAndDrop _pickupAndDrop;
		#endregion


		#region UnityCycle
		private void Awake()
		{
			_pickupAndDrop = GetComponent<PickupAndDrop>();
		}
		#endregion

		#region Pubilc Method
		/// <summary>
		/// 바라보는 방향으로 RayCast을 시도하고 대상이 있다면 상호작용합니다.
		/// </summary>
		public void RayCastAndInteraction()
		{
			if(RayCasetHit(out var hit) == true)
			{
				if(hit.collider.TryGetComponent<ICustomerInteraction>(out var interaction) == true)
				{
					Intreaction(interaction);
				}
			}
		}
		#endregion

		#region Private Method
		/// <summary>
		/// 상호작용 오브젝트와 상호작용합니다.
		/// </summary>
		private void Intreaction(ICustomerInteraction interaction)
		{
			interaction.InteractionCustomer(_pickupAndDrop);
		}

		#endregion
	}
}
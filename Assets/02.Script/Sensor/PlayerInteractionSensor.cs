using EverythingStore.Actor;
using EverythingStore.Actor.Player;
using UnityEngine;

namespace EverythingStore.Sensor
{
	[RequireComponent(typeof(PickupAndDrop))]
	public class PlayerInteractionSensor : InteractionSensorBase
    {
		#region Field
		private PickupAndDrop _pickupAndDrop;
		#endregion


		#region UnityCycle
		private void Awake()
		{
			_pickupAndDrop = GetComponent<PickupAndDrop>();
		}

		private void Update()
		{
			if(RayCasetHit(out var hit))
			{
				if(hit.collider.TryGetComponent<IPlayerInteraction>(out var interaction))
				{
					Intreaction(interaction);
				}
			}
		}
		#endregion

		#region Method
		/// <summary>
		/// ��ȣ�ۿ� ������Ʈ�� ��ȣ�ۿ��մϴ�.
		/// </summary>
		private void Intreaction(IPlayerInteraction interaction)
		{
			interaction.InteractionPlayer(_pickupAndDrop);
		}

		#endregion
	}
}


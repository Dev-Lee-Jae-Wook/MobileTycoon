using EverythingStore.Actor;
using EverythingStore.Actor.Player;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace EverythingStore.Sensor
{
	[RequireComponent(typeof(PickupAndDrop))]
	public class PlayerInteractionSensor : InteractionSensorBase
    {
		#region Field
		private Player _owner;
		private PickupAndDrop _pickupAndDrop;


		#endregion
		#region UnityCycle
		private void Awake()
		{
			_owner = GetComponent<Player>();
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
		/// 상호작용 오브젝트와 상호작용합니다.
		/// </summary>
		private void Intreaction(IPlayerInteraction interaction)
		{
			interaction.InteractionPlayer(_owner);
		}

		#endregion
	}
}


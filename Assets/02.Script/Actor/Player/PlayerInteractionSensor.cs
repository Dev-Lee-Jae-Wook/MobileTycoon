using EverythingStore.Actor.Customer;
using UnityEngine;

namespace EverythingStore.Actor.Player
{
    public class PlayerInteractionSensor : MonoBehaviour
    {
		#region Debug
		private bool _isPlayer = true;
		private void DebugInteractionCustomer(ICustomerInteraction interaction)
		{
			interaction.InteractionCustomer(_hand);
		}
		#endregion

		#region Field
		[SerializeField] private LayerMask _interactionLayerMask;
		[SerializeField] private Vector3 _sensorPivot;
		[SerializeField] private PickupAndDrop _hand;
		#endregion

		#region UnityCycle
		private void Update()
		{
			if(Input.GetKeyDown(KeyCode.F))
			{
				_isPlayer = !_isPlayer;
			}

			var hits = Physics.RaycastAll(transform.position + _sensorPivot, transform.forward, 1.0f, _interactionLayerMask);
			if (hits.Length > 0)
			{
				Debug.Log(hits[0].collider.name);

				if (_isPlayer == true)
					Intreaction(hits[0].collider.GetComponent<IPlayerInteraction>());
				else
					DebugInteractionCustomer(hits[0].collider.GetComponent<ICustomerInteraction>());
			}
		}
		#endregion

		#region Method
		/// <summary>
		/// ��ȣ�ۿ� ������Ʈ�� ��ȣ�ۿ��մϴ�.
		/// </summary>
		private void Intreaction(IPlayerInteraction interaction)
		{
			interaction.InteractionPlayer(_hand);
		}

		#endregion
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EverythingStore.Actor
{
    public class PlayerInteractionSensor : MonoBehaviour
    {
		#region Field
		[SerializeField] private LayerMask _interactionLayerMask;
		[SerializeField] private Vector3 _sensorPivot;
		[SerializeField] private Hand _hand;
		#endregion

		#region UnityCycle
		private void Update()
		{
			var hits = Physics.RaycastAll(transform.position + _sensorPivot, transform.forward, 1.0f, _interactionLayerMask);
			if (hits.Length > 0)
			{
				Intreaction(hits[0].collider.GetComponent<IPlayerInteraction>());
			}
		}
		#endregion

		#region Method
		/// <summary>
		/// 상호작용 오브젝트와 상호작용합니다.
		/// </summary>
		private void Intreaction(IPlayerInteraction interaction)
		{
			interaction.InteractionPlayer(_hand);
		}
		#endregion
	}
}


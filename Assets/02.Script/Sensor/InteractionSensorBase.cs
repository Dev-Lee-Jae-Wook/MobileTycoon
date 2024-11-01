using UnityEngine;

namespace EverythingStore.Sensor
{
	public abstract class InteractionSensorBase : MonoBehaviour
	{
		#region Field
		[SerializeField] private LayerMask _interactionLayerMask;
		[SerializeField] private Vector3 _sensorPivot;
		[SerializeField] private float _distance = 1.0f;
		[SerializeField] private Color _rayColor;
		#endregion


		#region UnityMethod
		private void OnDrawGizmos()
		{
			Gizmos.color = _rayColor;
			Vector3 p1 = transform.position + _sensorPivot;
			Vector3 p2 = p1 + transform.forward * _distance;
			Gizmos.DrawLine(p1, p2);
		}
		#endregion

		#region  Protected Method
		protected bool RayCasetHit(out RaycastHit hit)
		{
			Vector3 origin = transform.position + _sensorPivot;
			Vector3 dir = transform.forward;
			float distance = 1.0f;

			return Physics.Raycast(origin, dir, out hit, _distance, _interactionLayerMask);
		}
		#endregion
	}
}


using EverythingStore.Animation;
using EverythingStore.InteractionObject;
using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace EverythingStore.Actor
{
	public class PickupAndDrop : MonoBehaviour, IAnimationEventPickupAndDrop
	{
		#region Field
		[SerializeField] private Transform _pickupPoint;
		[SerializeField] private Rig _rig;
		private PickableObject _pickableObject;
		#endregion

		#region Property
		/// <summary>
		/// 현재 픽업한 아이템의 갯수
		/// </summary>
		public int pickUpObjectCount { get; private set; }
		#endregion

		#region Event
		public event Action OnAnimationPickup;
		public event Action OnAnimationDrop;
		#endregion

		#region UnityCycle
		private void Start()
		{
			SetRigWeight(0.0f);
			OnAnimationPickup += () => SetRigWeight(1.0f);
			OnAnimationDrop += () => SetRigWeight(0.0f);
		}
		#endregion

		#region Public Method
		/// <summary>
		/// 현재 픽업이 가능한지
		/// </summary>
		public bool CanPickUp()
		{
			return true;
		}

		/// <summary>
		/// 아이템을 픽업한다.
		/// </summary>
		public void PickUp(PickableObject pickableObject)
		{
			pickableObject.transform.parent = _pickupPoint;
			pickableObject.transform.localPosition = Vector3.zero;
			pickableObject.transform.localRotation = Quaternion.identity;
			_pickableObject = pickableObject;
			pickUpObjectCount++;
			OnAnimationPickup?.Invoke();
		}

		/// <summary>
		/// 가장 위에 있는 아이템을 드랍한다.
		/// </summary>
		public PickableObject Pop()
		{
			PickableObject popObject = _pickableObject;
			_pickableObject = null;
			pickUpObjectCount--;
			OnAnimationDrop?.Invoke();
			return popObject;
		}

		public bool IsPickUpObject()
		{
			return _pickableObject != null;
		}

		/// <summary>
		/// 가장 위에 있는 픽업 아이템을 반환합니다.
		/// </summary>
		public PickableObject PeekObject()
		{
			return _pickableObject;
		}
		#endregion

		#region Private Method
		private void SetRigWeight(float weight)
		{
			_rig.weight = weight;
		}
		#endregion

	}
}
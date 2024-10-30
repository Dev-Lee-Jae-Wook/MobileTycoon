using EverythingStore.Animation;
using EverythingStore.InteractionObject;
using EverythingStore.ProjectileMotion;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace EverythingStore.Actor
{
	[RequireComponent(typeof(BezierCurve))]
	public class PickupAndDrop : MonoBehaviour, IAnimationEventPickupAndDrop
	{
		#region Field
		/// <summary>
		/// 최대로 들 수 있는 아이템 갯수
		/// </summary>
		[SerializeField] private int _capacity;
		[SerializeField] private Transform _pickupPoint;
		[SerializeField] private float _coolTime;
		[ReadOnly][SerializeField] private float _currentCoolTime;

		private Rig _rig;
		private BezierCurve _bezierCurve;

		private Stack<PickableObject> _pickObjectStack;
		private float _nextHeight;
		#endregion

		#region Property
		/// <summary>
		/// 현재 픽업한 아이템의 갯수
		/// </summary>
		public int pickUpObjectCount => _pickObjectStack.Count;
		#endregion

		#region Event
		public event Action OnAnimationPickup;
		public event Action OnAnimationDrop;
		#endregion

		#region UnityCycle
		private void Awake()
		{
			_bezierCurve = GetComponent<BezierCurve>();
			_rig = transform.GetComponentInChildren<Rig>();
			_pickObjectStack = new Stack<PickableObject>();
			SetRigWeight(0.0f);
			OnAnimationPickup += () => SetRigWeight(1.0f);
			OnAnimationDrop += () => SetRigWeight(0.0f);
			_nextHeight = 0.0f;
			_currentCoolTime = 0.0f;
		}

		private void Update()
		{
			UpdateCoolTime();
		}

		#endregion

		#region Public Method
		/// <summary>
		/// 현재 픽업이 가능한지
		/// </summary>
		public bool CanPickup()
		{
			if(IsCoolTime() == true)
			{
				return false;
			}

			return pickUpObjectCount < _capacity;
		}

		public bool CanPopup()
		{
			if (IsCoolTime() == true)
			{
				return false;
			}

			return pickUpObjectCount > 0;
		}

		/// <summary>
		/// 픽업 연출 이후 최종적으로 픽업이 진행됩니다.
		/// </summary>
		public void PickUp(PickableObject pickableObject)
		{
			_bezierCurve.Movement(pickableObject.transform, _pickupPoint, _pickupPoint.position.y + 1.0f, GetPickupLocalPosition(),
				()=> {
					OnAnimationPickup?.Invoke();
				});

			_nextHeight += pickableObject.Height;
			_pickObjectStack.Push(pickableObject);
			StartCoolTime();
		}


		/// <summary>
		/// 가장 위에 있는 아이템을 드랍한다.
		/// </summary>
		public PickableObject Pop(Transform endTarget, Vector3 localPos,Action callback = null)
		{
			PickableObject popObject = _pickObjectStack.Pop();

			_bezierCurve.Movement(popObject.transform, endTarget, endTarget.position.y + 1.0f, localPos,
				callback);

			if (pickUpObjectCount == 0)
			{
				OnAnimationDrop?.Invoke();
			}

			_nextHeight = Mathf.Clamp(_nextHeight - popObject.Height, 0.0f, float.MaxValue);
			StartCoolTime();

			return popObject;
		}

		public bool IsPickUpObject()
		{
			return _pickObjectStack.Count > 0;
		}

		/// <summary>
		/// 가장 위에 있는 픽업 아이템을 반환합니다.
		/// </summary>
		public PickableObject PeekObject()
		{
			return _pickObjectStack.Peek();
		}
		#endregion

		#region Private Method
		private void SetRigWeight(float weight)
		{
			_rig.weight = weight;
		}

		/// <summary>
		/// 픽업하였을 때의 로컬 포지션을 반환합니다.
		/// </summary>
		/// <returns></returns>
		private Vector3 GetPickupLocalPosition()
		{
			return Vector3.up * _nextHeight;
		}
		private void UpdateCoolTime()
		{
			_currentCoolTime -= Time.deltaTime;
		}

		/// <summary>
		/// 쿨타임이 0이상이면 쿨타임이라고 판정
		/// </summary>
		private bool IsCoolTime()
		{
			return _currentCoolTime > 0.0f;
		}

		/// <summary>
		/// 쿨타임 시작
		/// </summary>
		private void StartCoolTime()
		{
			_currentCoolTime = _coolTime;
		}
		#endregion

	}
}
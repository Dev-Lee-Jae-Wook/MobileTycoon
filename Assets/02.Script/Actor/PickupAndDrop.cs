using EverythingStore.Animation;
using EverythingStore.InteractionObject;
using EverythingStore.Optimization;
using EverythingStore.ProjectileMotion;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using static EverythingStore.InteractionObject.PickableObject;

namespace EverythingStore.Actor
{
	[RequireComponent(typeof(BezierCurve))]
	public class PickupAndDrop : MonoBehaviour, IAnimationEventPickupAndDrop
	{
		#region Field
		/// <summary>
		/// 최대로 들 수 있는 아이템 갯수
		/// </summary>
		[SerializeField] private Transform _pickupPoint;
		[SerializeField] private float _coolTime;
		[ReadOnly][SerializeField] private float _currentCoolTime;

		private Rig _rig;
		private BezierCurve _bezierCurve;

		private Stack<PickableObject> _pickObjectStack;
		private float _nextHeight;

		private PickableObjectType _pickupObjectsType = PickableObjectType.None;
		#endregion

		#region Property
		/// <summary>
		/// 현재 픽업한 아이템의 갯수
		/// </summary>
		public int pickUpObjectCount => _pickObjectStack.Count;
		[ReadOnly] public int maxPickup;
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
		/// 가능한 조건 1. 픽업한 오브젝트가 동일해야된다 2.쿨타임이 아니여야된다 3.현재 픽업 수가 최대 개수보다 작아야된다.
		/// </summary>
		public bool CanPickup(PickableObjectType type)
		{
			//현재 픽업한 오브젝트의 타입과 다른 타입이라면 픽업할 수 없습니다.
			if (_pickupObjectsType != PickableObjectType.None && _pickupObjectsType != type)
			{
				return false;
			}

			if (IsCoolTime() == true)
			{
				return false;
			}

			return pickUpObjectCount < maxPickup;
		}

		public bool CanDrop()
		{
			if (IsCoolTime() == true)
			{
				return false;
			}

			return pickUpObjectCount > 0;
		}

		public void Clear()
		{
			while (_pickObjectStack.Count > 0)
			{
				var popObject = _pickObjectStack.Pop();
				popObject.GetComponent<PooledObject>().Release();
			}
			_pickupObjectsType = PickableObjectType.None;
			OnAnimationDrop?.Invoke();
		}


		/// <summary>
		/// 연출 없이 아이템을 드랍합니다.
		/// </summary>
		/// <returns></returns>
		private PickableObject Pop()
		{
			PickableObject popObject = _pickObjectStack.Pop();
			_nextHeight = Mathf.Clamp(_nextHeight - popObject.Height, 0.0f, float.MaxValue);

			if(_pickObjectStack.Count == 0)
			{
				_pickupObjectsType = PickableObjectType.None;
			}

			return popObject;
		}

		/// <summary>
		/// 픽업 연출 이후 최종적으로 픽업이 진행됩니다.
		/// </summary>
		public void Pickup(PickableObject pickableObject)
		{
			_bezierCurve.Movement(pickableObject.transform, _pickupPoint, _pickupPoint.position.y + 1.0f, GetPickupLocalPosition(),
				() =>
				{
					OnAnimationPickup?.Invoke();
				});
			Push(pickableObject);
			StartCoolTime();
		}

		/// <summary>
		/// 가장 위에 있는 아이템을 포물선 움직임 연출을 하면서 드랍합니다.
		/// </summary>
		public PickableObject Drop(Transform endTarget, Vector3 localPos,Action callback = null)
		{
			PickableObject popObject = Pop();

			if (pickUpObjectCount == 0)
			{
				OnAnimationDrop?.Invoke();
			}
			_bezierCurve.Movement(popObject.transform, endTarget, endTarget.position.y + 1.0f, localPos,
				callback);
			StartCoolTime();

			return popObject;
		}

		/// <summary>
		///  아이템을 한개라도 가지고 있는가?
		/// </summary>
		public bool HasPickupObject()
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
		/// <summary>
		/// 픽업 스택에 추가하고 다음 높이를 계산합니다.
		/// </summary>
		private void Push(PickableObject pickableObject)
		{
			_pickObjectStack.Push(pickableObject);
			_nextHeight += pickableObject.Height;
			_pickupObjectsType = pickableObject.type;
		}

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
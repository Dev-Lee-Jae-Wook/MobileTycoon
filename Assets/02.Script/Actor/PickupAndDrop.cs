using EverythingStore.Animation;
using EverythingStore.InteractionObject;
using EverythingStore.Optimization;
using EverythingStore.ProjectileMotion;
using EverythingStore.Timer;
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
		[SerializeField] private int _maxPickup;
		[SerializeField] private CoolTime _actionCoolTime;

		private Rig _rig;
		private BezierCurve _bezierCurve;
		private Stack<PickableObject> _pickObjectStack = new();
		private float _nextHeight;

		private PickableObjectType _pickupObjectsType = PickableObjectType.None;
		#endregion

		#region Property
		/// <summary>
		/// 현재 픽업한 아이템의 갯수
		/// </summary>
		public int pickUpObjectCount => _pickObjectStack.Count;

		public PickableObjectType pickupObjectsType => _pickupObjectsType;

		public int MaxPickup => _maxPickup;
		#endregion

		#region Event
		public event Action OnAnimationPickup;
		public event Action OnAnimationDrop;
		#endregion

		#region UnityCycle
		private void Awake()
		{
			_actionCoolTime = gameObject.AddComponent<CoolTime>();
			_bezierCurve = GetComponent<BezierCurve>();
			_rig = transform.GetComponentInChildren<Rig>();
			SetRigWeight(0.0f);
			OnAnimationPickup += () => SetRigWeight(1.0f);
			OnAnimationDrop += () => SetRigWeight(0.0f);
			_nextHeight = 0.0f;
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

			if (IsRunningCoolTime() == true)
			{
				return false;
			}

			return pickUpObjectCount < MaxPickup;
		}

		public bool CanDrop()
		{
			if (IsRunningCoolTime() == true)
			{
				return false;
			}

			return pickUpObjectCount > 0;
		}

		private bool IsRunningCoolTime()
		{
			return _actionCoolTime.IsRunning == true;
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
		public PickableObject Pop()
		{
			if( _pickObjectStack.Count == 0 )
			{
				return null;
			}

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
		/// Callback 연출로 픽업이 완료 되었을 때 호출 된다.
		/// </summary>
		public void Pickup(PickableObject pickableObject, Action callback = null)
		{
			_bezierCurve.Movement(pickableObject.transform, _pickupPoint, _pickupPoint.position.y + 1.0f, GetPickupLocalPosition(),
				() =>
				{
					OnAnimationPickup?.Invoke();
					callback?.Invoke();
				});
			Push(pickableObject);
			_actionCoolTime.StartCoolTime(_coolTime);
		}

		/// <summary>
		/// 가장 위에 있는 아이템을 포물선 움직임 연출을 하면서 드랍합니다.
		/// </summary>
		public PickableObject Drop(Transform endTarget, Vector3 localPos,Action<PickableObject> callback = null)
		{
			PickableObject popObject = Pop();

			if (pickUpObjectCount == 0)
			{
				OnAnimationDrop?.Invoke();
			}

			_bezierCurve.Movement(popObject.transform, endTarget, endTarget.position.y + 1.0f, localPos,
				()=>callback?.Invoke(popObject));
			_actionCoolTime.StartCoolTime(_coolTime);

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
		public void SetMaxPickup(int capacity)
		{
			_maxPickup = capacity;
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

		public bool IsRunningDrop()
		{
			return _bezierCurve.MovementCount > 0;
		}


		#endregion

		#region Test Method
		public void Test_AddObject(PickableObject pickupObject)
		{
			Push(pickupObject);
		}
		#endregion
	}
}
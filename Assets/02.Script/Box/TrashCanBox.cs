using EverythingStore.Actor.Player;
using EverythingStore.InteractionObject;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.BoxBox
{
	public class TrashCanBox : MonoBehaviour, IPlayerInteraction
	{
		#region Field
		[SerializeField] private Transform _pivot;
		private Stack<Box> _trashBoxStack = new();
		[SerializeField] private float boundaryY;
		[SerializeField] private int _capacity;
		private Action _pushableCallback;
		#endregion

		#region Property
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Start()
		{
		}
		#endregion

		#region Public Method
		/// <summary>
		/// �����Ⱑ ���̴� ������ �ڽ��� �߰��մϴ�.
		/// </summary>
		public void PushTrashBox(Box box)
		{
			box.transform.parent = _pivot;
			box.transform.localPosition = GetPushLocalPosititon();
			_trashBoxStack.Push(box);
		}

		public bool IsFull()
		{
			return _trashBoxStack.Count == _capacity;
		}

		public void SetPushableCallback(Action callback)
		{
			_pushableCallback = callback;
		}

		//�������뿡�� �ڽ� �����⸦ �ݴ´�.
		public void InteractionPlayer(Player player)
		{
			if (_trashBoxStack.Count == 0)
			{
				return;
			}

			var pickup = player.PickupAndDrop;

			if(pickup.CanPickup(PickableObjectType.Box) == true)
			{
				var popBox = _trashBoxStack.Pop();
				pickup.Pickup(popBox);
				_pushableCallback?.Invoke();
				_pushableCallback = null;
			}
		}
		#endregion

		#region Private Method
		/// <summary>
		/// Ǫ���� �Ͽ��� ���� ���� �������� �޾ƿɴϴ�.
		/// Ǫ���� �ϱ����� �����;ߵ˴ϴ�.
		/// </summary>
		private Vector3 GetPushLocalPosititon()
		{
			return Vector3.up * (_trashBoxStack.Count * boundaryY);
		}


		#endregion

		#region Protected Method
		#endregion

	}
}

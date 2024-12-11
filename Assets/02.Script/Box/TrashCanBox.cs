using EverythingStore.Actor.Player;
using EverythingStore.InteractionObject;
using EverythingStore.Upgrad;
using System;
using System.Collections.Generic;
using TMPro;
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
		[SerializeField] private TMP_Text _capacityText;
		private Canvas _canvasCapacity;

		public event Action OnUpgrad;
		public event Action OnAllUpgard;

		public int Lv => throw new NotImplementedException();

		public int MaxLv => throw new NotImplementedException();

		#endregion

		#region Property
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Awake()
		{
			_canvasCapacity = _capacityText.canvas;
			UpdateUI(_capacity);
			ToggleMaxUI(true);
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
			ToggleMaxUI(false);
		}

		private void ToggleMaxUI(bool isOn)
		{
			_canvasCapacity.enabled = isOn;
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

				if(_trashBoxStack.Count == 0)
				{
					ToggleMaxUI(true);
				}
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

		public void Upgrad(int value)
		{
			_capacity = value;
			UpdateUI(_capacity);
		}

		private void UpdateUI(int max)
		{
			_capacityText.text = $"MAX\n{max}";
		}

		public void Upgrad(int lv, out int nextCost)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Protected Method
		#endregion

	}
}

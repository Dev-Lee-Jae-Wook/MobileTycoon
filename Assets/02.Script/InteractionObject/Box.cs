using EverythingStore.Actor;
using EverythingStore.Actor.Player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.InteractionObject
{
	public class Box : MonoBehaviour,IPlayerInteraction
	{
		#region Filed
		[SerializeField] private GachaProbaility _gachaData;
		private Stack<SellObject> _items;

		//�ڽ��� ���¿� ���� ���־� ������Ʈ��
		[SerializeField] private List<GameObject> _boxVisuals;

		/// <summary>
		/// ������ �����۵��� ��ġ
		/// </summary>
		private Transform _itemSpawnPoint;

		/// <summary>
		/// ������ ����
		/// </summary>
		private enum State
		{
			BeforeOpen,
			Open,
			Emtpy
		}

		private State _state;
		#endregion

		#region Events
		/// <summary>
		/// ���ڰ� ������ �� ȣ��Ǵ� �̺�Ʈ
		/// </summary>
		public event Action OnOpenBox;

		/// <summary>
		/// ���ڿ� ������ ��ԵǾ��� �� ȣ��Ǵ� �̺�Ʈ
		/// </summary>
		public event Action OnEmtpyBox;

		#endregion

		#region UnityLifeCycle
		private void Awake()
		{
			_items = new Stack<SellObject>();
			_itemSpawnPoint = transform.Find("SpawnPoint");
		}
		#endregion

		#region Private Method

		/// <summary>
		/// ���ڸ� �����մϴ�.
		/// </summary>
		private void Open()
		{
			SetUp();
			ChangeState(State.Open);
		}

		/// <summary>
		/// ���ڿ��� ������ �����ϴ�.
		/// </summary>
		private SellObject PopSellObject(PickupAndDrop hand)
		{
			var sellObject = _items.Pop();

			if (_items.Count == 0)
			{
				EmptyBox();
			}

			return sellObject;
		}

		/// <summary>
		/// ���ڿ� ���� �������� �������� �����մϴ�.
		/// </summary>
		private void SetUp()
		{
			var createItem = _gachaData.Gacha();
			SellObject clone = Instantiate(createItem, _itemSpawnPoint);
			_items.Push(clone);
		}

		/// <summary>
		/// ���ڸ� ����ڷ� ��ȯ�մϴ�.
		/// </summary>
		private void EmptyBox()
		{
			ChangeState(State.Emtpy);
		}

		/// <summary>
		/// ������ ���¿� ���� �´� �ൿ�� �����մϴ�.
		/// </summary>
		private void ChangeState(State state)
		{
			_boxVisuals[(int)_state].SetActive(false);
			_state = state;
			switch (_state)
			{
				case State.BeforeOpen:
					break;
				case State.Open:
					OnOpenBox?.Invoke();
					break;
				case State.Emtpy:
					OnEmtpyBox?.Invoke();
					//Destroy(gameObject);
					break;
			}
			_boxVisuals[(int)_state].SetActive(true);
		}

		void IPlayerInteraction.InteractionPlayer(PickupAndDrop hand)
		{
			switch (_state)
			{
				case State.BeforeOpen:
					Open();
					break;
				case State.Open:
					if(hand.CanPickup() == true)
					{
						var popObject = PopSellObject(hand);
						hand.ProductionPickup(popObject);
					}
					break;
				case State.Emtpy:
					break;
			}
		}

		#endregion
	}
}

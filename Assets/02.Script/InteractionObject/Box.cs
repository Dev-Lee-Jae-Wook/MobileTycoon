using EverythingStore.Actor;
using EverythingStore.Actor.Player;
using EverythingStore.Gacha;
using EverythingStore.Optimization;
using EverythingStore.Sell;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.InteractionObject
{
	public class Box : PickableObject,IPlayerInteraction
	{
		#region Filed
		public override PickableObjectType type => PickableObjectType.Box;
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

		private BoxCollider _collider;

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
			_collider = GetComponent<BoxCollider>();
			var pooledObjecet = GetComponent<PooledObject>();
			pooledObjecet.OnRelease += Init;
		}
		#endregion

		#region Public Method
		public void InteractionPlayer(Player player)
		{
			PickupAndDrop pickup = player.PickupAndDrop;

			switch (_state)
			{
				case State.BeforeOpen:
					Open();
					break;
				case State.Open:
					if (pickup.CanPickup(_items.Peek().type) == true)
					{
						var popObject = PopSellObject(pickup);
						pickup.Pickup(popObject);
						if (_items.Count == 0)
						{
							EmptyBox();
						}
					}
					break;
			}
		}

		#endregion

		#region Private Method
		private void Init()
		{
			ChangeState(State.BeforeOpen);
			_collider.enabled = true;
		}

		/// <summary>
		/// ���ڸ� �����մϴ�.
		/// </summary>
		private void Open()
		{
			SpawnSellObject();
			ChangeState(State.Open);
		}

		/// <summary>
		/// ���ڿ��� ������ �����ϴ�.
		/// </summary>
		private SellObject PopSellObject(PickupAndDrop hand)
		{
			var sellObject = _items.Pop();
			sellObject.transform.parent = null;
			return sellObject;
		}

		/// <summary>
		/// ���ڿ� ���� �������� �������� �����մϴ�.
		/// </summary>
		private void SpawnSellObject()
		{
			var sellObjectPrefab = GachaManger.Instance.Gacha();
			var sellObject = Instantiate(sellObjectPrefab);
			sellObject.transform.position = _itemSpawnPoint.position;
			_items.Push(sellObject);
		}

		/// <summary>
		/// ���ڸ� ����ڷ� ��ȯ�մϴ�.
		/// </summary>
		private void EmptyBox()
		{
			//��ȣ�ۿ뿡 �������� ���ϰ��Ѵ�.
			_collider.enabled = false;
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
					break;
			}
			_boxVisuals[(int)_state].SetActive(true);
		}

		internal void SetInteraction(bool isEnabled)
		{
			_collider.enabled = isEnabled;
		}

		#endregion

	}
}

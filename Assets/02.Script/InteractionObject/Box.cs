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

		//박스에 상태에 따른 비주얼 오브젝트들
		[SerializeField] private List<GameObject> _boxVisuals;

		/// <summary>
		/// 생성된 아이템들의 위치
		/// </summary>
		private Transform _itemSpawnPoint;

		/// <summary>
		/// 상자의 상태
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
		/// 상자가 열렸을 때 호출되는 이벤트
		/// </summary>
		public event Action OnOpenBox;

		/// <summary>
		/// 상자에 아이템 비게되었을 때 호출되는 이벤트
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
		/// 상자를 오픈합니다.
		/// </summary>
		private void Open()
		{
			SpawnSellObject();
			ChangeState(State.Open);
		}

		/// <summary>
		/// 상자에서 아이템 꺼냅니다.
		/// </summary>
		private SellObject PopSellObject(PickupAndDrop hand)
		{
			var sellObject = _items.Pop();
			sellObject.transform.parent = null;
			return sellObject;
		}

		/// <summary>
		/// 상자에 들어가는 아이템을 랜덤으로 설정합니다.
		/// </summary>
		private void SpawnSellObject()
		{
			var sellObjectPrefab = GachaManger.Instance.Gacha();
			var sellObject = Instantiate(sellObjectPrefab);
			sellObject.transform.position = _itemSpawnPoint.position;
			_items.Push(sellObject);
		}

		/// <summary>
		/// 상자를 빈상자로 변환합니다.
		/// </summary>
		private void EmptyBox()
		{
			//상호작용에 반응하지 못하게한다.
			_collider.enabled = false;
			ChangeState(State.Emtpy);
		}

		/// <summary>
		/// 상자의 상태에 따라 맞는 행동을 정의합니다.
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

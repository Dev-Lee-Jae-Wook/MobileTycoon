using EverythingStore.Actor;
using EverythingStore.Actor.Player;
using EverythingStore.AssetData;
using EverythingStore.Delivery;
using EverythingStore.Gacha;
using EverythingStore.GameEvent;
using EverythingStore.Optimization;
using EverythingStore.Sell;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.InteractionObject
{
	public class Box : PickableObject,IPlayerInteraction, IPoolObject_CreateInitialization,IPoolObject_SpawnObjectInitialization, IPoolObject_GetInitialization
	{
		#region Filed
		public override PickableObjectType type => PickableObjectType.Box;
		private Stack<SellObject> _items;

		[SerializeField] private GachaProbabilityData _gachaProbabilityData;
		//�ڽ��� ���¿� ���� ���־� ������Ʈ��
		[SerializeField] private List<GameObject> _boxVisuals;
		/// <summary>
		/// ������ �����۵��� ��ġ
		/// </summary>
		private Transform _itemSpawnPoint;

		/// <summary>
		/// ������ ����
		/// </summary>
		public enum BoxState
		{
			BeforeOpen,
			Open,
			Emtpy
		}

		private BoxState _state;

		private BoxCollider _collider;
		private ObjectPoolManger _poolMagner;
		
		private Animator _animator;

		private bool _isPickupAble;

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
			_animator = GetComponent<Animator>();
			OnOpenBox += ProductBoxOpen;
		}
		#endregion

		#region Public Method
		public void CreateInitialization()
		{
			_items = new Stack<SellObject>();
			_itemSpawnPoint = transform.Find("SpawnPoint");
			_collider = GetComponent<BoxCollider>();
			var pooledObjecet = GetComponent<PooledObject>();
			foreach(var item in _boxVisuals)
			{
				item.transform.localPosition = Vector3.zero;
				item.gameObject.SetActive(false);
			}
		}

		public void SpawnObjectInitialization(ObjectPoolManger manger)
		{
			_poolMagner = manger;
			GetPoolObjectInitialization();
		}

		public void GetPoolObjectInitialization()
		{
			ChangeState(BoxState.BeforeOpen);
			_collider.enabled = false;
			_isPickupAble = false;
			_animator.SetTrigger("Init");
		}

		public void InteractionPlayer(Player player)
		{
			PickupAndDrop pickup = player.PickupAndDrop;

			switch (_state)
			{
				case BoxState.BeforeOpen:
					Open();
					break;
				case BoxState.Open:
					if (pickup.CanPickup(PickableObjectType.SellObject) == true && _isPickupAble == true)
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

		public void InteractionOn()
		{
			_collider.enabled = true;
		}

		/// <summary>
		/// ���ڸ� ����ڷ� ��ȯ�մϴ�.
		/// </summary>
		public void EmptyBox()
		{
			//��ȣ�ۿ뿡 �������� ���ϰ��Ѵ�.
			_collider.enabled = false;
			ChangeState(BoxState.Emtpy);
		}

		#endregion

		#region Private Method

		/// <summary>
		/// ���ڸ� �����մϴ�.
		/// </summary>
		private void Open()
		{
			SpawnSellObject();
			ChangeState(BoxState.Open);
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
			var sellObjectType = GachaManger.Instance.Gacha(_gachaProbabilityData).GetComponent<PooledObject>().Type;
			var sellObject = _poolMagner.GetPoolObject(sellObjectType);

			sellObject.transform.parent = _itemSpawnPoint;
			sellObject.transform.localPosition = Vector3.zero;
			sellObject.transform.localRotation = Quaternion.identity;

			_items.Push(sellObject.GetComponent<SellObject>());
		}



		/// <summary>
		/// ������ ���¿� ���� �´� �ൿ�� �����մϴ�.
		/// </summary>
		private void ChangeState(BoxState state)
		{
			_boxVisuals[(int)_state].SetActive(false);
			_state = state;
			switch (_state)
			{
				case BoxState.BeforeOpen:
					break;
				case BoxState.Open:
					OnOpenBox?.Invoke();
					break;
				case BoxState.Emtpy:
					OnEmtpyBox?.Invoke();
					break;
			}
			_boxVisuals[(int)_state].SetActive(true);
		}

		private void PickupAble()
		{
			_isPickupAble = true;
		}

		private void ProductBoxOpen()
		{
			_animator.SetTrigger("BoxOpen");
		}



		#endregion

	}
}

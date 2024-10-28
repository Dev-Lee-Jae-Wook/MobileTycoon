using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

namespace EverythingStore.InteractionObject
{
	public class Box : MonoBehaviour
	{
		#region Filed
		[SerializeField] private GachaProbaility _gachaData;
		private Stack<SellObject> _items;

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

		private void Start()
		{
			Open();
		}
		#endregion

		#region Methods

		#region Public
		/// <summary>
		/// ���ڸ� �����մϴ�.
		/// </summary>
		public void Open()
		{
			SetUp();
			OnOpenBox?.Invoke();
		}

		/// <summary>
		/// ���ڿ��� ������ �����⸦ �õ��մϴ�.
		/// </summary>
		public bool TryPopSellObject(out SellObject sellObject)
		{
			sellObject = null;

			if (_items.Count > 0)
			{
				sellObject = _items.Pop();

				if (_items.Count == 0)
				{
					EmptyBox();
				}
				return true;
			}

			return false;
		}
		#endregion

		#region Private
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
		#endregion

		#endregion

		/// <summary>
		/// ������ ���¿� ���� �´� �ൿ�� �����մϴ�.
		/// </summary>
		private void ChangeState(State state)
		{
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
					Destroy(gameObject);
					break;
			}
		}
	}
}

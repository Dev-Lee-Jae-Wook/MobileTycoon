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
		}

		private void Start()
		{
			Open();
		}
		#endregion

		#region Methods

		#region Public
		/// <summary>
		/// 상자를 오픈합니다.
		/// </summary>
		public void Open()
		{
			SetUp();
			OnOpenBox?.Invoke();
		}

		/// <summary>
		/// 상자에서 아이템 꺼내기를 시도합니다.
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
		/// 상자에 들어가는 아이템을 랜덤으로 설정합니다.
		/// </summary>
		private void SetUp()
		{
			var createItem = _gachaData.Gacha();
			SellObject clone = Instantiate(createItem, _itemSpawnPoint);
			_items.Push(clone);
		}

		/// <summary>
		/// 상자를 빈상자로 변환합니다.
		/// </summary>
		private void EmptyBox()
		{
			ChangeState(State.Emtpy);
		}
		#endregion

		#endregion

		/// <summary>
		/// 상자의 상태에 따라 맞는 행동을 정의합니다.
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

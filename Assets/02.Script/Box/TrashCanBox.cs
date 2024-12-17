using EverythingStore.Actor.Player;
using EverythingStore.InteractionObject;
using EverythingStore.Optimization;
using EverythingStore.Save;
using EverythingStore.Upgrad;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace EverythingStore.BoxBox
{
	public class TrashCanBox : MonoBehaviour, IPlayerInteraction, ISave
	{
		#region Field
		[SerializeField] private Transform _pivot;
		[SerializeField] private float boundaryY;
		[SerializeField] private TMP_Text _capacityText;
		[SerializeField] private UpgradSystemInt _upgradSystem;


		private Action _pushableCallback;
		private Stack<Box> _trashBoxStack = new();
		private Canvas _canvasCapacity;
		private int _capacity;

		private TrashBoxData _saveData;

		public string SaveFileName => "TrashCanBoxData";
		#endregion

		#region Property
		#endregion

		#region Event
		#endregion

		#region UnityCycle

		void Start()
		{
			InitSaveData();
			InitializeUpgrad();
			InitializeUI();
			LoadTrashBox();
		}

		private void LoadTrashBox()
		{
			foreach (PooledObjectType type in _saveData.trashBoxTypes)
			{
				if (type != PooledObjectType.None)
				{
					Box box = ObjectPoolManger.Instance.GetPoolObject(type).GetComponent<Box>();
					box.EmptyBox();
					PushTrashBox(box, true);
				}
			}
		}

		private void InitializeUpgrad()
		{
			_upgradSystem.Inititalize(_saveData.lv, _saveData.progressMoney, SetCapacity);
		}

		private void InitializeUI()
		{
			_canvasCapacity = _capacityText.canvas;
			UpdateUI(_capacity);
			ToggleMaxUI(true);
		}
		#endregion

		#region Public Method
		/// <summary>
		/// 쓰레기가 모이는 곳으로 박스를 추가합니다.
		/// </summary>
		public void PushTrashBox(Box box, bool isIntialize = false)
		{
			box.transform.parent = _pivot;
			box.transform.localPosition = GetPushLocalPosititon();
			_trashBoxStack.Push(box);
			ToggleMaxUI(false);
			PooledObjectType type = box.GetComponent<PooledObject>().Type;

			if (isIntialize == false)
			{
				_saveData.PushTrashBoxTypes(type);
			}
		}

		public void SetCapacity(int value)
		{
			_capacity = value;
			UpdateUI(_capacity);
		}

		public bool IsFull()
		{
			return _trashBoxStack.Count == _capacity;
		}

		/// <summary>
		/// 푸쉬가 불가능한 상태일 때 대기 시킨다.
		/// </summary>
		/// <param name="callback"></param>
		public void SetPushableCallback(Action callback)
		{
			_pushableCallback = callback;
		}

		//쓰레기통에서 박스 쓰레기를 줍는다.
		public void InteractionPlayer(Player player)
		{
			if (_trashBoxStack.Count == 0)
			{
				return;
			}

			var pickup = player.PickupAndDrop;

			if(pickup.CanPickup(PickableObjectType.Box) == true)
			{
				Box box = PopBox();
				pickup.Pickup(box);
				_pushableCallback?.Invoke();
				_pushableCallback = null;

				if (_trashBoxStack.Count == 0)
				{
					ToggleMaxUI(true);
				}
			}
		}

		private Box PopBox()
		{
			_saveData.PopTrashBox();
			return _trashBoxStack.Pop();
		}

		public void InitSaveData()
		{
			if(SaveSystem.HasSaveData(SaveFileName) == false)
			{
				_saveData = new();
				Save();
			}
			else
			{
				_saveData = SaveSystem.LoadData<TrashBoxData>(SaveFileName);
			}

			SaveManager.Instance.RegisterSave(this);
		}

		public async void Save()
		{
			await SaveSystem.SaveData<TrashBoxData>(_saveData, SaveFileName);
		}

		#endregion

		#region Private Method
		private void ToggleMaxUI(bool isOn)
		{
			_canvasCapacity.enabled = isOn;
		}

		/// <summary>
		/// 푸쉬를 하였을 때의 로컬 포지션을 받아옵니다.
		/// 푸쉬를 하기전에 가져와야됩니다.
		/// </summary>
		private Vector3 GetPushLocalPosititon()
		{
			return Vector3.up * (_trashBoxStack.Count * boundaryY);
		}

		private void UpdateUI(int max)
		{
			_capacityText.text = $"MAX\n{max}";
		}

		#endregion

	}
}

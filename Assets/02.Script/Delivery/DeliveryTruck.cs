using EverythingStore.InteractionObject;
using EverythingStore.Optimization;
using EverythingStore.BoxBox;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;


public class DeliveryTruck : MonoBehaviour
{
	#region Field
	[SerializeField] private ObjectPoolManger _poolManger;
	[Title("Point")]
	[SerializeField] private Transform _initPoint;
	[SerializeField] private Transform _arrivePoint;
	[SerializeField] private Transform _exitPoint;

	[Title("BoxStorage")]
	[SerializeField] private BoxStorage _boxStorage;

	private Queue<Box> _boxQueue = new();
	
	private Animator _animator;
	#endregion

	#region Property
	public bool IsDelivery { get; private set; }
	#endregion

	#region Event
	public event Action OnFinshDelivey;
	#endregion

	#region UnityCycle
	private void Awake()
	{
		_animator = GetComponent<Animator>();
	}

	#endregion

	#region Public Method

	/// <summary>
	/// ��� ���μ����� �����մϴ�.
	/// </summary>
	public void StartDeliveryProcess()
	{
		_animator.SetTrigger("Delivery");
	}



	/// <summary>
	/// �ڽ��� ������ �ڽ� ������� �Ѱ��ݴϴ�.
	/// </summary>
	private void Delivery()
	{
		while(_boxQueue.Count > 0)
		{
			var box = _boxQueue.Dequeue();
			box.gameObject.SetActive(true);
			_boxStorage.AddBox(box);
		}
	}

	/// <summary>
	/// �ʱ� ��ġ�� ���ư��� ����� ������ �˸��ϴ�.
	/// </summary>
	private void FinshDelivery()
	{
		OnFinshDelivey?.Invoke();
	}
	#endregion

	#region Private Method
	/// <summary>
	/// ������ �ڽ��� �߰��մϴ�.
	/// </summary>
	/// <param name="pooledObject"></param>
	private void AddBox(PooledObject pooledObject)
	{
		pooledObject.gameObject.SetActive(false);
		_boxQueue.Enqueue(pooledObject.GetComponent<Box>());
	}

	public void SpawnBox(BoxOrderData[] datas)
	{
		int amount;
		foreach (var item in datas)
		{
			amount = item.Amount;
			while (amount > 0)
			{
				AddBox(GetBox(item.Type));
				amount--;
			}
		}
	}


	/// <summary>
	/// BoxType�� PooledObjectType���� ��ȯ�մϴ�.
	/// </summary>
	private PooledObjectType ConvertPooledObjectType(BoxType boxType)
	{
		PooledObjectType result = PooledObjectType.Box_Normal;

		switch (boxType)
		{
			case BoxType.Rare:
				result = PooledObjectType.Box_Rare;
				break;
			case BoxType.Unique:
				result = PooledObjectType.Box_Unique;
				break;
			case BoxType.Lengendary:
				result = PooledObjectType.Box_Lengendary;
				break;
		}

		return result;
	}

	private PooledObject GetBox(BoxType boxType)
	{
		var pooledObjectType = ConvertPooledObjectType(boxType);
		return _poolManger.GetPoolObject(pooledObjectType);
	}

	#endregion


	#region Protected Method
	#endregion

}

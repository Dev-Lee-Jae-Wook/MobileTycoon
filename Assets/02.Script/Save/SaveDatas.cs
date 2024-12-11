using EverythingStore.GameEvent;
using EverythingStore.Optimization;
using System;
using System.Collections.Generic;
using static EverythingStore.InteractionObject.Box;

namespace EverythingStore.Save
{
	public abstract class SaveData { }

	[Serializable]
	public class GameTarget : SaveData
	{
		public GameTargetType Type;

		public GameTarget()
		{
			Type = GameTargetType.Tutorial_GameStart;
		}
	}

	[Serializable]
	public class PlayerData : SaveData
	{
		/// <summary>
		/// Player Money
		/// </summary>
		public int money;

		/// <summary>
		/// Player Speed Lv
		/// </summary>
		public int speedLv;

		/// <summary>
		/// Player PickupLv
		/// </summary>
		public int pickupLv;

		public PlayerData()
		{
			money = 0;
			speedLv = 0;
			pickupLv = 0;
		}
	}

	[Serializable]
	public class SaleStandData : SaveData
	{
		/// <summary>
		/// ���� ����� �ǸŴ��� ����
		/// </summary>
		public int salesStandCount;

		/// <summary>
		/// ������ �ǸŴ��� ����
		/// </summary>
		public int lastSalesStandLv;

		public PooledObjectType[] SalesStandPutSellObjects_0;
		public PooledObjectType[] SalesStandPutSellObjects_1;
		public PooledObjectType[] SalesStandPutSellObjects_2;

		public int[] salesStandPushSellObjectIndex;

		public int[] progressMoneys;

		public bool salesStand1Unlock;
		public bool salesStand1UnlockAreaVisable;
		public int salesStand1UnlockAreaProgress;

		public bool salesStand2Unlock;
		public bool salesStand2UnlockAreaVisable;
		public int salesStand2UnlockAreaProgress;


		public SaleStandData()
		{
			salesStandCount = 1;
			lastSalesStandLv = 0;
			
			SalesStandPutSellObjects_0 = new PooledObjectType[18];

			SalesStandPutSellObjects_1 = new PooledObjectType[18];

			SalesStandPutSellObjects_2 = new PooledObjectType[18];

			salesStandPushSellObjectIndex = new int[3];

			salesStand1Unlock = false;
			salesStand1UnlockAreaVisable = false;
			salesStand1UnlockAreaProgress = 0;

			salesStand1Unlock = false;
			salesStand2UnlockAreaVisable = false;
			salesStand2UnlockAreaProgress = 0;

			progressMoneys = new int[3] ;
		}

		public PooledObjectType[] GetSalesStandPutSellObjects(int index)
		{
			if (index == 0)
				return SalesStandPutSellObjects_0;

			if (index == 1)
				return SalesStandPutSellObjects_1;

			return SalesStandPutSellObjects_2;
		}

		public void Clear(int index)
		{
			var saleStandPutSellObject = GetSalesStandPutSellObjects(index);

			for(int i = 0; i < saleStandPutSellObject.Length; i++)
			{
				saleStandPutSellObject[i] = PooledObjectType.None;
			}

			salesStandPushSellObjectIndex[index] = 0;
		}

		public void AddSalesStandPushSellObject(int index, PooledObjectType type)
		{
			var saleStandPutSellObject = GetSalesStandPutSellObjects(index);
			var pushSellObjectIndex = salesStandPushSellObjectIndex[index];
			saleStandPutSellObject[pushSellObjectIndex] = type;
			salesStandPushSellObjectIndex[index]++;
		}

	}

	[Serializable]
	public class AuctionData : SaveData
	{
		/// <summary>
		/// ����� �رݵǾ� �ִ°�?
		/// </summary>
		public bool isUnlock;
		/// <summary>
		/// ���� ��Ÿ��
		/// </summary>
		public float coolTime;

		public AuctionData()
		{
			isUnlock = false;
			coolTime = 0;
		}
	}

	[Serializable]
	public class BoxStroageData : SaveData
	{
		/// <summary>
		/// ��í �ڽ��� Ÿ�� ���� ��� None���� ����
		/// </summary>
		public PooledObjectType outputBoxType;

		/// <summary>
		/// ��í �ڽ��� ����
		/// </summary>
		public BoxState outputBoxState;

		public Queue<PooledObjectType> stroageBoxQueue;

		BoxStroageData()
		{
			outputBoxType = PooledObjectType.None;
			outputBoxState = BoxState.BeforeOpen;
			stroageBoxQueue = new Queue<PooledObjectType>();
		}
	}

	[Serializable]
	public class TrashBoxData : SaveData
	{
		/// <summary>
		/// ����
		/// </summary>
		public int lv;
		/// <summary>
		/// �����ִ� �ڽ� ����
		/// </summary>
		public int putBoxCount;

		public TrashBoxData()
		{
			lv = 0;
			putBoxCount = 0;
		}
	}
}
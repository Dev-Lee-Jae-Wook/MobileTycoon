using EverythingStore.GameEvent;
using EverythingStore.InteractionObject;
using EverythingStore.Optimization;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using static EverythingStore.InteractionObject.Box;

namespace EverythingStore.Save
{
	public abstract class SaveData { }

	[Serializable]
	public class GameTargetData : SaveData
	{
		public GameTargetType Type;

		public GameTargetData()
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

		public float worldPos_X;
		public float worldPos_Y;
		public float worldPos_Z;

		public PooledObjectType[] PickupObjects;

		public PlayerData(float posX, float posY, float posZ)
		{
			money = 0;
			speedLv = 0;
			pickupLv = 0;
			worldPos_X = posX;
			worldPos_Y = posY;
			worldPos_Z = posZ;
		}

		public void UpdatePickupObject(PooledObjectType[] pickupObjects)
		{
			PickupObjects = pickupObjects;
		}
	}

	[Serializable]
	public class SaleStandData : SaveData
	{
		/// <summary>
		/// 현재 언락된 판매대의 갯수
		/// </summary>
		public int salesStandCount;

		/// <summary>
		/// 마지막 판매대의 레벨
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
		/// 언락이 해금되어 있는가?
		/// </summary>
		public bool isUnlock;

		/// <summary>
		/// 해금에 들어간 돈
		/// </summary>
		public int progressMoney;

		/// <summary>
		/// 옥션 쿨타임
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
		/// 가챠 박스의 타입 없는 경우 None으로 설정
		/// </summary>
		public PooledObjectType outputBoxType;

		/// <summary>
		/// 가챠 박스의 상태
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
		/// 레벨
		/// </summary>
		public int lv;

		/// <summary>
		/// 업그레이드 진행 돈
		/// </summary>
		public int progressMoney;

		public PooledObjectType[] trashBoxTypes;
		public int lastIndex;

		public TrashBoxData()
		{
			lv = 0;
			progressMoney = 0;
			trashBoxTypes = new PooledObjectType[18];
			for (int i = 0; i < trashBoxTypes.Length; i++)
			{
				trashBoxTypes[i] = PooledObjectType.None;
			}
			lastIndex = -1;
		}

		public void PushTrashBoxTypes(PooledObjectType type)
		{
			lastIndex++;
			trashBoxTypes[lastIndex] = type;
		}

		public void PopTrashBox()
		{
			trashBoxTypes[lastIndex] = PooledObjectType.None;
			lastIndex--;
		}

		public void Clear()
		{
			for (int i = 0; i < trashBoxTypes.Length; i++)
			{
				trashBoxTypes[i] = PooledObjectType.None;
			}
			lastIndex = 0;
		}
	}
}
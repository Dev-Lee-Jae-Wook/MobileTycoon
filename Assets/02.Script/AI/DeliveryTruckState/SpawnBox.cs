using EverythingStore.Optimization;
using EverythingStore.BoxBox;

namespace EverythingStore.AI.DeliveryTruckState
{
	public class SpawnBox : DeliveryTruckBase, IFSMState
	{
		public SpawnBox(DeliveryTruck truck) : base(truck)
		{
		}

		public FSMStateType Type => FSMStateType.DeliveryTruck_SpawnBox;

		public void Enter()
		{
			int amount = 0;
			foreach (var item in truck.OrderData)
			{
				amount = item.Amount;
				while (amount > 0)
				{
					truck.AddBox(GetBox(item.Type));
					amount--;
				}
			}
		}

		public FSMStateType Excute()
		{
			return FSMStateType.DeliveryTruck_MoveTo_ArrivePoint;
		}

		public void Exit()
		{

		}

		/// <summary>
		/// BoxType을 PooledObjectType으로 변환합니다.
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
			return truck.PoolManger.GetPoolObject(pooledObjectType);
		}
	}
}

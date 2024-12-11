using EverythingStore.Actor;
using EverythingStore.Actor.Customer;
using EverythingStore.GameEvent;
using EverythingStore.InteractionObject;
using EverythingStore.RayInteraction;

namespace EverythingStore.AI.CustomerState
{
	public  class SalesStandPickupSellObject : CustomerStateBase, IFSMState
	{
		#region Field
		private PickupAndDrop _pickup;
		private Counter _counter;
		private SalesStand _salesStand;
		#endregion

		#region Property
		public FSMStateType Type => FSMStateType.Customer_Interaction_SaleStation;
		#endregion

		#region Public Method
		public SalesStandPickupSellObject(Customer owner) : base(owner)
		{
			_pickup = owner.pickupAndDrop;
		}

		public void Enter()
		{
			_salesStand = owner.EnterSalesStand;
		}

		public FSMStateType Excute()
		{
			FSMStateType next = Type;
			if(_pickup.pickUpObjectCount < owner.BuyCount)
			{
				_salesStand.InteractionCustomer(_pickup);
			}
			else if(_pickup.pickUpObjectCount == owner.BuyCount)
			{
				_salesStand.ExitCustomer();
				return FSMStateType.Customer_Enter_Counter;
			}

			return next;
		}

		public void Exit()
		{
			if(GameEventManager.Instance.GameTarget == GameTargetType.Tutorial_Pickup)
			{
				Tutorial.Instance.GoToCounter();
			}
		}
		#endregion

	}
}

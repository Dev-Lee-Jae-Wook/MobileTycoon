using EverythingStore.Actor;
using EverythingStore.Actor.Customer;

namespace EverythingStore.AI.CustomerState
{
	public  class CounterCaculationWait : CustomerStateBase, IFSMState
	{
		#region Field
		private PickupAndDrop _pickupAndDrop;
		#endregion

		#region Property
		public FSMStateType Type => FSMStateType.Customer_CounterCaculationWait;
		#endregion

		#region Public Method
		public CounterCaculationWait(Customer owner) : base(owner)
		{
			_pickupAndDrop = owner.pickupAndDrop;
		}

		public void Enter()
		{
			
		}

		public FSMStateType Excute()
		{
			FSMStateType next = Type;

			if(_pickupAndDrop.HasPickupObject() == true)
			{
				next = FSMStateType.Customer_GoToOutSide;
			}

			return next;
		}

		public void Exit()
		{
		}
		#endregion

	}
}

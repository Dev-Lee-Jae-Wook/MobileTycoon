using EverythingStore.Actor;
using EverythingStore.Actor.Customer;
using EverythingStore.InteractionObject;

namespace EverythingStore.AI.CustomerState
{
	public class CounterDropSellObject : CustomerStateBase, IFSMState
	{
		#region Field
		private PickupAndDrop _pickupAndDrop;
		private ICustomerInteraction _counter;
		#endregion

		#region Property
		public FSMStateType Type => FSMStateType.Customer_Counter_DropSellObject;
		#endregion

		#region Public Method
		public CounterDropSellObject(Customer owner, ICustomerInteraction counter) : base(owner)
		{
			_pickupAndDrop = owner.pickupAndDrop;
			_counter = counter;
		}

		public void Enter()
		{

		}

		public FSMStateType Excute()
		{
			FSMStateType next = Type;

			if (_pickupAndDrop.HasPickupObject() == true)
			{
				_counter.InteractionCustomer(_pickupAndDrop);
			}
			else
			{
				next = FSMStateType.Customer_Counter_WaitSendPackage;
			}

			return next;
		}

		public void Exit()
		{
		}
		#endregion

	}
}

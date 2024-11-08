using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.AI
{
	public enum FSMStateType
	{
		None,
		Customer_MoveTo_SalesStation,
		Customer_Interaction_SaleStation,
		Customer_MoveTo_Counter,
		Customer_Counter_DropSellObject,
		Customer_Counter_WaitSendPackage,
		Customer_GoOutSide,
		Customer_TriggerWait,
		Customer_GoToCounter,
		Customer_EnterSalesStand,
		Stop,
		Customer_MoveTo_EnterPoint_SalesStand,
		Customer_MoveTo_EnterPoint_Counter,
		Customer_EnterCounter,
		Customer_ExitStore,
	}
}

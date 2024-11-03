using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.AI
{
	public enum FSMStateType
	{
		None,
		Customer_MoveToSalesStation,
		Customer_SaleStationWait,
		Customer_MoveToCounter,
		Customer_CounterDropSellObject,
		Customer_CounterCaculationWait,
		Customer_GoToOutSide,
		Customer_TriggerWait,
		Customer_GoToCounter,
	}
}

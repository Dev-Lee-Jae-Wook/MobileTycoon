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
		DeliveryTruck_SpawnBox,
		DeliveryTruck_BoxSendToStoreage,
		DeliveryTruck_MoveTo_ArrivePoint,
		DeliveryTruck_MoveTo_InitArrivePointMidPoint,
		DeliveryTruck_BoxSendToStoreSorage,
		DeliveryTruck_MoveTo_Exit,
		DeliveryTruck_MoveTo_ExitPoint,
		DeliveryTruck_DeliveryEnd,
		CustomerAuction_Sitdown,
		CustomerAuction_AuctionWait,
		CustomerAuction_EnterStore,
		CustomerAuction_EnterAuction,
		CustomerAuction_MoveToSitDown,
		CustomerAuction_ResultCheck,
		CustomerAuction_DoAcution,
		CustomerAuction_TakeAuctionItem,
		CustomerAuction_MoveToExit,
		CustomerAuction_Reaction_Fail,
		ExitStore,
		CustomerAuction_MoveTo_SuccesBid,
		CustomerAuction_Reaction_Resentful,
		CustomerAuction_Reaction_SucessAuctionItem,
		CustomerAuction_MoveTo_MidPoint,
		CustomerAuction_MoveTo_Table,
	}
}

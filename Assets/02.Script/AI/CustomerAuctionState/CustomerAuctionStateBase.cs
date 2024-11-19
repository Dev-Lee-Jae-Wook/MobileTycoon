using EverythingStore.Actor.Customer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.AI.CustomerStateAuction
{
	public abstract class CustomerAuctionStateBase
	{
		#region Field
		protected CustomerAuction owner;
		#endregion

		#region Public Method
		public CustomerAuctionStateBase(CustomerAuction owner)
		{
			this.owner = owner;
		}
		#endregion

	}
}

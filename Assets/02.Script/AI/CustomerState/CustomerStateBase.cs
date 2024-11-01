using EverythingStore.Actor.Customer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.AI
{
	public abstract class CustomerStateBase
	{
		#region Field
		protected Customer owner;
		#endregion

		#region Public Method
		public CustomerStateBase(Customer owner)
		{
			this.owner = owner;
		}
		#endregion

	}
}

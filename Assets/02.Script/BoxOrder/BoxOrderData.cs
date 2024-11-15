using System;

namespace EverythingStore.OrderBox
{
	[Serializable]
	public struct BoxOrderData
	{
		public BoxType Type;
		public int Amount;
	}
}
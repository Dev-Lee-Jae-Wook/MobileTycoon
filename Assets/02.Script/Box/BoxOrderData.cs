using System;

namespace EverythingStore.BoxBox
{
	[Serializable]
	public struct BoxOrderData
	{
		public BoxType Type;
		public int Amount;

		public BoxOrderData(BoxType type, int amount)
		{
			Type = type;
			Amount = amount;
		}
	}
}
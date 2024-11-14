using EverythingStore.InteractionObject;
using UnityEngine;

namespace EverythingStore.Sell
{
	public class SellObject: PickableObject
	{
		[field:SerializeField] public int Money {  get; private set; }
		[field: SerializeField] public SellObjectRank Rank { get; private set; }
		public override PickableObjectType type => PickableObjectType.SellObject;
		
	}
}
using UnityEngine;

namespace EverythingStore.InteractionObject
{
	public class SellObject: PickableObject
	{
		[field:SerializeField] public int Money {  get; private set; }
		public override PickableObjectType type => PickableObjectType.SellObject;
	}
}
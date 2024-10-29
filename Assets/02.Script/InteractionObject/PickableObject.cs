using UnityEngine;

namespace EverythingStore.InteractionObject
{ 
	public abstract class PickableObject : MonoBehaviour
	{
		public enum PickableObjectType
		{
			None,
			SellObject,
			Package,
		}

		public abstract PickableObjectType type { get; }
	}
}

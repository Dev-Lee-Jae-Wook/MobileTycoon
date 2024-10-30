using UnityEngine;

namespace EverythingStore.InteractionObject
{ 
	public abstract class PickableObject : MonoBehaviour
	{
		[field: SerializeField] public float Height { get; private set; }
		public enum PickableObjectType
		{
			None,
			SellObject,
			Package,
		}

		public abstract PickableObjectType type { get; }
	}
}

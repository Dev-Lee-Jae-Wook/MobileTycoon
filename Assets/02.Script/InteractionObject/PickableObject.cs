using UnityEngine;

namespace EverythingStore.InteractionObject
{
	public abstract partial class PickableObject : MonoBehaviour
	{
		[field: SerializeField] public float Height { get; private set; }

		public abstract PickableObjectType type { get; }
	}
}

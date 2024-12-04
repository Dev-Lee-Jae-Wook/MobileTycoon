using UnityEngine;

namespace EverythingStore.GameEvent
{
	public abstract class GameEventBase:MonoBehaviour
	{
		public abstract GameEventType Type { get; }
		public abstract void OnEvent();
	}
}
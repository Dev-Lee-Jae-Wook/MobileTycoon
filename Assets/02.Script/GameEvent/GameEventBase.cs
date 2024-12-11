using UnityEngine;

namespace EverythingStore.GameEvent
{
	public abstract class GameEventBase:MonoBehaviour
	{
		public abstract GameTargetType Type { get; }
		public abstract void OnEvent();
	}
}
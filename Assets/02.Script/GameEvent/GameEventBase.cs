using System;
using UnityEngine;

namespace EverythingStore.GameEvent
{
	public abstract class GameEventBase:MonoBehaviour
	{
		public abstract GameTargetType Type { get; }
		public virtual void OnEvent()
		{
			OnEvented?.Invoke();
		}

		public event Action OnEvented;
	}
}
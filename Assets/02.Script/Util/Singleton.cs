using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
	public static T Instance;

	private void Awake()
	{
		if(Instance != null)
		{ 
			Destroy(gameObject);
			return;
		}

		Instance = this as T;
		AwakeInit();
	}

	protected abstract void AwakeInit();

}
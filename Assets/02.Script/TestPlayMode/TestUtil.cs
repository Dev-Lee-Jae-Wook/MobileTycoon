using UnityEngine;

namespace EverythingStore.Util
{
	public class TestUtil
	{
		/// <summary>
		/// Resource File/TestObject 내부에 있는 게임 오브젝트를 생성하고 반환합니다.
		/// </summary>
		public static T InstantiateResource<T>(string name) where T : Object
		{
			return MonoBehaviour.Instantiate(Resources.Load<T>("TestObject/" + name));
		}

		public static T Instantiate<T>() where T : UnityEngine.Component
		{
			GameObject newGameObject = new GameObject("new");
			return newGameObject.AddComponent<T>();
		}
	}
}

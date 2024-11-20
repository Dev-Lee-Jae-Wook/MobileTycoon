using UnityEngine;

namespace EverythingStore.Util
{
	public class TestUtil
	{
		/// <summary>
		/// Resource File/TestObject ���ο� �ִ� ���� ������Ʈ�� �����ϰ� ��ȯ�մϴ�.
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

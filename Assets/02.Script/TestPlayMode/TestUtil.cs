using UnityEngine;

namespace EverythingStore.Util
{
	public class TestUtil
	{
		/// <summary>
		/// Resource File/TestObject ���ο� �ִ� ���� ������Ʈ�� �����ϰ� ��ȯ�մϴ�.
		/// </summary>
		public static T Instantiate<T>(string name) where T : Object
		{
			return MonoBehaviour.Instantiate(Resources.Load<T>("TestObject/" + name));
		}
	}
}

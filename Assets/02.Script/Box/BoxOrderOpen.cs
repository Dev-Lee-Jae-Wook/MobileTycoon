using EverythingStore.InteractionObject;
using UnityEngine;

namespace EverythingStore.BoxBox
{
	public class BoxOrderOpen : MonoBehaviour, ISwitch
	{
		[SerializeField] private BoxOrder _boxOrder;
		public void SwitchAction()
		{
			_boxOrder.Open();
		}
	}
}
using EverythingStore.InteractionObject;
using System;
using UnityEngine;

namespace EverythingStore.Actor
{
	public class Hand:MonoBehaviour
	{
		[SerializeField] private Transform _hand;
		private PickableObject _pickableObject;

		public int pickUpObjectCount {  get; private set; }

		public bool CanPickUp()
		{
			return true;
		}

		public void PickUp(PickableObject pickableObject)
		{
			pickableObject.transform.parent = _hand;
			pickableObject.transform.localPosition = Vector3.zero;
			pickableObject.transform.localRotation = Quaternion.identity;
			_pickableObject = pickableObject;
			pickUpObjectCount++;
		}

		public PickableObject Pop()
		{
			PickableObject popObject = _pickableObject;
			_pickableObject = null;
			pickUpObjectCount--;
			return popObject;
		}

		internal bool IsPickUpObject()
		{
			return _pickableObject != null;
		}

		public PickableObject PeekObject()
		{
			return _pickableObject;
		}
	}
}
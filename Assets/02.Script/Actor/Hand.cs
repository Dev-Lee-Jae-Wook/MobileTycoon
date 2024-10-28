using EverythingStore.InteractionObject;
using System;
using UnityEngine;

namespace EverythingStore.Actor
{
	public class Hand:MonoBehaviour
	{
		[SerializeField] private Transform _hand;

		public bool CanPickUp()
		{
			return true;
		}

		public void PickUp(SellObject sellObject)
		{
			sellObject.transform.parent = _hand;
			sellObject.transform.localPosition = Vector3.zero;
			sellObject.transform.localRotation = Quaternion.identity;
		}
	}
}
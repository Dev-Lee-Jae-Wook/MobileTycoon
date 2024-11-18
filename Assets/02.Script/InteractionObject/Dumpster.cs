using EverythingStore.Actor.Player;
using EverythingStore.Optimization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.InteractionObject
{
	public class Dumpster : MonoBehaviour, IPlayerInteraction
	{
		#region Field
		[SerializeField] private Transform _dropPoint;
		#endregion

		#region Property
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		#endregion

		#region Public Method
		public void InteractionPlayer(Player player)
		{
			var drop = player.PickupAndDrop;
			if(drop.CanDrop() == false)
			{
				return;
			}

			var dropObject = drop.PeekObject();

			if (dropObject.type != PickableObjectType.Box)
			{
				return;
			}

			var box = dropObject.GetComponent<PooledObject>();

			drop.Drop(
				_dropPoint,
				Vector3.zero,
				() =>
				{
					box.Release();
				});
		}
		#endregion

		#region Private Method
		#endregion

		#region Protected Method
		#endregion
	}
}

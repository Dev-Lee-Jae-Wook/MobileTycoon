using EverythingStore.Actor.Customer;
using EverythingStore.Delivery;

namespace EverythingStore.AI.DeliveryTruckState
{
	public abstract class DeliveryTruckBase
	{
		protected DeliveryTruck truck;

		protected DeliveryTruckBase(DeliveryTruck truck)
		{
			this.truck = truck;
		}
	}
}
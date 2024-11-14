using EverythingStore.Actor;
using EverythingStore.Actor.Customer;
using EverythingStore.AI;
using EverythingStore.AI.CustomerState;
using EverythingStore.InteractionObject;
using EverythingStore.Sell;
using EverythingStore.Util;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TestTools;
using static UnityEditor.VersionControl.Asset;

public class CustomerTest
{
	private const float TEST_Y = 100.0f;

	private Customer _customer;
	private SalesStand _salesStand;
	private SellObject _sellObject1;
	private SellObject _sellObject2;
	private Counter _counter;
	private List<IFSMState> states = new();


	[SetUp]
	public void Setup()
	{
		TestUtil.Instantiate<GameObject>("TestNavMesh");
		_customer = TestUtil.Instantiate<Customer>("TestCustomer");
		_salesStand = TestUtil.Instantiate<SalesStand>("TestSalesStand");
		_sellObject1 = TestUtil.Instantiate<SellObject>("TestSellObjectPenguin");
		_sellObject2 = TestUtil.Instantiate<SellObject>("TestSellObjectPenguin");
		_counter = TestUtil.Instantiate<Counter>("TestCounter");

		states.Add(new CounterWaitSendPackage(_customer));
		states.Add(new CounterDropSellObject(_customer));
		states.Add(new SaleStationInteraction(_customer));
	}

	[TearDown]
	public void Teardown()
	{
		Object.Destroy(_customer.gameObject);
		Object.Destroy(_salesStand.gameObject);
		Object.Destroy(_sellObject1.gameObject);
	}

	[UnityTest]
	public IEnumerator SaleStationPickUpObject()
	{
		_customer.transform.position = new Vector3(0, TEST_Y, 0);
		_salesStand.transform.position = new Vector3(0, TEST_Y, 1.0f);

		_customer.Setup(
		states,
		FSMStateType.Customer_Interaction_SaleStation
		);

		//판매대에 없는 상태에서 아이템 픽업 하고자 할 때
		_customer.Sensor.RayCastAndInteraction();
		yield return null;
		Assert.AreEqual(_customer.CurrentState, FSMStateType.Customer_Interaction_SaleStation);
		yield return null;
		
		//판매대에 아이템 추가
		_salesStand.PushSellObject(_sellObject1);
		_customer.Sensor.RayCastAndInteraction();

		yield return null;
		Assert.AreEqual(_customer.CurrentState, FSMStateType.Customer_MoveTo_Counter);
	}

	[UnityTest]
	public IEnumerator CounterDropSellObject()
	{

		var pickupAndDrop = _customer.GetComponent<PickupAndDrop>();
		pickupAndDrop.Pickup(_sellObject1);

		//손님이 카운터랑 상호 작용을 통해서 픽업한 오브젝트를 넘긴다.
		_counter.InteractionCustomer(pickupAndDrop);

		//통과 - counter에게 오브젝트가 있는지
		Assert.AreNotEqual(_counter.Money , 0);

		_counter.Package();
		_counter.SendPackageToCustomer();
		
		//통과 - 손님이 패키지를 받음
		Assert.AreEqual(pickupAndDrop.HasPickupObject(), true);

		yield return null;
	}

}

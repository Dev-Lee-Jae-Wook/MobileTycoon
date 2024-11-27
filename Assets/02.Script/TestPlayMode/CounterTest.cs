using System.Collections;
using EverythingStore.Actor.Customer;
using EverythingStore.InteractionObject;
using EverythingStore.Optimization;
using EverythingStore.Sell;
using EverythingStore.Util;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CounterTest
{
    private Counter _counter;
    private Customer _customer;
    private ObjectPoolManger _poolManger;

    [SetUp]
    public void SetUp()
    {
        GameObject testModel = TestUtil.InstantiateResource<GameObject>("CounterTestModel");
		_counter = testModel.GetComponentInChildren<Counter>();
        _customer = testModel.GetComponentInChildren<Customer>();
		_poolManger = testModel.GetComponentInChildren<ObjectPoolManger>();
	}

    
    [UnityTest]
    public IEnumerator LastObject()
    {
        //준비 단계
        var sellObject1 = _poolManger.GetPoolObject(PooledObjectType.SellObject_PC).GetComponent<SellObject>();
        var sellObject2 = _poolManger.GetPoolObject(PooledObjectType.SellObject_TRax).GetComponent<SellObject>();
        _counter.PushSellObject(sellObject1);
        _counter.PushSellObject(sellObject2);

        var lastObjectName = _counter.SellPackage.LastObject.gameObject.name;

        Assert.AreEqual(lastObjectName, sellObject2.gameObject.name);

		yield return null;
    }
}

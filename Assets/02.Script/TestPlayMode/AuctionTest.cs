using EverythingStore.Actor;
using EverythingStore.Actor.Customer;
using EverythingStore.AI;
using EverythingStore.AI.CustomerState;
using EverythingStore.AuctionSystem;
using EverythingStore.InteractionObject;
using EverythingStore.Sell;
using EverythingStore.Util;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class AuctionTest
{

	//[UnityTest]
	//public IEnumerator Buy()
	//{
	//	//준비물
	//	AuctionManger manger = TestUtil.Instantiate<AuctionManger>();
	//	List<AuctionParticipant> customerList = new();
	//	customerList.Add(new(manger.Submit, 100));
	//	customerList.Add(new(manger.Submit, 80));
	//	bool isRun = true;
	//	//액션
	//	while (isRun) 
	//	{
	//		foreach (var customer in customerList)
	//		{
	//			customer.TrySubmit();
	//		}
	//		isRun = customerList[0].CanSubmit() || customerList[1].CanSubmit();
	//	}

	//	//통과 체크
	//	Assert.AreEqual(manger.LastBid, customerList[0]);

	//	yield return null;
	//}

	//[UnityTest]
	//public IEnumerator RacieFail()
	//{
	//	//준비물
	//	AuctionManger manger = TestUtil.Instantiate<AuctionManger>();
	//	List<AuctionParticipant> customerList = new();

	//	manger.SetStartBidMoney(150);

	//	customerList.Add(new(manger.Submit, 100));
	//	customerList.Add(new(manger.Submit, 200));

	//	//통과 체크
	//	Assert.AreNotEqual(manger.LastBid, customerList[0]);

	//	yield return null;
	//}


	//[UnityTest]
	//public IEnumerator Priority()
	//{
	//	//준비물
	//	AuctionManger manger = TestUtil.Instantiate<AuctionManger>();
	//	List<AuctionParticipant> customerList = new();

	//	manger.SetStartBidMoney(100);

	//	customerList.Add(new(manger.Submit, 300, 1.0f));
	//	customerList.Add(new(manger.Submit, 400, 0.0f));

	//	float _time = 1.0f;
	//	//액션
	//	while (_time > 0.0f)
	//	{
	//		foreach (var customer in customerList)
	//		{
	//			customer.TrySubmit();
	//		}

	//		yield return null;
	//		_time -= Time.deltaTime;
	//	}


	//	//통과 체크
	//	Assert.AreEqual(manger.LastBid, customerList[0]);
	//}

	//[UnityTest]
	//public IEnumerator MoneyLevelUp()
	//{
	//	//준비물
	//	AuctionManger manger = TestUtil.Instantiate<AuctionManger>();
	//	List<AuctionParticipant> customerList = new();

	//	manger.SetStartBidMoney(100);

	//	customerList.Add(new(manger.Submit, 300));
	//	customerList.Add(new(manger.Submit, 400));

	//	float _time = 1.0f;

	//	manger.Submit.SetSubmitMinimumMoney(100);

	//	//액션
	//	while (_time > 0.0f)
	//	{
	//		foreach (var customer in customerList)
	//		{
	//			customer.TrySubmit();
	//		}

	//		manger.Submit.AddSubmitMinimumMoney(10);

	//		yield return null;
	//		_time -= Time.deltaTime;
	//	}


	//	//통과 체크
	//	Debug.Log(manger.Submit.SubmitMinimumMoney);
	//}


}

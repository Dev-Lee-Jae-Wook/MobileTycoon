using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EverythingStore.AuctionSystem
{
    public class AuctionUI : MonoBehaviour
    {
		#region Field
		[SerializeField] private AuctionManger _manager;
		[SerializeField] private Animator _boradTextAnimator;

		[Title("Auction")]
		[SerializeField] private GameObject _auctionDispaly;
		[SerializeField] private TMP_Text _lastBidMoney;
		[SerializeField] private Slider _auctionTimerSlider;

		[Title("Info")]
		[SerializeField] private TMP_Text _board;

		private int _auctionItemValue;
		#endregion

		#region Property
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Start()
		{
			_manager.OnSetAuctionTime += SetAuctionItem;

			_manager.OnUpdateBidMoney += UpdateMoney;
			_manager.OnSetupBidTimer += OpenAuctionDispaly;
			_manager.OnUpdateBidTimer += UpdateAuctionTimer;
			_manager.OnEndAuction += EndAuction;
			_manager.OnUpdateCloseTime += UpdateCloseTime;
			_manager.OnUpdateReadyCustomer += UpdateReadyCustomer;
			_manager.OnUpdateReadyWait += UpdateReadyWait;
			_manager.OnWaitAuctionItem += WaitAuctionItem;
			_manager.OnStartAuction += () => SetMoney(_auctionItemValue);

			CloseAuctionDisplay();
		}

		private void UpdateReadyWait(int time)
		{
			_board.text = $"Start Auction\n{time}";
		}

		private void UpdateReadyCustomer(int current, int max)
		{
			_board.text = $"Value : {_auctionItemValue}\nWait : {current}/{max}";
		}
		#endregion

		#region Public Method
		#endregion

		#region Private Method
		private void WaitAuctionItem()
		{
			_board.text = "Drop\nAuction Item";
		}


		private void UpdateMoney(int money)
		{
			_boradTextAnimator.SetTrigger("Update");
			SetMoney(money);
		}

		private void SetMoney(int money)
		{
			_lastBidMoney.text = money.ToString();
		}

		private void OpenAuctionDispaly(float time)
		{
			_auctionDispaly.SetActive(true);
			_auctionTimerSlider.maxValue = time;
			_auctionTimerSlider.value = time;
			_lastBidMoney.text = _auctionItemValue.ToString();
			_board.gameObject.SetActive(false);
		}
		private void CloseAuctionDisplay()
		{
			_auctionDispaly.SetActive(false);
			_board.gameObject.SetActive(true);
		}

		private void EndAuction(int finalBid)
		{
			CloseAuctionDisplay();
			_board.text = $"Final Bid\n{finalBid}";
		}

		private void UpdateAuctionTimer(float time)
		{
			_auctionTimerSlider.value = time;
		}

		private void UpdateCloseTime(float time)
		{
			_board.text = $"Close\n{time.ToString("F0")}";
		}

		private void SetAuctionItem(int value)
		{
			_auctionItemValue = value;
		}
		#endregion

		#region Protected Method
		#endregion

	}
}

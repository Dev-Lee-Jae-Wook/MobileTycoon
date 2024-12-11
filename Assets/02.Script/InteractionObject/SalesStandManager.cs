using EverythingStore.InputMoney;
using EverythingStore.InteractionObject;
using EverythingStore.Optimization;
using EverythingStore.Save;
using EverythingStore.Sell;
using EverythingStore.Upgrad;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.MaterialProperty;

namespace EverythingStore.Manager
{
	public class SalesStandManager : MonoBehaviour, ISave
	{
		#region Field
		[SerializeField] private SalesStand[] _salesStands;
		[SerializeField] private UnlockSystem[] _unlocks;
		private SaleStandData _salesStandData;
		#endregion

		#region Property
		public SalesStand[] SalesStands => _salesStands;
		public string SaveFileName => "SalesStandData";
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Awake()
		{
			InitializeSalesStandData();
			SaveManager.Instance.RegisterSave(this);
		}

		void Start() 
		{
			InitializeSalesStand();
			InitializeLockAreas();
			_salesStandData.salesStand1UnlockAreaVisable = true;
		}

		private void InitializeLockAreas()
		{
			UpgradSystemInt salesStand0 = _salesStands[0].UpgradSystem;
			UpgradSystemInt salesStand1 = _salesStands[1].UpgradSystem;

			_unlocks[0].Initialization(_salesStandData.salesStand1Unlock, _salesStandData.salesStand1UnlockAreaProgress);
			_unlocks[1].Initialization(_salesStandData.salesStand2Unlock, _salesStandData.salesStand2UnlockAreaProgress);
			//---------------------------------------------------------------------------------------
			if (_salesStandData.salesStand1UnlockAreaVisable == false)
			{
				_unlocks[0].gameObject.SetActive(false);
				
				salesStand0.OnAllComplete += () =>
				{
					_unlocks[0].gameObject.SetActive(true);
					_salesStandData.salesStand1UnlockAreaVisable = true;
				};
			}

			_unlocks[0].InputMoneyArea.OnUpdateMoney += (progressMoney) =>
			{
				_salesStandData.salesStand1UnlockAreaProgress = progressMoney;
			};

			_unlocks[0].OnUnlock += AddSalesStandCount;
			//---------------------------------------------------------------------------------------

			if (_salesStandData.salesStand2UnlockAreaVisable == false)
			{
				_unlocks[1].gameObject.SetActive(false);

				salesStand1.OnAllComplete += () =>
				{
					_unlocks[1].gameObject.SetActive(true);
					_salesStandData.salesStand2UnlockAreaVisable = true;
				};
			}

			_unlocks[1].InputMoneyArea.OnUpdateMoney += (progressMoney) =>
			{
				_salesStandData.salesStand2UnlockAreaProgress = progressMoney;
			};
			_unlocks[1].OnUnlock += AddSalesStandCount;
		}

		private void InitializeSalesStand()
		{
			//�ǸŴ� �ʱ�ȭ �غ�
			int saleStandCount = _salesStandData.salesStandCount;
			int lastUpgradLv = _salesStandData.lastSalesStandLv;
			List<PooledObjectType[]> sellObjectsList = new();
			sellObjectsList.Add(_salesStandData.SalesStandPutSellObjects_0);
			sellObjectsList.Add(_salesStandData.SalesStandPutSellObjects_1);
			sellObjectsList.Add(_salesStandData.SalesStandPutSellObjects_2);

			SetUpSaleStands(saleStandCount, lastUpgradLv, sellObjectsList);
		}

		private void InitializeSalesStandData()
		{
			if (SaveSystem.HasSaveData(SaveFileName) == false)
			{
				InitSaveData();
				Save();
			}
			else
			{
				_salesStandData = SaveSystem.LoadData<SaleStandData>(SaveFileName);
			}
		}

		#endregion

		#region Public Method
		public SalesStand EnterSalesStand()
		{
			SalesStand saleStand = null;
			for (int i = 0; i < _salesStands.Length && saleStand == null; i++)
			{
				if (_salesStands[i].IsEnterable() == true)
				{
					saleStand = _salesStands[i];
				}
			}
			return saleStand;
		}
		#endregion

		#region Private Method


		/// <summary>
		/// �ǸŴ���� �ʱ�ȭ �մϴ�.
		/// </summary>
		private void SetUpSaleStands(int openSalesStand, int lastSalesStandLv, List<PooledObjectType[]> sellObjectsList)
		{
			int maxLv = _salesStands[0].UpgradSystem.MaxLv;

			for(int i =0; i< _salesStands.Length; i++)
			{
				int currentSalesCount = i + 1;
				int progressMoney = _salesStandData.progressMoneys[i];
				PooledObjectType[] sellObjects = sellObjectsList[i];
				int lv = 0;

				//���� �ǸŴ밡 �����ִ� �ǸŴ� ���� ���� ��� Max�� �ʱ�ȭ
				if (currentSalesCount < openSalesStand)
				{
					lv = maxLv;
				}
				//������ �ǸŴ밡 ���� �ִ� �ǸŴ��� �������� ��� LV�� ���߾� �ʱ�ȭ
				else if(currentSalesCount == openSalesStand)
				{
					lv = lastSalesStandLv;
				}
				_salesStands[i].Inititalize(lv, progressMoney, sellObjectsList[i]);

				//���� ������ ���� �ǸŴ�� ��Ȱ��ȭ
				if(currentSalesCount > openSalesStand)
				{
					_salesStands[i].gameObject.SetActive(false);
				}

				_salesStands[i].UpgradSystem.OnUpgrad += () =>
				{
					_salesStandData.lastSalesStandLv++;
				};
			}

			_salesStands[0].UpgradSystem.InputMoneyArea.OnUpdateMoney += (money) => 
			{
				_salesStandData.progressMoneys[0] = money;
			};

			_salesStands[1].UpgradSystem.InputMoneyArea.OnUpdateMoney += (money) =>
			{
				_salesStandData.progressMoneys[1] = money;
			};

			_salesStands[2].UpgradSystem.InputMoneyArea.OnUpdateMoney += (money) =>
			{
				_salesStandData.progressMoneys[2] = money;
			};

		}

		public void InitSaveData()
		{
			_salesStandData = new();
		}

		public async void Save()
		{
			for (int i = 0; i < _salesStands.Length; i++)
			{
				_salesStandData.Clear(i);
				
				//����� �ִ� ������ �����ϱ�
				foreach(var sellObject in _salesStands[i].SalesObjectStack.Reverse())
				{
					PooledObject pooledObject = sellObject.GetComponent<PooledObject>();
					_salesStandData.AddSalesStandPushSellObject(i, pooledObject.Type);
				}
			}

			await SaveSystem.SaveData<SaleStandData>(_salesStandData, SaveFileName);
		}

		private void SetSellObject(SalesStand salesStand, Stack<PooledObjectType> sellObjectStack)
		{
			foreach (var item in sellObjectStack)
			{
				var sellObject = ObjectPoolManger.Instance.GetPoolObject(item).GetComponent<SellObject>();
				salesStand.PushSellObject(sellObject);
			}
		}

		private void AddSalesStandCount()
		{
			_salesStandData.salesStandCount++;
			_salesStandData.lastSalesStandLv = 0;

		}
		#endregion

		#region Protected Method
		#endregion

	}
}

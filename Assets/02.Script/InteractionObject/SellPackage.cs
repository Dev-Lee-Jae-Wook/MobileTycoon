using UnityEngine;

namespace EverythingStore.InteractionObject
{
	public class SellPackage: PickableObject
	{
		#region Field
		[SerializeField] private Transform _packagePoint;
		private int _totalMoney;

		[SerializeField] private GameObject _openPackage;
		[SerializeField] private GameObject _clasePackage;
		#endregion

		#region Property
		public bool IsPackage { get; private set; } = false;
		public Transform PackagePoint => _packagePoint;

		public override PickableObjectType type => PickableObjectType.Package;
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		#endregion

		#region Public Method
		/// <summary>
		/// 포장지에 구매 아이템을 추가합니다.
		/// </summary>
		public void AddSellItem(SellObject sellObject)
		{
			_totalMoney += sellObject.Money;
		}

		/// <summary>
		/// 포장을 하고 구매 비용을 반환합니다.
		/// </summary>
		public int Package()
		{
			Debug.Log($"포장을 완료합니다. 총 가격: {_totalMoney}");

			_openPackage.SetActive(false);
			_clasePackage.SetActive(true);
			IsPackage = true;

			return _totalMoney;
		}
		#endregion

		#region Private Method
		#endregion
	}
}
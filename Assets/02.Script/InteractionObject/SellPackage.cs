using UnityEngine;

namespace EverythingStore.InteractionObject
{
	public class SellPackage: PickableObject
	{
		#region Field
		[SerializeField] private Transform _packagePoint;

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
		/// 포장을 하고 구매 비용을 반환합니다.
		/// </summary>
		public void Package()
		{
			_openPackage.SetActive(false);
			_clasePackage.SetActive(true);
			IsPackage = true;
		}
		#endregion

		#region Private Method
		#endregion
	}
}
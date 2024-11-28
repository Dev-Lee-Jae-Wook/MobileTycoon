using EverythingStore.InteractionObject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.InputMoney
{
    public class UnlockSystem : MonoBehaviour
    {
		#region Field
		[SerializeField] private int _money;
		[SerializeField] private InputMoneyArea _inputMoneyArea;
		private IUnlock _unlock;
		#endregion

		#region Property
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Awake()
		{
			_unlock = GetComponent<IUnlock>();
			_inputMoneyArea.OnCompelte += UnLock;
		}

		private void Start()
		{
			_inputMoneyArea.SetUp(_money);
		}
		#endregion

		#region Private Method
		private void UnLock()
		{
			_unlock.Unlock();
			gameObject.SetActive(false);
		}
		#endregion
	}
}

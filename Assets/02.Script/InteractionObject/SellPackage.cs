using EverythingStore.Optimization;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

namespace EverythingStore.InteractionObject
{
	public class SellPackage: PickableObject,IPoolObject_CreateInitialization
	{
		#region Field
		[SerializeField] private Transform _packagePoint;
		[SerializeField] private AnimationClip _packageClip;
		[SerializeField] private GameObject _openPackage;
		[SerializeField] private GameObject _closePackage;

		private PooledObject _pooledObject;
		private PooledObject _lastObject;

		private Animator _animator;
		private bool _isPackage = false;
		private Action _packageCallback;
		private WaitForSeconds _productWait;
		#endregion

		#region Property
		public bool IsPackage => _isPackage;
		public Transform PackagePoint => _packagePoint;
		public PooledObject LastObject => _lastObject;
		public override PickableObjectType type => PickableObjectType.Package;
		#endregion

		#region Event
		#endregion

		#region UnityCycle

		#endregion

		#region Public Method
		public void CreateInitialization()
		{
			_pooledObject = GetComponent<PooledObject>();
			_animator = GetComponent<Animator>();
			_productWait = new(_packageClip.length);
		}

		[Button("Test")]
		/// <summary>
		/// 포장을 하고 구매 비용을 반환합니다.
		/// </summary>
		public void Package(Action callback)
		{
			_animator.SetTrigger("Package");
			_isPackage = true;
			_packageCallback = callback;
		}

		public void Push(PooledObject topObject)
		{
			_lastObject?.Release();

			topObject.transform.parent = _packagePoint;
			topObject.transform.localPosition = Vector3.zero;
			topObject.transform.localRotation = Quaternion.identity;
			topObject.transform.localScale = Vector3.one;

			_lastObject = topObject;
		}

		public void ClosePackage()
		{
			_lastObject.Release();
			_lastObject = null;
		}

		public void PackageEnd()
		{
			_packageCallback.Invoke();
		}
		#endregion

		#region Private Method
		private IEnumerator C_Package()
		{
			_animator.SetTrigger("Package");
			yield return _productWait;
			_isPackage = true;
		}

		private void Init()
		{
			_lastObject?.Release();
			_closePackage.SetActive(false);
			_openPackage.SetActive(true);
			_isPackage = false;
			_packageCallback = null;
		}


		#endregion
	}
}
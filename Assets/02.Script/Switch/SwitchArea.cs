using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.InteractionObject
{
    public class SwitchArea : MonoBehaviour
	{ 
		#region Field
        [SerializeField] private LayerMask _switchAbleLayer;
		[SerializeField] private Vector3 _size;
		[SerializeField] private GameObject _switchObject;
        private ISwitch _target;
		
		private bool _isDown = false;
		#endregion

		#region Property
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Awake()
		{
			_target = _switchObject.GetComponent<ISwitch>();
		}

		private void Update()
		{
            if (IsDetect() == true)
            {
				if (_isDown == false)
				{
					_target.SwitchAction();
					_isDown = true;
				}
            }
			else
			{
				_isDown = false;
			}

        }

		private void OnDrawGizmos()
		{
			Gizmos.DrawWireCube(transform.position, _size);
		}
		#endregion

		#region Public Method
		#endregion

		#region Private Method
		private bool IsDetect()
		{
			var hitColliders = Physics.OverlapBox(transform.position, _size * 0.5f, Quaternion.identity, _switchAbleLayer);
			if (hitColliders.Length > 0)
			{
				return true;
			}
			return false;
		}
		#endregion

		#region Protected Method
		#endregion


	}
}

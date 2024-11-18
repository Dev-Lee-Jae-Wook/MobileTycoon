using EverythingStore.InteractionObject;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.BoxBox
{
	public class OutputBox : MonoBehaviour
	{
		#region Field
		[SerializeField] private Box _box = null;
		[SerializeField] private Transform _boxPivot;
		[SerializeField] private float _boxReadyTime;
		[SerializeField] private TrashCanBox _trashCan;
		#endregion

		#region Property
		#endregion

		#region Event
		/// <summary>
		/// ��í �� �� �ִ� �ڽ��� ������ �� ȣ��˴ϴ�.
		/// </summary>
		public event Action OnEmptyBox;
		#endregion

		#region UnityCycle
		#endregion

		#region Public Method
		/// <summary>
		/// ���� �ڽ��� �ִ��� 
		/// </summary>
		public bool HasBox()
		{
			return _box != null;
		}

		/// <summary>
		/// �ڽ��� �߰��մϴ�.
		/// </summary>
		public void SetBox(Box box)
		{
			_box = box;
			_box.transform.parent = _boxPivot;
			_box.transform.localPosition = Vector3.down;
			_box.OnEmtpyBox += BoxToTrashCan;
			StartCoroutine(C_BoxReady());
		}

		#endregion

		#region Private Method
		private IEnumerator C_BoxReady()
		{
			float time = 0.0f;
			Vector3 localPos;
			while (time < _boxReadyTime)
			{
				float progress = time / _boxReadyTime;
				localPos = Vector3.Lerp(_box.transform.localPosition, Vector3.zero, progress);
				_box.transform.localPosition = localPos;
				yield return null;
				time += Time.deltaTime;
			}

			_box.transform.localPosition = Vector3.zero;
			_box.SetInteraction(true);
		}

		private void BoxToTrashCan()
		{
			if(_trashCan.IsFull() == true)
			{
				_trashCan.SetPushableCallback(BoxToTrashCan);
				return;
			}

			_trashCan.PushTrashBox(_box);
			_box.OnEmtpyBox -= BoxToTrashCan;
			_box = null;
			OnEmptyBox?.Invoke();
		}
		#endregion
	}
}

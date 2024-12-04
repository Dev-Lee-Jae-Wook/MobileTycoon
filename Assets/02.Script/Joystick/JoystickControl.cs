using EverythingStore.Actor.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Input
{
	public class JoystickControl : MonoBehaviour
	{
		[SerializeField] private PlayerInput _input;
		[SerializeField] private Joystick _joystik;

		private void Start()
		{
			_input.OnChangeControl += UpdateControl;
		}

		private void UpdateControl(bool isControl)
		{
			_joystik.OnPointerUp(null);
			_joystik.gameObject.SetActive(isControl);
		}
	}
}

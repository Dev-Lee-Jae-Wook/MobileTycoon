using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Animation
{
    public interface IAnimationEventPickupAndDrop
    {
        /// <summary>
        /// ĳ���Ͱ� ������ �Ⱦ��� ȣ��Ǵ� �̺�Ʈ (Drop 0.0, Pickup 1.0)
        /// </summary>
        event Action OnAnimationPickup;

		/// <summary>
		/// ĳ���Ͱ� ������ ����� ȣ��Ǵ� �̺�Ʈ (Drop 0.0, Pickup 1.0)
		/// </summary>
		event Action OnAnimationDrop;
    }
}

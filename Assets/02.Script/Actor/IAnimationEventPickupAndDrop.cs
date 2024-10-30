using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Animation
{
    public interface IAnimationEventPickupAndDrop
    {
        /// <summary>
        /// 캐릭터가 아이템 픽업시 호출되는 이벤트 (Drop 0.0, Pickup 1.0)
        /// </summary>
        event Action OnAnimationPickup;

		/// <summary>
		/// 캐릭터가 아이템 드랍시 호출되는 이벤트 (Drop 0.0, Pickup 1.0)
		/// </summary>
		event Action OnAnimationDrop;
    }
}

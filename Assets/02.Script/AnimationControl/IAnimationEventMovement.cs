using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Animation
{
    public interface IAnimationEventMovement
    {
        /// <summary>
        /// 캐릭터가 움직일 때 호출되는 이벤트 (0.0 Stop, 1.0 Move)
        /// </summary>
        event Action<float> OnAnimationMovement;
    }
}

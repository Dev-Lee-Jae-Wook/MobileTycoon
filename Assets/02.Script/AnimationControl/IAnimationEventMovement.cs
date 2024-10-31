using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Animation
{
    public interface IAnimationEventMovement
    {
        /// <summary>
        /// ĳ���Ͱ� ������ �� ȣ��Ǵ� �̺�Ʈ (0.0 Stop, 1.0 Move)
        /// </summary>
        event Action<float> OnAnimationMovement;
    }
}

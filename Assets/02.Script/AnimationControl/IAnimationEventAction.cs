using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Animation
{
    public interface IAnimationEventAction
    {
        /// <summary>
        /// 캐릭터가 앉기 이벤트
        /// </summary>
        event Action OnAnimationSitdown;
    }
}

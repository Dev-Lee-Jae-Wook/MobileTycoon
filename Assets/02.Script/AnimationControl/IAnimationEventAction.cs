using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Animation
{
    public interface IAnimationEventAction
    {
        /// <summary>
        /// ĳ���Ͱ� �ɱ� �̺�Ʈ
        /// </summary>
        event Action OnAnimationSitdown;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Animation
{
    public interface IAnimationEventMovement
    {
        event Action<float> OnMovement;
    }
}

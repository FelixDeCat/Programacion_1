using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IRemovable<T>
{
    void ConfigureToRemove(Action<T> deltoremove);
}

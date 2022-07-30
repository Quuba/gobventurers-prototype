using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Optional<T>
{
    [SerializeField] private bool enabled;
    [SerializeField] private T value;

    public Optional(T initialValue)
    {
        enabled = true;
        value = initialValue;
    }

    public bool Enabled => enabled;
    public T Value => value;

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class BoolObject : ScriptableObject
{
    [SerializeField]
    private bool value;

    public event Action OnUpdated;

    public bool Value
    {
        set
        {
            this.value = value;
            if (OnUpdated != null)
            {
                OnUpdated();
            }
        }
        get
        {
            return value;
        }
    }
}

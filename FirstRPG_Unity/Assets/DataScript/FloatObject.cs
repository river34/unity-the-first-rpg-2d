using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class FloatObject : ScriptableObject
{
    [SerializeField]
    private float value;

    public event Action OnUpdated;

    public float Value
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

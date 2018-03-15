using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class IntObject : ScriptableObject
{
    [SerializeField]
    private int value;

    public event Action OnUpdated;

    public int Value
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

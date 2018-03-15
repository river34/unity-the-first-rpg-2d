using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance;

    public IntObject RewardNum;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        DontDestroyOnLoad(this);

        RewardNum.Value = 0;
    }
}

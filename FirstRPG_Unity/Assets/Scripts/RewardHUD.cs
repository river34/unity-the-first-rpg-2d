using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardHUD : MonoBehaviour
{
    public IntObject RewardNum;

    public Text Num;

    private void Start()
    {
        RewardNum.OnUpdated += OnRewardNumUpdatedHandler;
    }

    public void SetNum(int num)
    {
        Num.text = "x " + num;
    }

    private void OnRewardNumUpdatedHandler()
    {
        SetNum(RewardNum.Value);
    }
}
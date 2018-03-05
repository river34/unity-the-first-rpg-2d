using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHUD : MonoBehaviour {

    public Image Item;

    public Text Num;

    public void Initialize(Item item, int num)
    {
        Item.sprite = item.Image;
        Num.text = "x " + num;
    }

    public void SetNum(int num)
    {
        Num.text = "x " + num;
    }
}

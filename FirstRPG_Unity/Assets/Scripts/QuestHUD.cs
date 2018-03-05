using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestHUD : MonoBehaviour {

    public Text Text;

    public Image Item;

    public Text Num;

    public Color CompletedColor = Color.yellow;

    public void Initialize(string text, Character target)
    {
        Text.text = "Go find " + target.Name;
        Item.gameObject.SetActive(false);
        Num.gameObject.SetActive(false);
    }

    public void Initialize(string text, Item item, int num)
    {
        Text.text = "Get";
        Item.sprite = item.Image;
        Num.text = "x " + num;
        Item.gameObject.SetActive(true);
        Num.gameObject.SetActive(true);
    }

    public void Complete()
    {
        Text.color = CompletedColor;
        Num.color = CompletedColor;
    }
}

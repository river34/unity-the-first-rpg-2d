using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Item : MonoBehaviour
{
    public string Name;

    [System.NonSerialized]
    public Sprite Image;

    private static int count;
    private bool inRangeOfPlayer;
    private Player player;
    private int id;

    private void Awake()
    {
        id = count;
        count++;
    }

    public int ID
    {
        get
        {
            return id;
        }
    }

    private void Start()
    {
        Image = GetComponent<SpriteRenderer>().sprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRangeOfPlayer = true;
            player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Pickup -= OnPickupHandler;
                player.Pickup += OnPickupHandler;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRangeOfPlayer = false;
            player = null;
        }
    }

    public void Reset()
    {
        gameObject.SetActive(true);
    }

    private void OnPickupHandler()
    {
        if (inRangeOfPlayer == true && player != null)
        {
            if (Inventory.Instance.AddItem(this) == true)
            {
                gameObject.SetActive(false);
            }
        }
    }
}

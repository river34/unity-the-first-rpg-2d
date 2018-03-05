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
    private Character player;
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

    private void Update()
    {
        if (inRangeOfPlayer == true && player != null)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                if (Inventory.Instance.AddItem(this) == true)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRangeOfPlayer = true;
            player = other.GetComponent<Character>();
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour {

    public static Inventory Instance;

    public event Action<Item> OnItemAdded;

    public ItemHUD ItemHUDPrefab;

    public Transform Holder;

    public int MaxNumItemTypes = 6;

    private Dictionary<string, int> items;
    private Dictionary<string, ItemHUD> itemHUDs;
    private int numItemTypes;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            items = new Dictionary<string, int>();
            itemHUDs = new Dictionary<string, ItemHUD>();
        }
        else
        {
            Destroy(this);
        }

        DontDestroyOnLoad(this);
    }

    public bool AddItem(Item item)
    {
        if (items.ContainsKey(item.Name) == false)
        {
            if (numItemTypes < MaxNumItemTypes)
            {
                numItemTypes++;
                items.Add(item.Name, 1);
            }
            else
            {
                DebugLog.Print(DebugLog.LogType.Warning, "cannot add item. inventory is full.");
                return false;
            }
        }
        else
        {
            items[item.Name]++;
        }

        if (itemHUDs.ContainsKey(item.Name) == false)
        {
            GameObject newItemHUD = Instantiate(ItemHUDPrefab.gameObject, Holder);
            ItemHUD itemHUD = newItemHUD.GetComponent<ItemHUD>();
            itemHUD.Initialize(item, items[item.Name]);
            itemHUDs.Add(item.Name, itemHUD);
        }
        else
        {
            itemHUDs[item.Name].SetNum(items[item.Name]);
        }

        if (OnItemAdded != null)
        {
            OnItemAdded(item);
        }

        return true;
    }

    public bool HasItem(Item item, int num)
    {
        if (items.ContainsKey(item.Name))
        {
            if (items[item.Name] >= num)
            {
                return true;
            }
        }
        return false;
    }
   
    public void Deduct(Item item, int num)
    {
        if (items.ContainsKey(item.Name))
        {
            if (items[item.Name] > num)
            {
                items[item.Name] -= num;
                if (itemHUDs.ContainsKey(item.Name))
                {
                    itemHUDs[item.Name].SetNum(items[item.Name]);
                }
            }
            else
            {
                numItemTypes--;
                items.Remove(item.Name);
                if (itemHUDs.ContainsKey(item.Name))
                {
                    Destroy(itemHUDs[item.Name].gameObject);
                    itemHUDs.Remove(item.Name);
                }
            }
        }
    }
}

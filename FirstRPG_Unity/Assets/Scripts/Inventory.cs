using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Inventory : MonoBehaviour {

    public static Inventory Instance;

    public event Action<Item> OnItemAdded;

    public event Action<Item, int> OnItemRemoved;

    public ItemHUD ItemHUDPrefab;

    public Transform Holder;

    public int MaxNumItemTypes = 6;

    private SortedDictionary<string, int> items;
    private Dictionary<string, ItemHUD> itemHUDs;
    private Dictionary<string, Item> itemPrefabs;
    private int numItemTypes;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            items = new SortedDictionary<string, int>();
            itemHUDs = new Dictionary<string, ItemHUD>();
            itemPrefabs = new Dictionary<string, Item>();
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

        if (itemPrefabs.ContainsKey(item.Name) == false)
        {
            itemPrefabs.Add(item.Name, item);
        }

        if (OnItemAdded != null)
        {
            OnItemAdded(item);
        }

        return true;
    }

    public void RemoveLastItem()
    {
        if (items.Count > 0)
        {
            var last = items.Keys.Last();

            if (itemPrefabs.ContainsKey(last))
            {
                if (OnItemRemoved != null)
                {
                    OnItemRemoved(itemPrefabs[last], items[last]);
                }
            }

            Debug.Log("Discarded " + last + " x " + items[last]);

            Deduct(last, items[last]);
        }
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

    public void Deduct(string name, int num)
    {
        if (items.ContainsKey(name))
        {
            if (items[name] > num)
            {
                items[name] -= num;
                if (itemHUDs.ContainsKey(name))
                {
                    itemHUDs[name].SetNum(items[name]);
                }
            }
            else
            {
                numItemTypes--;
                items.Remove(name);
                if (itemHUDs.ContainsKey(name))
                {
                    Destroy(itemHUDs[name].gameObject);
                    itemHUDs.Remove(name);
                }
            }
        }
    }
   
    public void Deduct(Item item, int num)
    {
        Deduct(item.Name, num);
    }
}

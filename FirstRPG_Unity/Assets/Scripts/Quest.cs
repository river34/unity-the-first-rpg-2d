using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Quest : RPGEvent
{
    public enum QuestType { Collect, Talk }

    public QuestType Type;

    [System.NonSerialized]
    public Character FromCharacter;

    public Character ToCharacter;

    public Item Item;

    public int Num;

    public int Reward;

    public event Action<Quest> OnCompleted, OnClosed;
    
    private enum Status { NotInitialized, Open, Completed, Closed };
    private Status status;
    private static int count;
    private int id;

    private void Awake()
    {
        id = count;
        count++;
    }

    private void Start()
    {
        FromCharacter = GetComponent<Character>();
    }

    public int ID
    {
        get
        {
            return id;
        }
    }

    private void Update()
    {
        if (status == Status.Open)
        {
            if (Type == QuestType.Collect)
            {
                if (Inventory.Instance.HasItem(Item, Num) == true)
                {
                    status = Status.Completed;
                    if (OnCompleted != null)
                    {
                        OnCompleted(this);
                    }
                }
            }
            else if (Type == QuestType.Talk)
            {
                if (ToCharacter.InRangeOfPlayer() == true)
                {
                    status = Status.Completed;
                    if (OnCompleted != null)
                    {
                        OnCompleted(this);
                    }
                }
            }
        }
    }

    public void SetClosed()
    {
        status = Status.Closed;
        if (OnClosed != null)
        {
            OnClosed(this);
        }
    }

    public void SetOpened()
    {
        status = Status.Open;
    }

    public bool IsCompleted()
    {
        return status == Status.Completed;
    }
}

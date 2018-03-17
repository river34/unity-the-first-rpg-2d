using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RPGEventTrigger : MonoBehaviour
{
    [NonSerialized]
    public Queue<RPGEvent> RPGEvents;

    public event Action<RPGEvent> OnRPGEventTriggered, OnRPGEventCompleted, OnRPGEventClosed;

    private bool isTriggered;

    private Quest openQuest;

    private void Awake()
    {
        RPGEvents = new Queue<RPGEvent>();

        RPGEvent[] rpgEvents = GetComponents<RPGEvent>();
        if (rpgEvents != null)
        {
            foreach (RPGEvent rpgEvent in rpgEvents)
            {
                RPGEvents.Enqueue(rpgEvent);
            }
        }
    }

    public bool HasDialogue()
    {
        if (RPGEvents != null && RPGEvents.Count > 0)
        {
            RPGEvent rpgEvent = RPGEvents.Peek();
            Dialogue dialogue = rpgEvent as Dialogue;
            if (dialogue != null)
            {
                return true;
            }
        }
        return false;
    }

    public bool HasQuest()
    {
        if (RPGEvents != null && RPGEvents.Count > 0)
        {
            RPGEvent rpgEvent = RPGEvents.Peek();
            Quest quest = rpgEvent as Quest;
            if (quest != null)
            {
                return true;
            }
        }
        return false;
    }

    public bool HasOpenQuest()
    {
        return openQuest != null;
    }

    public bool IsTriggered()
    {
        return isTriggered;
    }

    public void SetTriggered(bool isTriggered)
    {
        this.isTriggered = isTriggered;
    }

    public void SetName(string name)
    {
        if (RPGEvents != null && RPGEvents.Count > 0)
        {
            foreach (RPGEvent rpgEvent in RPGEvents)
            {
                rpgEvent.Name = name;
            }
        }
    }

    public bool TriggerRPGEvent()
    {
        if (RPGEvents.Count <= 0)
        {
            Debug.Log(gameObject.name + " no more events");
            return false;
        }

        RPGEvent rpgEvent = RPGEvents.Peek();

        Dialogue dialogue = (rpgEvent as Dialogue);
        if (dialogue != null)
        {
            Debug.Log(gameObject.name + " trigger dialogue");

            DialogueManager.Instance.AddDialogue(dialogue);
            DialogueManager.Instance.OnDialogueCompleted -= (incomingDialogue) => { };
            DialogueManager.Instance.OnDialogueCompleted += (incomingDialogue) =>
            {
                if (incomingDialogue == dialogue)
                {
                    Debug.Log(gameObject.name + " dialogue completed");
                    Debug.Log(gameObject.name + " dialogue = " + dialogue.Sentences.Length);
                    if (OnRPGEventClosed != null)
                    {
                        OnRPGEventClosed(dialogue);
                    }
                    Destroy(dialogue);
                }
            };
            if (OnRPGEventTriggered != null)
            {
                OnRPGEventTriggered(dialogue);
            }
            RPGEvents.Dequeue();
            return true;
        }

        Quest quest = (rpgEvent as Quest);
        if (quest != null)
        {
            Debug.Log(gameObject.name + " trigger quest");

            if (QuestManager.Instance.HasCapacity())
            {
                QuestManager.Instance.AddQuest(quest);
                QuestManager.Instance.OnQuestCompleted -= (incomingQuest) => { };
                QuestManager.Instance.OnQuestCompleted += (incomingQuest) =>
                {
                    if (incomingQuest == quest)
                    {
                        if (OnRPGEventCompleted != null)
                        {
                            OnRPGEventCompleted(quest);
                        }
                    }
                };
                QuestManager.Instance.OnQuestClosed -= (incomingQuest) => { };
                QuestManager.Instance.OnQuestClosed += (incomingQuest) =>
                {
                    if (incomingQuest == quest)
                    {
                        if (OnRPGEventClosed != null)
                        {
                            OnRPGEventClosed(quest);
                        }
                        Destroy(quest);
                    }
                };
                quest.SetOpened();
                openQuest = quest;
                if (OnRPGEventTriggered != null)
                {
                    OnRPGEventTriggered(quest);
                }
                RPGEvents.Dequeue();
            }
            else
            {
                DebugLog.Print(DebugLog.LogType.Warning, "cannot add quest. exceed max num of quests.");
                return false;
            }
            return true;
        }
        return true;
    }

    public void TryCloseQuest()
    {
        if (openQuest != null && openQuest.IsCompleted())
        {
            openQuest.SetClosed();
        }
    }
}

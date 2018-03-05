using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public event Action<Quest> OnQuestCompleted, OnQuestClosed;

    public int MaxNumQuests = 4;

    public QuestHUD QuestHUDPrefab;

    public Transform Holder;

    private List<Quest> openQuests;
    private List<Quest> closedQuests;
    private Dictionary<int, QuestHUD> questHUDs;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            openQuests = new List<Quest>();
            closedQuests = new List<Quest>();
            questHUDs = new Dictionary<int, QuestHUD>();
        }
        else
        {
            Destroy(this);
        }

        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        // debug log - print current quest
        if (Input.GetKeyDown(KeyCode.Q) && Input.GetKey(KeyCode.LeftControl))
        {
            DebugLog.Print(DebugLog.LogType.Debug, "quest manager - ");
            DebugLog.Print(DebugLog.LogType.Log, "  num of open quests = " + openQuests.Count);
            foreach (Quest quest in openQuests)
            {
                DebugLog.Print(DebugLog.LogType.Log, "    " + quest.Name);
            }
            DebugLog.Print(DebugLog.LogType.Log, "  num of closed quests = " + closedQuests.Count);
        }
    }

    public void AddQuest(Quest quest)
    {
        if (openQuests.Contains(quest)) return;
        if (questHUDs.ContainsKey(quest.ID)) return;

        if (HasCapacity() == false)
        {
            DebugLog.Print(DebugLog.LogType.Warning, "cannot add quest. exceed max num of quests.");
            return;
        }

        quest.OnCompleted -= OnCompletedHandler;
        quest.OnCompleted += OnCompletedHandler;

        quest.OnClosed -= OnClosedHandler;
        quest.OnClosed += OnClosedHandler;

        openQuests.Add(quest);

        GameObject newQuestHUD = Instantiate(QuestHUDPrefab.gameObject, Holder);
        QuestHUD questHUD = newQuestHUD.GetComponent<QuestHUD>();
        questHUDs.Add(quest.ID, questHUD);

        switch (quest.Type)
        {
            case Quest.QuestType.Collect:
                questHUD.Initialize(quest.name, quest.Item, quest.Num);
                break;

            case Quest.QuestType.Talk:
                questHUD.Initialize(quest.name, quest.ToCharacter);
                break;
        }
    }

    public void OnCompletedHandler(Quest quest)
    {
        if (questHUDs.ContainsKey(quest.ID))
        {
            questHUDs[quest.ID].Complete();
        }

        if (OnQuestCompleted != null)
        {
            OnQuestCompleted(quest);
        }
    }

    public void OnClosedHandler(Quest quest)
    {
        if (openQuests.Contains(quest))
        {
            openQuests.Remove(quest);
        }

        closedQuests.Add(quest);

        if (questHUDs.ContainsKey(quest.ID))
        {
            Destroy(questHUDs[quest.ID].gameObject);
        }

        if (quest.Type == Quest.QuestType.Collect)
        {
            Inventory.Instance.Deduct(quest.Item, quest.Num);
        }

        if (OnQuestClosed != null)
        {
            OnQuestClosed(quest);
        }
    }

    public bool HasCapacity()
    {
        return openQuests.Count < MaxNumQuests;
    }
}

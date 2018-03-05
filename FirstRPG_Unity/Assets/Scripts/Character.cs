using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Character : MonoBehaviour
{
    public string Name;

    public float MoveSpeed = 1f;

    public GameObject DialogueBark;

    public GameObject QuestBark;

    public BoolObject InDialogue;

    public AnimationClip IdleClip;

    public AnimationClip WalkingClip;

    protected Animator anim;
    protected enum MoveDirection { Up, Down, Left, Right };

    private bool isWalking, isWalkingLast;
    private bool isLeft, isLeftLast;
    private bool canMove;
    private Vector3 localScale;
    private RPGEventTrigger EventTrigger;
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

    protected virtual void Start()
    {
        if (QuestBark != null)
        {
            QuestBark.SetActive(false);
        }

        if (DialogueBark != null)
        {
            DialogueBark.SetActive(false);
        }

        EventTrigger = GetComponent<RPGEventTrigger>();

        if (EventTrigger != null && EventTrigger.RPGEvents.Count <= 0)
        {
            EventTrigger = null;
        }

        if (EventTrigger != null)
        {
            EventTrigger.SetName(Name);

            UpdateRPGEvent();
        }

        anim = GetComponent<Animator>();

        if (anim != null)
        {
            AnimatorOverrideController myNewOverrideController = new AnimatorOverrideController(anim.runtimeAnimatorController);
            myNewOverrideController.runtimeAnimatorController = anim.runtimeAnimatorController;
            if (IdleClip != null)
            {
                myNewOverrideController["KnightIdle"] = IdleClip;
            }
            if (WalkingClip != null)
            {
                myNewOverrideController["KnightWalking"] = WalkingClip;
            }
            myNewOverrideController.runtimeAnimatorController = anim.runtimeAnimatorController;
            anim.runtimeAnimatorController = myNewOverrideController;
        }

        localScale = transform.localScale;

        SetCanMove(true);

        InDialogue.OnUpdated += OnInDialogueUpdated;
    }

    protected virtual void Update()
    {
        isWalking = false;

        if (inRangeOfPlayer == true && player != null)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                if (EventTrigger != null && EventTrigger.IsTriggered() == false)
                {
                    EventTrigger.SetTriggered(true);
                    
                    if (EventTrigger.HasDialogue())
                    {
                        SetCanMove(false);
                        if (player != null)
                        {
                            player.SetCanMove(false);
                        }
                    }
                    
                    bool result = EventTrigger.TriggerRPGEvent();
                    if (result == false)
                    {
                        DebugLog.Print(DebugLog.LogType.Warning, "cannot triiger event");
                        EventTrigger.SetTriggered(false);
                    }
                }

                if (EventTrigger != null && EventTrigger.IsTriggered() == true)
                {
                    if (EventTrigger.HasOpenQuest() == true)
                    {
                        EventTrigger.TryCloseQuest();
                    }
                }
            }
        }
    }

    protected virtual void LateUpdate()
    {
        if (isWalking != isWalkingLast)
        {
            anim.SetBool("IsWalking", isWalking);
        }
        isWalkingLast = isWalking;

        if (isLeft != isLeftLast)
        {
            transform.localScale = new Vector3(-1 * localScale.x, localScale.y, localScale.z);
            localScale = transform.localScale;
        }
        isLeftLast = isLeft;
    }

    protected virtual void Move(MoveDirection direction)
    {
        if (canMove == false) return;

        switch (direction)
        {
            case MoveDirection.Up:
                transform.position += Vector3.up * MoveSpeed * Time.deltaTime;
                isWalking = true;
                break;
            case MoveDirection.Down:
                transform.position += Vector3.down * MoveSpeed * Time.deltaTime;
                isWalking = true;
                break;
            case MoveDirection.Left:
                transform.position += Vector3.left * MoveSpeed * Time.deltaTime;
                isWalking = true;
                isLeft = true;
                break;
            case MoveDirection.Right:
                transform.position += Vector3.right * MoveSpeed * Time.deltaTime;
                isWalking = true;
                isLeft = false;
                break;
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

    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }

    private void OnInDialogueUpdated()
    {
        if (InDialogue.Value == false && canMove == false)
        {
            SetCanMove(true);
        }
    }

    private void OnRPGEventTriggeredHandler(RPGEvent rpgEvent)
    {
        Dialogue dialogue = rpgEvent as Dialogue;

        if (dialogue != null)
        {
            if (DialogueBark != null)
            {
                DialogueBark.SetActive(false);
            }
            return;
        }

        Quest quest = rpgEvent as Quest;

        if (quest != null)
        {
            if (QuestBark != null)
            {
                QuestBark.SetActive(false);
            }
            return;
        }
    }

    private void OnRPGEventCompletedHandler(RPGEvent rpgEvent)
    {
        Quest quest = rpgEvent as Quest;

        if (quest != null)
        {
            if (QuestBark != null)
            {
                QuestBark.SetActive(true);
            }
        }
    }

    private void OnRPGEventClosedHandler(RPGEvent rpgEvent)
    {
        Quest quest = rpgEvent as Quest;

        if (quest != null)
        {
            if (QuestBark != null)
            {
                QuestBark.SetActive(false);
            }
        }
        
        EventTrigger.SetTriggered(false);
        UpdateRPGEvent();
    }

    public bool InRangeOfPlayer()
    {
        return inRangeOfPlayer;
    }

    private void UpdateRPGEvent()
    {
        if (EventTrigger.HasDialogue())
        {
            if (DialogueBark != null)
            {
                DialogueBark.SetActive(true);
            }

            EventTrigger.OnRPGEventTriggered -= OnRPGEventTriggeredHandler;
            EventTrigger.OnRPGEventTriggered += OnRPGEventTriggeredHandler;

            EventTrigger.OnRPGEventClosed -= OnRPGEventClosedHandler;
            EventTrigger.OnRPGEventClosed += OnRPGEventClosedHandler;
        }

        if (EventTrigger.HasQuest())
        {
            if (QuestBark != null)
            {
                QuestBark.SetActive(true);
            }

            EventTrigger.OnRPGEventTriggered -= OnRPGEventTriggeredHandler;
            EventTrigger.OnRPGEventTriggered += OnRPGEventTriggeredHandler;

            EventTrigger.OnRPGEventCompleted -= OnRPGEventCompletedHandler;
            EventTrigger.OnRPGEventCompleted += OnRPGEventCompletedHandler;

            EventTrigger.OnRPGEventClosed -= OnRPGEventClosedHandler;
            EventTrigger.OnRPGEventClosed += OnRPGEventClosedHandler;
        }
    }
}

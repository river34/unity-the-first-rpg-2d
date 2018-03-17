using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public event Action<Dialogue> OnDialogueCompleted;

    public Text NameText;

    public Text DialogueText;

    public Animator Animator;

    public BoolObject InDialogue;

    private Queue<string> sentences;
    private Queue<Dialogue> dialogueBuffer;
    private Dialogue currentDialogue;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            sentences = new Queue<string>();
            dialogueBuffer = new Queue<Dialogue>();
        }
        else
        {
            Destroy(this);
        }

        DontDestroyOnLoad(this);
    }

    public void AddDialogue(Dialogue dialogue)
    {
        Debug.Log("AddDialogue");
        dialogueBuffer.Enqueue(dialogue);

        StartDialogue();
    }

    private void StartDialogue()
    {
        Debug.Log("StartDialogue");
        Player.Instance.Jump -= OnJumpHandler;
        Player.Instance.Jump += OnJumpHandler;

        if (currentDialogue == null)
        {
            currentDialogue = dialogueBuffer.Dequeue();
        }

        InDialogue.Value = true;

        Animator.SetBool("IsOpen", true);

        if (currentDialogue.FromPlayer == true)
        {
            NameText.text = Player.Instance.Name;
        }
        else
        {
            NameText.text = currentDialogue.Name;
        }

        sentences.Clear();

        foreach (string sentence in currentDialogue.Sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        DialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            DialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        InDialogue.Value = false;

        Animator.SetBool("IsOpen", false);

        if (OnDialogueCompleted != null)
        {
            OnDialogueCompleted(currentDialogue);
        }

        Debug.Log("EndDialogue");
        currentDialogue = null;

        if (dialogueBuffer.Count > 0)
        {
            StartDialogue();
        }

        Player.Instance.Jump -= OnJumpHandler;
    }

    void OnJumpHandler()
    {
        Debug.Log("OnJumpHandler");
        if (InDialogue.Value == true)
        {
            DisplayNextSentence();
        }
    }
}

using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SentenceResponseFriend
{
    [TextArea] public string sentence;   
    [TextArea] public string response;   
}

public class Friend : MonoBehaviour
{
    private long[] friendPoints = new long[] { 100, 1000, 10000, 1000000, 999999999999999999L };

    [Header("UI + Game References")]
    public GameObject sentenceUI;                 // The UI panel parent
    public WordOrderingManager wordManager;
    public PlayerInventory playerInventory;

    [Header("Sentence/Response Mapping")]
    public List<SentenceResponseFriend> sentenceResponses;

    private int currentSentenceIndex = 0;
    public string npcName = "Friend";
    private bool playerInRange = false;
    private bool autoOpenNext = false;


    public string GetResponseForSentence(string playerSentence)
    {
        foreach (var sr in sentenceResponses)
        {
            if (playerSentence.Equals(sr.sentence, System.StringComparison.OrdinalIgnoreCase))
            {
                return sr.response;
            }
        }
        return "Sorry, I don’t understand that request.";
    }

    void Start()
    {
        LockMouse();

        //Ensure sentence UI is hidden initially
        if (sentenceUI != null)
            sentenceUI.SetActive(false);
    }
    void Update()
    {
        if (!playerInRange) return;
        if (playerInRange && autoOpenNext)
        {
            autoOpenNext = false;
            OpenNextSentenceUI();
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            
            OpenNextSentenceUI();

        }
    }

    private void OpenNextSentenceUI()
    {
        if (sentenceUI != null && currentSentenceIndex < sentenceResponses.Count)
        {
            wordManager.SetActiveFriend(this);
            OpenSentenceGame();

            string target = sentenceResponses[currentSentenceIndex].sentence;
            wordManager.GenerateChallenge(new List<string>(target.Split(' ')));

        }
        else
        {
            DialogueManager.Instance.ShowDialogue(npcName + ": And I love you too, my friend.");
            if (Progress.Instance != null) Progress.Instance.SetFriendDone();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            DialogueManager.Instance.ShowDialogue("Press E to talk to " + npcName);
            playerInventory = other.GetComponent<PlayerInventory>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            DialogueManager.Instance.ClearDialogue();
            if (sentenceUI != null)
                sentenceUI.SetActive(false);
            LockMouse();
        }
    }

    public void OpenSentenceGame()
    {
        if (sentenceUI != null)
            sentenceUI.SetActive(true);
        UnlockMouse();
    }

    public void CloseSentenceGame()
    {
        if (sentenceUI != null)
            sentenceUI.SetActive(false);
        LockMouse();
    }



    public void NextSentence()
    {
        currentSentenceIndex = Mathf.Min(currentSentenceIndex + 1, sentenceResponses.Count);
    }
    private void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public void OnSentenceComplete(string playerSentence)
    {

        if (currentSentenceIndex >= sentenceResponses.Count)
        {
            DialogueManager.Instance.ShowDialogue($"{npcName}: And I love you too, my friend.");
            CloseSentenceGame();
            if (Progress.Instance != null) Progress.Instance.SetFriendDone();
            return;
        }

        string npcResponse = sentenceResponses[currentSentenceIndex].response;
        DialogueManager.Instance.ShowDialogue($"{npcName}: {npcResponse}");

        long pointsToGive = 0;
        if (currentSentenceIndex < friendPoints.Length)
        {
            pointsToGive = friendPoints[currentSentenceIndex];
        }
        else
        {
            pointsToGive = friendPoints[friendPoints.Length - 1];
        }
        ScoreManager.Instance?.AddPoints(pointsToGive);


        Invoke(nameof(AdvanceAfterDelay), 2.2f);
    }

    private void AdvanceAfterDelay()
    {
        currentSentenceIndex++;
        autoOpenNext = true;
        CloseSentenceGame();
    }


    //private void UnlockNextSentenceChallenge()
    //{
    //    if (questStage < sentenceResponses.Count)
    //    {
    //        //currentSentenceIndex = questStage;
    //        string nextSentence = sentenceResponses[currentSentenceIndex].sentence;
    //        DialogueManager.Instance.ShowDialogue($"{npcName}: Let’s move to the next one!");
    //        wordManager.GenerateChallenge(new List<string>(nextSentence.Split(' ')));
    //        OpenSentenceGame();
    //    }
    //    else
    //    {
    //        DialogueManager.Instance.ShowDialogue($"{npcName}: Thank you, come again!");
    //    }
    //}
}

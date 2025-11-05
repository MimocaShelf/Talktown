using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SentenceResponsePharmacy
{
    [TextArea] public string sentence;
    [TextArea] public string response;
}

public class PharmacyNPCInteract : MonoBehaviour
{
    [Header("Item Request")]
    public ItemType requestedItem = ItemType.None;

    public List<ItemType> itemsEveryThird = new List<ItemType>();

    [Header("UI + Game References")]
    public GameObject sentenceUI;                 // The UI panel parent
    public WordOrderingManager wordManager;
    public PlayerInventory playerInventory;

    [Header("Sentence/Response Mapping")]
    public List<SentenceResponse> sentenceResponses;

    private int currentSentenceIndex = 0;
    public string npcName = "Pharmacist";
    private bool playerInRange = false;
    private bool autoOpenNext = false;

    private int questStage = 0;
    private bool waitingForItem = false;


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
        if (playerInRange && !waitingForItem && autoOpenNext)
        {
            autoOpenNext = false; 
            OpenNextSentenceUI();
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (waitingForItem)
            {
                TryResolveItemTurnIn();
                return;
            }
            OpenNextSentenceUI();
            
        }
    }

    private void OpenNextSentenceUI()
    {
        if (currentSentenceIndex < sentenceResponses.Count)
        {
            OpenSentenceGame();
            wordManager.SetActivePharmacy(this);
            //DialogueManager.Instance.ShowDialogue($"{npcName}:How can I be of help?");

            string answer = sentenceResponses[currentSentenceIndex].sentence;
            wordManager.GenerateChallenge(new List<string>(answer.Split(' ')));
        }
        else
        {
            DialogueManager.Instance.ShowDialogue($"{npcName}: That’s all for today. Take care!");
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


    //public void SetRequestedItem(ItemType type)
    //{
    //    requestedItem = type;

    //    DialogueManager.Instance.ShowDialogue($"{npcName}: Please bring me {type}.");
    //}

    public void OnSentenceComplete(string playerSentence)
    {

        if (currentSentenceIndex >= sentenceResponses.Count)
        {
            DialogueManager.Instance.ShowDialogue($"{npcName}: Thanks, that was helpful.");
            CloseSentenceGame();
            return;
        }

        string npcResponse = sentenceResponses[currentSentenceIndex].response;
        DialogueManager.Instance.ShowDialogue($"{npcName}: {npcResponse}");

        bool isEveryThird = ((currentSentenceIndex + 1) % 3 == 0);
        if (isEveryThird)
        {
            switch ((currentSentenceIndex + 1) / 3 - 1)   
            {
                case 0: 
                    requestedItem = ItemType.Syrup; 
                    break;
                case 1: 
                    requestedItem = ItemType.EyeDrops;
                    break;
                case 2: 
                    requestedItem =  ItemType.Lotion; 
                    break;
                default: 
                    requestedItem = ItemType.None; 
                    break;
            }

            waitingForItem = true;
            //DialogueManager.Instance.ShowDialogue($"{npcName}: Can you bring me some {requestedItem}?");
            CloseSentenceGame();
            return;
        }
        Invoke(nameof(AdvanceAfterDelay), 2.2f);
    }

    private void AdvanceAfterDelay()
    {
        currentSentenceIndex++;
        autoOpenNext = true;  
        CloseSentenceGame();
    }

    private void TryResolveItemTurnIn()
    {
        if (playerInventory == null)
        {
            DialogueManager.Instance.ShowDialogue($"{npcName}:  Did you not find the {requestedItem}?");
            return;
        }

        if (!playerInventory.HasItem)
        {
            DialogueManager.Instance.ShowDialogue($"{npcName}: You don't have the {requestedItem} yet.");
            return;
        }

        if (playerInventory.HeldItem == requestedItem)
        {
            DialogueManager.Instance.ShowDialogue($"{npcName}: Perfect! That’s the {requestedItem}!");
            playerInventory.TryConsume(requestedItem);

            waitingForItem = false;
            requestedItem = ItemType.None;
            currentSentenceIndex++;
            ScoreManager.Instance?.AddPoints(10);
            if (ScoreManager.Instance != null)
                ScoreManager.Instance.AddPoints(10);

            Invoke(nameof(UnlockNextSentenceChallenge), 2.2f);
        }
        else
        {
            DialogueManager.Instance.ShowDialogue($"{npcName}: That is not what you wanted. You asked for {requestedItem}.");
        }
    }


    private void UnlockNextSentenceChallenge()
    {
        if (questStage < sentenceResponses.Count)
        {
            //currentSentenceIndex = questStage;
            string nextSentence = sentenceResponses[currentSentenceIndex].sentence;
            DialogueManager.Instance.ShowDialogue($"{npcName}: Let’s move to the next one!");
            wordManager.GenerateChallenge(new List<string>(nextSentence.Split(' ')));
            OpenSentenceGame();
        }
        else
        {
            DialogueManager.Instance.ShowDialogue($"{npcName}: Thank you, come again!");
        }
    }
}

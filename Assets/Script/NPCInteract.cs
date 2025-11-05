using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SentenceResponse
{
    [TextArea] public string sentence;
    [TextArea] public string response;
}

    public class NPCInteract : MonoBehaviour
{ 

    [Header("Item Request")]
    public ItemType requestedItem = ItemType.None;
    [Header("UI + Game References")]
    public GameObject sentenceUI;                 // The UI panel parent
    public WordOrderingManager wordManager;
    public PlayerInventory playerInventory;

    [Header("Sentence/Response Mapping")]
    public List<SentenceResponse> sentenceResponses;  
    private int currentSentenceIndex = 0;
    private const int MaxSentences = 5;
    public string npcName = "NPC";
    [TextArea] public string dialogueLine = "Hello, welcome to Talktown!";
    private bool playerInRange = false;

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

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (waitingForItem)
            {
                TryResolveItemTurnIn();
                return;
            }
            if (questStage >= MaxSentences)
            {
                DialogueManager.Instance.ShowDialogue($"{npcName}: Thank you, come again!");
                return;
            }

            if (CompareTag("GroceryNPC"))
            {
                OpenSentenceGame();
                wordManager.SetActiveGrocery(this);
                DialogueManager.Instance.ShowDialogue($"{npcName}: Yes! How can I help?");

                if (wordManager != null && sentenceResponses.Count > 0)
                {
                    string sentence = sentenceResponses[currentSentenceIndex].sentence;
                    wordManager.GenerateChallenge(new List<string>(sentence.Split(' ')));
                }
            }
            else
            {
                DialogueManager.Instance.ShowDialogue(npcName + ": Welcome to Talktown!");

                DialogueChoiceManager.Instance.ShowChoices(
                    "Ask about the town", () => DialogueManager.Instance.ShowDialogue("This town is full of life!"),
                    "Ask about groceries", () => DialogueManager.Instance.ShowDialogue("There is a grocery store behind me. Check it out!"),
                    "Ask about food", () => DialogueManager.Instance.ShowDialogue("Try the local café!"),
                    "Say goodbye", () => DialogueManager.Instance.ShowDialogue("Goodbye! Come back anytime.")
                );
            }
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
        currentSentenceIndex++;

        if (currentSentenceIndex >= sentenceResponses.Count)
        {
            Debug.Log("No more sentences available for this NPC.");
        }
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
        if (questStage >= MaxSentences)
        {
            DialogueManager.Instance.ShowDialogue($"{npcName}: Thank you, come again!");
            CloseSentenceGame();
            return;
        }

        switch (questStage)
        {
            case 0:
                requestedItem = ItemType.Apples;
                break;

            case 1:
                requestedItem = ItemType.Milk;
                break;

            case 2:
                requestedItem = ItemType.Chips;
                break;

            case 3:
                requestedItem = ItemType.Water;
                break;
            
            case 4:
                requestedItem = ItemType.Bread;
                break;


            default:
                DialogueManager.Instance.ShowDialogue($"{npcName}: You're doing well!");
                return;
        }

        if (questStage < sentenceResponses.Count)
        {
            string npcResponse = sentenceResponses[questStage].response;
            DialogueManager.Instance.ShowDialogue($"{npcName}: {npcResponse}");
        }
        waitingForItem = true;
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
            questStage++;
            currentSentenceIndex = questStage;

            Invoke(nameof(UnlockNextSentenceChallenge), 3f);
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
            currentSentenceIndex = questStage;
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




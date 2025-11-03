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

    [Header("Sentence/Response Mapping")]
    public List<SentenceResponse> sentenceResponses;  
    private int currentSentenceIndex = 0;
    public string npcName = "NPC";
    [TextArea] public string dialogueLine = "Hello, welcome to Talktown!";
    private bool playerInRange = false;


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

        // Ensure sentence UI is hidden initially
        if (sentenceUI != null)
            sentenceUI.SetActive(false);
    }
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (CompareTag("GroceryNPC"))
            {
                OpenSentenceGame();
                DialogueManager.Instance.ShowDialogue(npcName + ": Yes! How can I help?");

                if (wordManager != null && sentenceResponses.Count > 0)
                {
                    // Get current sentence from mapping and generate challenge
                    string sentence = sentenceResponses[currentSentenceIndex].sentence;
                    wordManager.GenerateChallenge(new List<string>(sentence.Split(' ')));
                }
            TryResolveItemTurnIn();
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

        // Default follow-up if nothing matched
        DialogueManager.Instance.ShowDialogue(npcName + ": Can I help with anything else?");
    }



    public void NextSentence()
    {
        currentSentenceIndex++;

        if (currentSentenceIndex >= sentenceResponses.Count)
        {
            Debug.Log("No more sentences available for this NPC.");
            currentSentenceIndex = 0; // loop back, or remove this line if you don’t want looping
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


    public void SetRequestedItem(ItemType type)
    {
        requestedItem = type;

        DialogueManager.Instance.ShowDialogue($"{npcName}: Please bring me {type}.");
    }

     private void TryResolveItemTurnIn()
     {
         var player = FindAnyObjectByType<PlayerInventory>();

            if (!player.HasItem)
            {
                DialogueManager.Instance.ShowDialogue($"{npcName}: You’re not holding anything. Please bring {requestedItem}.");
                return;
            }

            if (player.HeldItem == requestedItem)
            {

                player.TryConsume(requestedItem);
                DialogueManager.Instance.ShowDialogue($"{npcName}: Perfect, that’s exactly the {requestedItem}.");
                requestedItem = ItemType.None; 
            }
            else
            {
                DialogueManager.Instance.ShowDialogue($"{npcName}: That’s not {requestedItem}. Let’s try again.");
                OpenSentenceGame(); 
            }
     }

    private void UnlockNextSentenceChallenge()
    {
        NextSentence(); 
        OpenSentenceGame(); 
    }
}




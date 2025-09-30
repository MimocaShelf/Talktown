using UnityEngine;

public class NPCInteract : MonoBehaviour
{
    public string npcName = "NPC";
    [TextArea] public string dialogueLine = "Hello, welcome to Talktown!";
    private bool playerInRange = false;
    public GameObject sentenceUI;             // The UI panel parent
    public WordOrderingManager wordManager;     // Drag your WordOrderingManager here

    void start()
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
                wordManager.GenerateChallenge(new System.Collections.Generic.List<string>
                { "I", "would", "like", "to", "buy", "milk" });

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

        // NPC follow-up
        DialogueManager.Instance.ShowDialogue(npcName + ": Can I help with anything else?");
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
}


using UnityEngine;

public class NPCInteract : MonoBehaviour
{
    public string npcName = "NPC";
    [TextArea] public string dialogueLine = "Hello, welcome to Talktown!";
    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (CompareTag("GroceryNPC"))
            {
                DialogueManager.Instance.ShowDialogue(npcName + ": What can I help you with today?");

                DialogueChoiceManager.Instance.ShowChoices(
                    "I want rice", () => DialogueManager.Instance.ShowDialogue("NPC: Sure, the rice is in aisle 2."),
                    "I want chips", () => DialogueManager.Instance.ShowDialogue("NPC: Sure, Chips are in the aisle 9."),
                    "I want water", () => DialogueManager.Instance.ShowDialogue("NPC: Sure, Water bottles are near the drinks section, aisle 8."),
                    "I want milk", () => DialogueManager.Instance.ShowDialogue("NPC: Sure, Milk is in the refrigerated section, aisle 6.")
                );
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
            DialogueChoiceManager.Instance.HideChoices();
        }
    }
}

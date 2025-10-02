using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WordOrderingManager : MonoBehaviour
{
    public Transform answerPanel;
    public GameObject wordButtonPrefab;
    public GameObject slotPrefab;
    public Transform scrambledPanel;
    public TextMeshProUGUI wrongText;
    public TextMeshProUGUI correctText;
    public NPCInteract groceryNPC;   // reference to the NPC

    private List<string> activeSentence;  // holds the current correct sentence

    // Generate a new scrambled challenge
    public void GenerateChallenge(List<string> sentence)
    {
        activeSentence = sentence; // store current correct sentence

        // Clear old UI
        foreach (Transform child in scrambledPanel) Destroy(child.gameObject);
        foreach (Transform child in answerPanel) Destroy(child.gameObject);

        // Shuffle words
        List<string> scrambled = new List<string>(sentence);
        for (int i = 0; i < scrambled.Count; i++)
        {
            string temp = scrambled[i];
            int rand = Random.Range(0, scrambled.Count);
            scrambled[i] = scrambled[rand];
            scrambled[rand] = temp;
        }

        // Create scrambled word buttons
        foreach (string word in scrambled)
        {
            GameObject button = Instantiate(wordButtonPrefab, scrambledPanel);
            button.GetComponentInChildren<TextMeshProUGUI>().text = word;
        }

        // Create answer slots
        for (int i = 0; i < sentence.Count; i++)
        {
            Instantiate(slotPrefab, answerPanel);
        }
    }

    // Called when the Check button is pressed
    public void CheckAnswer()
    {
        string playerSentence = "";
        Slot[] slots = answerPanel.GetComponentsInChildren<Slot>();

        foreach (Slot slot in slots)
        {
            TextMeshProUGUI slotText = slot.GetComponentInChildren<TextMeshProUGUI>();
            if (slotText != null && !string.IsNullOrWhiteSpace(slotText.text))
            {
                playerSentence += slotText.text + " ";
            }
        }

        playerSentence = playerSentence.Trim();
        string correctString = string.Join(" ", activeSentence);

        if (playerSentence.Equals(correctString, System.StringComparison.OrdinalIgnoreCase))
        {
            Debug.Log("✅ Correct: " + playerSentence);
            StartCoroutine(HandleCorrectAnswer(playerSentence));
        }
        else
        {
            Debug.Log("❌ Wrong: " + playerSentence + " | Correct is: " + correctString);
            StartCoroutine(ShowFeedback(wrongText));
        }
    }

    // Handles correct answers
    private IEnumerator HandleCorrectAnswer(string playerSentence)
    {
        correctText.gameObject.SetActive(true);

        // Ask NPC for the correct response
        string npcResponse = groceryNPC.GetResponseForSentence(playerSentence);

        // Immediately close the puzzle UI
        groceryNPC.CloseSentenceGame();

        // Show NPC dialogue
        DialogueManager.Instance.ShowDialogue(groceryNPC.npcName + ": " + npcResponse);


        // Wait for 2 seconds before prompting item collection
        yield return new WaitForSeconds(2f);

        //string requiredItem = groceryNPC.sentenceResponses[groceryNPC.currentSentenceIndex].requiredItemName;

        //Prompt player to find item
       // DialogueManager.Instance.ShowDialogue("Now go find a " + requiredItem + " and press E to pick it up!");


        // Advance to the next sentence in NPC
        groceryNPC.NextSentence();

        yield return new WaitForSeconds(3f);
        correctText.gameObject.SetActive(false);
    }

    // Handles wrong feedback
    private IEnumerator ShowFeedback(TextMeshProUGUI feedbackText)
    {
        feedbackText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        feedbackText.gameObject.SetActive(false);
    }
}

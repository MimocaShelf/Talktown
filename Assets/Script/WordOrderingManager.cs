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

    //Generate a new scrambled challenge
    public void GenerateChallenge(List<string> sentence)
    {
        activeSentence = sentence; // store current correct sentence

        //clear old UI
        foreach (Transform child in scrambledPanel) Destroy(child.gameObject);
        foreach (Transform child in answerPanel) Destroy(child.gameObject);

        //Shuffle words
        List<string> scrambled = new List<string>(sentence);
        for (int i = 0; i < scrambled.Count; i++)
        {
            string temp = scrambled[i];
            int rand = Random.Range(0, scrambled.Count);
            scrambled[i] = scrambled[rand];
            scrambled[rand] = temp;
        }

        foreach (string word in scrambled)
        {
            GameObject button = Instantiate(wordButtonPrefab, scrambledPanel);
            button.GetComponentInChildren<TextMeshProUGUI>().text = word;
        }

        for (int i = 0; i < sentence.Count; i++)
        {
            Instantiate(slotPrefab, answerPanel);
        }
    }

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
            StartCoroutine(HandleCorrectAnswer(playerSentence));
        }
        else
        {
            StartCoroutine(ShowFeedback(wrongText));
        }
    }

    // Handles correct answers
    private IEnumerator HandleCorrectAnswer(string playerSentence)
    {
        correctText.gameObject.SetActive(true);

        string npcResponse = groceryNPC.GetResponseForSentence(playerSentence);
        groceryNPC.CloseSentenceGame();
        DialogueManager.Instance.ShowDialogue(groceryNPC.npcName + ": " + npcResponse);
        groceryNPC.OnSentenceComplete(playerSentence);
        groceryNPC.CloseSentenceGame();

        yield return new WaitForSeconds(2f);
        correctText.gameObject.SetActive(false);
    }

    // Handles wrong feedback
    private IEnumerator ShowFeedback(TextMeshProUGUI feedbackText)
    {
        feedbackText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        feedbackText.gameObject.SetActive(false);
    }

    private ItemType MapSentenceToItem(string sentence)
    {
        string s = sentence.ToLowerInvariant();
        if (s.Contains("apple")) return ItemType.Apples;
        if (s.Contains("bread")) return ItemType.Milk;
        if (s.Contains("cereal")) return ItemType.Chips;
        if (s.Contains("water")) return ItemType.Water;
        if (s.Contains("chicken")) return ItemType.Chicken;
        return ItemType.None;
    }

    public void HandleCorrectSentence(string playerSentence)
    {
        Debug.Log($"Correct sentence completed: {playerSentence}");

        // Example matching logic:
        if (playerSentence.Contains("apple"))
            groceryNPC.requestedItem = ItemType.Apples;
        else if (playerSentence.Contains("milk"))
            groceryNPC.requestedItem = ItemType.Milk;
        else if (playerSentence.Contains("bread"))
            groceryNPC.requestedItem = ItemType.Chips;

        groceryNPC.CloseSentenceGame();
    }
}

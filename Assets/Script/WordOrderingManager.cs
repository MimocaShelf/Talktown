using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class WordOrderingManager : MonoBehaviour
{
    public Transform answerPanel;
    public GameObject wordButtonPrefab;
    public GameObject slotPrefab;
    public Transform scrambledPanel;
    public TextMeshProUGUI wrongText;
    public TextMeshProUGUI correctText;
    public NPCInteract groceryNPC;

    private List<string> correctSentence = new List<string> { "I", "would", "like", "to", "buy", "milk" };

    void Start()
    {
    }

    public void GenerateChallenge(List<string> sentence)
    {
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
        string correctString = string.Join(" ", correctSentence);

        if (playerSentence.Equals(correctString, System.StringComparison.OrdinalIgnoreCase))
        {
            Debug.Log("✅ Correct: " + playerSentence);
            StartCoroutine(HandleCorrectAnswer());
        }
        else
        {
            Debug.Log("❌ Wrong: " + playerSentence + " | Correct is: " + correctString);
            StartCoroutine(ShowFeedback(wrongText));
        }
    }

    private IEnumerator HandleCorrectAnswer()
    {
        correctText.gameObject.SetActive(true);

        // Immediately close the puzzle UI
        groceryNPC.CloseSentenceGame();

        // Wait 3 seconds before hiding the feedback
        yield return new WaitForSeconds(3f);
        correctText.gameObject.SetActive(false);
    }

    private IEnumerator ShowFeedback(TextMeshProUGUI feedbackText)
    {
        feedbackText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        feedbackText.gameObject.SetActive(false);
    }
}

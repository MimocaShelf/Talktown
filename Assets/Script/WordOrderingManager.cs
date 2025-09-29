using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WordOrderingManager : MonoBehaviour
{
    public Slot[] slots;
    public Transform answerPanel;
    public GameObject wordButtonPrefab;
    public GameObject slotPrefab;
    public Transform scrambledPanel;

    private List<string> correctSentence = new List<string> { "I", "would", "Like", "to", "buy", "milk" };

    void Start()
    {
        GenerateChallenge(correctSentence);
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

        // Get all slots under the AnswerPanel
        Slot[] slots = answerPanel.GetComponentsInChildren<Slot>();

        foreach (Slot slot in slots)
        {
            // Each slot has its own TMP text child
            TextMeshProUGUI slotText = slot.GetComponentInChildren<TextMeshProUGUI>();

            if (slotText != null && !string.IsNullOrWhiteSpace(slotText.text))
            {
                Debug.Log($"[CHECK] Slot {slot.name} contains: {slotText.text}");
                playerSentence += slotText.text + " ";
            }
            else
            {
                Debug.Log($"[CHECK] Slot {slot.name} is empty.");
            }
        }

        playerSentence = playerSentence.Trim();
        string correctString = string.Join(" ", correctSentence);

        if (playerSentence == correctString)
            Debug.Log("✅ Correct: " + playerSentence);
        else
            Debug.Log("❌ Wrong. You said: " + playerSentence + " | Correct is: " + correctString);
    }

}

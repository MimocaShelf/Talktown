using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WordOrderingManager : MonoBehaviour
{
    public GameObject wordButtonPrefab;
    public GameObject slotPrefab;
    public Transform scrambledPanel;
    public Transform answerPanel;

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
        List<string> playerSentence = new List<string>();

        foreach (Transform slot in answerPanel)
        {
            if (slot.childCount > 0)
                playerSentence.Add(slot.GetComponentInChildren<TextMeshProUGUI>().text);
        }

        string result = string.Join(" ", playerSentence);
        string correct = string.Join(" ", correctSentence);

        if (result == correct)
        {
            Debug.Log("✅ Correct! +10 points");
        }
        else
        {
            Debug.Log("❌ Wrong order. Try again!");
        }
    }
}

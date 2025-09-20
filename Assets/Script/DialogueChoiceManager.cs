using TMPro;
using UnityEngine;

public class DialogueChoiceManager : MonoBehaviour
{
    public static DialogueChoiceManager Instance;

    public GameObject choicePanel;
    public TextMeshProUGUI topText, leftText, rightText, bottomText;

    private System.Action topAction, leftAction, rightAction, bottomAction;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void ShowChoices(
        string top, System.Action topAct,
        string left, System.Action leftAct,
        string right, System.Action rightAct,
        string bottom, System.Action bottomAct)
    {
        choicePanel.SetActive(true);

        topText.text = top;
        leftText.text = left;
        rightText.text = right;
        bottomText.text = bottom;

        topAction = topAct;
        leftAction = leftAct;
        rightAction = rightAct;
        bottomAction = bottomAct;
    }

    void Update()
    {
        if (!choicePanel.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.W)) { topAction?.Invoke(); HideChoices(); }
        if (Input.GetKeyDown(KeyCode.A)) { leftAction?.Invoke(); HideChoices(); }
        if (Input.GetKeyDown(KeyCode.D)) { rightAction?.Invoke(); HideChoices(); }
        if (Input.GetKeyDown(KeyCode.S)) { bottomAction?.Invoke(); HideChoices(); }
    }

    public void HideChoices()
    {
        choicePanel.SetActive(false);
    }
}

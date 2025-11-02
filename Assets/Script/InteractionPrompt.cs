using UnityEngine;
using TMPro;

public class InteractionPrompt : MonoBehaviour
{

    public static InteractionPrompt Instance;
    [SerializeField] private CanvasGroup group;
    [SerializeField] private TextMeshProUGUI label;

    private void Awake() => Instance = this;

    public void Show(string text)
    {
        if (label) label.text = text;
        if (group) { group.alpha = 1; group.interactable = false;}
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        if (group) group.alpha = 0;
        gameObject.SetActive(false);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI pointsPopupText;

    private long totalScore = 0;
    public long lastAddedPoints = 0;

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        UpdateUI();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Total Score: " + totalScore;
    }
    public void AddPoints(long amount)
    {
        lastAddedPoints = amount;
        totalScore += amount;

        UpdateUI();

        if (pointsPopupText != null)
        {
            pointsPopupText.gameObject.SetActive(true);
            pointsPopupText.text = "+" + amount;
        }
    }

}

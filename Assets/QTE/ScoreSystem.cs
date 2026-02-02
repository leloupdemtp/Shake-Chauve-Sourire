using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    public int score;
    public TextMeshProUGUI scoreText;

    private void Start()
    {
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }
}
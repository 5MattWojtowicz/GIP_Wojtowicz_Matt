using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public TMPro.TextMeshProUGUI scoreText;
    public static ScoreSystem instance;
    public int score = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // <-- score blijft bestaan tussen scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddPoints(int amount)
    {
        score += amount;

        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    public void ResetScore()
    {
        score = 0;

        if (scoreText != null)
            scoreText.text = score.ToString();
    }
}

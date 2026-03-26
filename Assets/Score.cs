using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem instance;
    public int score = 0;

    void Awake()
    {
        instance = this;
    }

    public void AddPoints(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);
    }
}

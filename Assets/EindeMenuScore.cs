using UnityEngine;
using TMPro;

public class EindMenuScore : MonoBehaviour
{

    public TextMeshProUGUI scoreText;

    void Start()
    {
        scoreText.text = ScoreSystem.instance.score.ToString();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}

using UnityEngine;

public class Killscript : MonoBehaviour
{
    public Transform capsule;

    Vector3 respawnPos = new Vector3(-0.139492f, 1.419994f, 1.965019f);

    private PlayerMovement pm;

    private void Start()
    {
        pm = GetComponentInParent<PlayerMovement>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("KillPart"))
        {
            // Speler terug naar respawn
            transform.position = respawnPos;

            // Score resetten
            ScoreSystem.instance.score = 0;
            ScoreSystem.instance.ResetScore();

            // Alle targets opnieuw zichtbaar maken
            TargetManager.instance.ResetAllTargets();

            // Debug
            Debug.Log("Player died: resetting score and targets.");
        }
    }
}
using UnityEngine;

public class Target : MonoBehaviour
{
    public int points = 10;
    public GameObject confettiPrefab;
    public GameObject floatingPointsPrefab;

    public void Hit(float damage)
    {
        // Confetti effect
        Instantiate(confettiPrefab, transform.position, Quaternion.identity);

        // Floating points
        GameObject fp = Instantiate(floatingPointsPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        fp.GetComponent<FloatingPoints>().SetPoints(points);

        // Score toevoegen
        ScoreSystem.instance.AddPoints(points);

        // Target onzichtbaar maken i.p.v. vernietigen
        gameObject.SetActive(false);
        Debug.Log("Hiding target: " + gameObject.name);
    }

    public void ResetTarget()
    {
        // Target opnieuw zichtbaar maken
        gameObject.SetActive(true);
    }
}
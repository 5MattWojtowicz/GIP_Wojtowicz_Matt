using UnityEngine;

public class Target : MonoBehaviour
{
    public int points = 10;
    public GameObject confettiPrefab;
    public GameObject floatingPointsPrefab;

    public void Hit(float damage)
    {

        Instantiate(confettiPrefab, transform.position, Quaternion.identity);

        GameObject fp = Instantiate(floatingPointsPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        fp.GetComponent<FloatingPoints>().SetPoints(points);

        ScoreSystem.instance.AddPoints(points);

        Destroy(transform.root.gameObject);
        Debug.Log("Destroying target: " + gameObject.name);
    }
}
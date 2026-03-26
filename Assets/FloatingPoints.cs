using UnityEngine;
using TMPro;

public class FloatingPoints : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float lifeTime = 1f;
    public TextMeshPro text;

    Transform player;

    void Start()
    {
        player = Camera.main.transform;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.LookAt(player);
        transform.Rotate(0, 180, 0);
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
    }

    public void SetPoints(int amount)
    {
        text.text = "+" + amount;
    }
}
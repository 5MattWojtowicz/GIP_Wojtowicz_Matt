using UnityEngine;
using UnityEngine.SceneManagement;

public class EindeTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Speler"))
        {
            SceneManager.LoadScene("eind_menu");
        }
    }
}


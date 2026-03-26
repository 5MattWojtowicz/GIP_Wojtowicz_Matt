using UnityEngine;

public class Gun : MonoBehaviour
{

    public float range = 100f;
    public float damage = 1f;

    public Camera cam;
    public Transform weaponHolder;

    void Update()
    {
        weaponHolder.rotation = cam.transform.rotation;

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {

        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            Target target = hit.collider.GetComponent<Target>();
            if (target != null)
            {
                target.Hit(damage);
            }
        }
    }
}

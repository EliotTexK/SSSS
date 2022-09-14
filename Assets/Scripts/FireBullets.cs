using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullets : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float firingRadius = 4f;
    // Update is called once per frame
    void Update()
    {
        //if (!GameManager.Instance.updatePhysics)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = Input.mousePosition;
                Plane xy = new Plane(Vector3.zero, Vector3.up, Vector3.right);
                Ray ray = Camera.main.ScreenPointToRay(mousePos);
                float distance;
                xy.Raycast(ray, out distance);
                Vector2 coords2d = ray.GetPoint(distance);
                Vector2 aimDir = (coords2d - (Vector2)transform.position).normalized;
                GameObject myBullet = GameObject.Instantiate(bulletPrefab,
                    (Vector2) transform.position + firingRadius * aimDir, Quaternion.identity);
                myBullet.GetComponent<NewtonianPhysics>().velocity = aimDir * 0.25f;
                GameManager.Instance.updatePhysics = true;
            }
        }
    }
}

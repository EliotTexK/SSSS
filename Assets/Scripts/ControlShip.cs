using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NewtonianPhysics))]
[RequireComponent(typeof(OrbitTarget))]
[RequireComponent(typeof(DrawCircle))]
public class ControlShip : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float firingRadius = 4f;
    public float bulletSpeed = 1f;
    public float moveSpeed = 0.3f;
    public bool isHumanShip;
    private DrawCircle myCircle;
    private NewtonianPhysics myPhysics;
    private Vector2 moveTo;
    // is the ship actively moving?
    private bool isMoving = false;

    void Start()
    {
        myCircle = GetComponent<DrawCircle>();
        myPhysics = GetComponent<NewtonianPhysics>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.updatePhysics && isHumanShip == GameManager.Instance.isHumanTurn)
        {
            isMoving = false;
            GetComponent<OrbitTarget>().enabled = true;
            GetComponent<OrbitTarget>().applyInitialForce();
            myCircle.radius = firingRadius;
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
                    (Vector2)transform.position + firingRadius * aimDir, Quaternion.identity);
                myBullet.GetComponent<NewtonianPhysics>().velocity = aimDir * bulletSpeed;
                GameManager.Instance.updatePhysics = true;
                myCircle.radius = 0f;
                return;
            }
            if (Input.GetMouseButtonDown(1))
            {
                Vector2 mousePos = Input.mousePosition;
                Plane xy = new Plane(Vector3.zero, Vector3.up, Vector3.right);
                Ray ray = Camera.main.ScreenPointToRay(mousePos);
                float distance;
                xy.Raycast(ray, out distance);
                Vector2 coords2d = ray.GetPoint(distance);
                moveTo = coords2d;
                isMoving = true;
                GetComponent<OrbitTarget>().enabled = false;
                GameManager.Instance.updatePhysics = true;
                myCircle.radius = 0f;
                return;
            }
        }
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.updatePhysics)
        {
            Vector2 difference = moveTo - (Vector2)transform.position;
            if (isMoving)
            {
                myPhysics.velocity = difference.normalized * moveSpeed;
                if (difference.magnitude < 2f)
                {
                    isMoving = false;
                    GetComponent<OrbitTarget>().enabled = true;
                    GetComponent<OrbitTarget>().applyInitialForce();
                }
            }
        }
    }
}

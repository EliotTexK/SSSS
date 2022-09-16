using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NewtonianPhysics))]
[RequireComponent(typeof(OrbitTarget))]
[RequireComponent(typeof(DrawCircle))]
public class ControlMothership : ControlUnit
{
    public GameObject bulletPrefab;
    public float firingRadius = 4f;
    public float bulletSpeed = 1f;
    public float moveSpeed = 0.3f;
    private NewtonianPhysics myPhysics;
    private Vector2 moveTo;
    private bool isMoving = false;

    void Start()
    {
        myPhysics = GetComponent<NewtonianPhysics>();
    }   
    protected override void reactToMouseEvent(int input) {
        switch (input) {
            case 0:
            {
                Vector2 coords2d = getCoordsFromMouse();
                Vector2 aimDir = (coords2d - (Vector2)transform.position).normalized;
                GameObject myBullet = GameObject.Instantiate(bulletPrefab,
                    (Vector2)transform.position + firingRadius * aimDir, Quaternion.identity);
                myBullet.GetComponent<NewtonianPhysics>().velocity = aimDir * bulletSpeed;
                break;
            }
            case 1:
            {
                Vector2 coords2d = getCoordsFromMouse();
                moveTo = coords2d;
                isMoving = true;
                GetComponent<OrbitTarget>().enabled = false;
                break;
            }
            default:
            {
                Debug.Log("what?!?");
                break;
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

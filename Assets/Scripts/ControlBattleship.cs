using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NewtonianPhysics))]
[RequireComponent(typeof(OrbitTarget))]
[RequireComponent(typeof(Collider2D))]
public class ControlBattleship : ControlUnit
{
    public GameObject bulletPrefab;
    private Collider2D myCollider;
    public float firingRadius = 4f;
    public float bulletSpeed = 1f;
    public float moveSpeed = 0.3f;
    public float detectionRange = 30f;
    public float firingTime = 2f;
    private float firingTimer;
    private NewtonianPhysics myPhysics;
    private Vector2 moveTo;
    private bool isMoving = false;

    void Start()
    {
        myPhysics = GetComponent<NewtonianPhysics>();
        myCollider = GetComponent<Collider2D>();
        firingTimer = firingTime;
    }   
    public override void performAction(int input) {
        switch (input) {
            case 0:
            {
                break;     // do nothing, end turn
            }
            case 1:
            {
                Vector2 coords2d = getCoordsFromMouse();
                moveTo = coords2d;
                isMoving = true;
                GetComponent<OrbitTarget>().enabled = false;
                break;
            }
        }
        endTurn();
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
            if (firingTimer <= 0) {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,detectionRange);
                if (firingTimer <= 0f) {
                    foreach (Collider2D collider in colliders) {
                        if (collider && collider != myCollider && collider.tag != "Bullet") {
                            ControlUnit other = collider.gameObject.GetComponent<ControlUnit>();
                            if (other && other.isHumanUnit != this.isHumanUnit) {
                                Vector2 aimDir = ((Vector2)other.transform.position - (Vector2)transform.position).normalized;
                                GameObject myBullet = GameObject.Instantiate(bulletPrefab,
                                    (Vector2)transform.position + firingRadius * aimDir, Quaternion.identity);
                                myBullet.GetComponent<NewtonianPhysics>().velocity = aimDir * bulletSpeed;
                                break;  // only fire at one target!
                            }
                        }
                    }
                    firingTimer = firingTime;
                }
            }
            firingTimer-=Time.fixedDeltaTime;
        }
    }

    void OnGUI() {
        // code for drawing a symbol at "isMoving" goes here
    }

    public override void onPortal()
    {
        isMoving = false;
        GetComponent<OrbitTarget>().enabled = true;
        GetComponent<OrbitTarget>().applyInitialForce();
    }
    public override string getControlText()
    {
        return "left-click: skip turn, right-click: move unit";
    }
}

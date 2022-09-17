using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class NewtonianPhysics : MonoBehaviour
{
    public float mass;
    public Vector2 velocity;
    public static float gravConstant = 5f;
    // not used in OrbitTarget
    public float personalGravConstant = 0f;
    public float damageMultiplier = 1000f;
    public float detectionRange = 30f;
    private Collider2D myCollider;
    private Portal lastPortal = null;
    void Start()
    {
        myCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.Instance.updatePhysics) {
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,detectionRange);
                foreach (Collider2D collider in colliders) {
                    if (collider && collider != myCollider && collider.tag != "Bullet") {
                        NewtonianPhysics other = collider.gameObject.GetComponent<NewtonianPhysics>();
                        if (other) {
                            // apply gravitational acceleration
                            Vector2 targetDir = (other.transform.position - transform.position);
                            float distance = targetDir.magnitude;
                            float gravScalar = personalGravConstant * mass * other.mass / (distance * distance);
                            Vector2 gravAccel = targetDir.normalized * gravScalar;
                            velocity += gravAccel * Time.fixedDeltaTime;
                        }
                    }
                }
            }
            {
                CircleCollider2D[] collisions = new CircleCollider2D[1];
                myCollider.OverlapCollider(new ContactFilter2D(), collisions);
                if (collisions[0]) {
                        Portal otherPortal = collisions[0].gameObject.GetComponent<Portal>();
                        if (otherPortal) {
                            if (otherPortal == lastPortal) {
                                CancelInvoke();
                                Invoke("resetLastPortal",0.5f);
                            } else {
                                lastPortal = otherPortal.outPortal;
                                transform.position = otherPortal.outPortal.transform.position;
                                var unit = GetComponent<ControlUnit>();
                                if (unit) {
                                    unit.onPortal();
                                }
                            }
                        }
                    }
            }
            transform.Translate(velocity);
        }
    }
    void resetLastPortal() {
        lastPortal = null;
    }
}

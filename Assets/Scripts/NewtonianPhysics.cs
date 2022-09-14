using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class NewtonianPhysics : MonoBehaviour
{
    public float mass;
    public Vector2 velocity;
    public static float gravConstant = 20f;
    // not used in OrbitTarget
    public float personalGravConstant = 0f;
    public float damageMultiplier = 1000f;
    public float detectionRange = 30f;
    private Collider2D myCollider;
    void Start()
    {
        myCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.Instance.updatePhysics) {
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
            transform.Translate(velocity);
        }
    }
}

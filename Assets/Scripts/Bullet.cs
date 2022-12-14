using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(NewtonianPhysics))]
public class Bullet : MonoBehaviour
{
    private Collider2D myCollider;
    private NewtonianPhysics myPhysics;
    public float knockback = 0.2f;
    void Start()
    {
        myCollider = GetComponent<Collider2D>();
        myPhysics = GetComponent<NewtonianPhysics>();
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.updatePhysics)
        {
            // collide with other objects, instantly destroy the bullet
            CircleCollider2D[] collisions = new CircleCollider2D[1];
            myCollider.OverlapCollider(new ContactFilter2D(), collisions);
            if (collisions[0])
            {
                if (collisions[0].tag == "Sun") {
                    Destroy(this.gameObject);
                    return;
                }
                NewtonianPhysics otherPhysics = collisions[0].GetComponent<NewtonianPhysics>();
                if (otherPhysics) {
                    otherPhysics.velocity += knockback * myPhysics.velocity * myPhysics.mass / otherPhysics.mass;
                }
                Destructible otherDest = collisions[0].GetComponent<Destructible>();
                if (otherDest) {
                    otherDest.subtractHealth(myPhysics.mass * myPhysics.damageMultiplier);
                }
                myPhysics.velocity = Vector2.zero;
                Destroy(this.gameObject);
            }
            if (transform.position.magnitude > GameManager.Instance.maxDistance * 2f) {
                Destroy(this.gameObject, 1.5f);
            }
        }
    }
}

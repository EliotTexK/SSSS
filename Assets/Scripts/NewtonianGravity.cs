using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class NewtonianGravity : MonoBehaviour
{
    // mass is built into rigidBody2D
    const float gravConstant = 20f;
    public Rigidbody2D myRigidBody2D;
    public Collider2D myCollider2D;
    private float detectionRadius;
    // Start is called before the first frame update
    void Start() {
        detectionRadius = myRigidBody2D.mass * gravConstant * 2f;
    }

    void Update() {
        // TODO: make a collision layer for newtonian objects
        // finds all colliders in our radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        // for each collider in our radius, gravitate toward it
        foreach (var collider in colliders) {
            // ignores self-collision
            if (collider.transform == transform) continue;
            Vector2 vectorDifference = (collider.transform.position - transform.position);
            float distance = vectorDifference.magnitude;
            float gravForce = gravConstant * (myRigidBody2D.mass * collider.attachedRigidbody.mass) / (distance * distance);
            myRigidBody2D.AddForce(vectorDifference.normalized * gravForce * Time.deltaTime);
        }
    }
}

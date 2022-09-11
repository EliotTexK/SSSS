using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// gravitates toward other objects by the inverse square law
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class NewtonianGravity : MonoBehaviour
{
    // mass is built into rigidBody2D
    const float gravConstant = 20f;
    private Rigidbody2D myRigidBody2D;
    private Collider2D myCollider2D;
    private float detectionRadius;
    // Start is called before the first frame update
    void Start() {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        myCollider2D = GetComponent<Collider2D>();
        detectionRadius = gravConstant * 0.2f;
    }

    void Update() {
        // finds all colliders in our radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        // for each collider in our radius, gravitate toward it
        foreach (var collider in colliders) {
            // ignores self-collision
            if (collider.transform == transform) continue;
            Vector2 vectorDifference = (collider.transform.position - transform.position);
            float distance = vectorDifference.magnitude;
            float gravForce = gravConstant * (myRigidBody2D.mass * collider.attachedRigidbody.mass) / (distance * Mathf.Sqrt(distance));
            myRigidBody2D.AddForce(vectorDifference.normalized * gravForce * Time.deltaTime);
        }
    }

    void OnDrawGizmos() {
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, detectionRadius);
    }
}

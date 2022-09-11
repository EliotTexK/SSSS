using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// adjusts orbit to be circular
[RequireComponent(typeof(Rigidbody2D))]
public class AdjustToCircularOrbit : MonoBehaviour
{
    const float gravConstant = 20f;
    public Collider2D myStar;
    private float desiredRadius;
    Rigidbody2D myRigidBody2D;
    // Start is called before the first frame update
    void Start() {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        // desired radius is our current distance
        Vector2 vectorDifference = (myStar.transform.position - transform.position);
        float distance = vectorDifference.magnitude;
        float gravForce = gravConstant * (myRigidBody2D.mass * myStar.attachedRigidbody.mass) / (distance * distance);
        Vector2 grav = (vectorDifference.normalized * gravForce);
        Vector2 tangent = new Vector2(grav.y, -grav.x);
        myRigidBody2D.velocity = tangent;
    }

    // Update is called once per frame
    void Update() {
        
    }
}

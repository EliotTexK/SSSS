using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    public Rigidbody2D myRigidBody2D;
    void Update()
    {
        float forceX = Input.GetAxisRaw("Horizontal") * Time.deltaTime * 4000f;
        float forceY = Input.GetAxisRaw("Vertical") * Time.deltaTime * 4000f;

        myRigidBody2D.AddForce(new Vector2(forceX, forceY));
    }
}

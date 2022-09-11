using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Control : MonoBehaviour
{
    private Rigidbody2D myRigidBody2D;
    public float controlIntensity = 1000f;
    void Start() {
        myRigidBody2D = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        float forceX = Input.GetAxisRaw("Horizontal") * Time.deltaTime * controlIntensity;
        float forceY = Input.GetAxisRaw("Vertical") * Time.deltaTime * controlIntensity;

        myRigidBody2D.AddForce(new Vector2(forceX, forceY));
    }
}

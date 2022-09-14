using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NewtonianPhysics))]
public class FaceVelocity : MonoBehaviour
{
    private NewtonianPhysics myPhysics;
    public Transform toRotate;
    void Start()
    {
        myPhysics = GetComponent<NewtonianPhysics>();
    }
    void FixedUpdate()
    {
        toRotate.rotation = Quaternion.LookRotation(myPhysics.velocity.normalized);
    }
}

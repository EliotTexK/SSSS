using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewtonianPhysics : MonoBehaviour
{
    public float mass;
    public Vector2 velocity;
    public const float gravConstant = 20f;
    // by what factor should gravity be applied to THIS object?
    public float personalGravConstant = gravConstant;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

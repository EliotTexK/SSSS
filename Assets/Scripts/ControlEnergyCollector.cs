using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NewtonianPhysics))]
[RequireComponent(typeof(OrbitTarget))]
[RequireComponent(typeof(Collider2D))]
public class ControlEnergyCollector : ControlUnit
{
    private NewtonianPhysics myPhysics;
    private Vector2 moveTo;
    private bool isMoving = false;
    public float moveSpeed = 0.3f;
    public float collectionEfficiency = 2f;
    void Start() {
        myPhysics = GetComponent<NewtonianPhysics>();
    }
    protected override void reactToMouseEvent(int input)
    {
            switch (input) {
            case 0:
            {
                break;  // do nothing
            }
            case 1:
            {
                Vector2 coords2d = getCoordsFromMouse();
                moveTo = coords2d;
                isMoving = true;
                GetComponent<OrbitTarget>().enabled = false;
                break;
            }
            default:
            {
                Debug.Log("what?!?");
                break;
            }
        }
    }
    void FixedUpdate()
    {
        if (GameManager.Instance.updatePhysics)
        {
            Vector2 difference = moveTo - (Vector2)transform.position;
            if (isMoving)
            {
                myPhysics.velocity = difference.normalized * moveSpeed;
                if (difference.magnitude < 2f)
                {
                    isMoving = false;
                    GetComponent<OrbitTarget>().enabled = true;
                    GetComponent<OrbitTarget>().applyInitialForce();
                }
            }
            if (isHumanUnit) {
                GameManager.Instance.humanEnergy += Time.fixedDeltaTime * collectionEfficiency;
            } else {
                GameManager.Instance.alienEnergy += Time.fixedDeltaTime * collectionEfficiency;
            }
        }  
    }
    void OnGUI() {
        // code for drawing a symbol at "isMoving" goes here
    }

    public override void onPortal()
    {
        isMoving = false;
        GetComponent<OrbitTarget>().enabled = true;
        GetComponent<OrbitTarget>().applyInitialForce();
    }
}
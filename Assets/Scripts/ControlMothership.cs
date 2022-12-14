using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NewtonianPhysics))]
[RequireComponent(typeof(OrbitTarget))]
[RequireComponent(typeof(DrawCircle))]
public class ControlMothership : ControlUnit
{
    public GameObject bulletPrefab;
    public float firingRadius = 4f;
    public float summoningRadius;
    public float bulletSpeed = 1f;
    public float moveSpeed = 0.3f;
    private NewtonianPhysics myPhysics;
    private Vector2 moveTo;
    private bool isMoving = false;
    public List<GameObject> unitPrefabs;
    void Start()
    {
        GetComponent<DrawCircle>().radius = summoningRadius;
        myPhysics = GetComponent<NewtonianPhysics>();
    }
    public override void performAction(int input) {
        switch (input) {
            case 0:
            {
                Vector2 coords2d = getCoordsFromMouse();
                Vector2 aimDir = (coords2d - (Vector2)transform.position).normalized;
                GameObject myBullet = GameObject.Instantiate(bulletPrefab,
                    (Vector2)transform.position + firingRadius * aimDir, Quaternion.identity);
                myBullet.GetComponent<NewtonianPhysics>().velocity = aimDir * bulletSpeed;
                break;
            }
            case 1:
            {
                Vector2 coords2d = getCoordsFromMouse();
                moveTo = coords2d;
                isMoving = true;
                GetComponent<OrbitTarget>().enabled = false;
                break;
            }
            case 2:
            {
                break;
            }
        }
        endTurn();
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
            } else {
                if (isHumanUnit) {
                    GameManager.Instance.humanMoney += Time.fixedDeltaTime * 20f;
                } else {
                    GameManager.Instance.alienMoney += Time.fixedDeltaTime * 20f;
                }
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
    public void addUnit(int unitIndex, Vector2 location) {
        GameObject unit = GameObject.Instantiate(unitPrefabs[unitIndex], location, Quaternion.identity);
        unit.GetComponent<OrbitTarget>().target = GetComponent<OrbitTarget>().target;
        addToUnitChain(unit);
    }
    public override string getControlText()
    {
        return "left-click: shoot, right-click: move unit";
    }
    void OnDestroy() {
        if (isHumanUnit) {
            GameManager.Instance.gameOutcome = 1;
        } else {
            GameManager.Instance.gameOutcome = 2;
        }
    }
}
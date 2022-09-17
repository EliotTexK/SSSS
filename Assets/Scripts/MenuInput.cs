using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInput : MonoBehaviour
{
    // 0 - control units, 1 - place battleship, 2 - place enercy collector, 3 - place portal1, 4 - place portal2,
    // 5 - place blackhole, 6 - upgrade weapon
    [SerializeField]
    private int selectedAction = 0;

    public int SelectedAction { get => selectedAction; set => selectedAction = value; }
    public GameObject portalPrefab1, portalPrefab2, blackHolePrefab;
    private Portal linkPortal = null;

    void Update()
    {
        if (!GameManager.Instance.updatePhysics)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                selectedAction = 0;
                GameObject.Destroy(linkPortal);
                linkPortal = null;
            }
            if (Input.GetMouseButtonDown(0)) {
                Vector2 mouseCoords = getCoordsFromMouse();
                if (mouseCoords.magnitude < GameManager.Instance.maxDistance) {
                    switch (selectedAction) {
                        case 0: {
                            GameManager.Instance.controlledUnit.GetComponent<ControlUnit>().performAction(0);
                            selectedAction = 0;
                            break;
                        }
                        case 1: {
                            GameManager.Instance.summonUnit(0,mouseCoords);
                            selectedAction = 0;
                            break;
                        }
                        case 2: {
                            GameManager.Instance.summonUnit(1,mouseCoords);
                            selectedAction = 0;
                            break;
                        }
                        case 3: {
                            linkPortal = GameObject.Instantiate(portalPrefab1, mouseCoords, Quaternion.identity).GetComponent<Portal>();
                            selectedAction = 4;
                            break;
                        }
                        case 4: {
                            Portal linkPortal2 = GameObject.Instantiate(portalPrefab2, mouseCoords, Quaternion.identity).GetComponent<Portal>();
                            linkPortal2.outPortal = linkPortal;
                            linkPortal.outPortal = linkPortal2;
                            linkPortal = null;
                            selectedAction = 0;
                            break;
                        }
                        case 5: {
                            GameObject.Instantiate(blackHolePrefab, mouseCoords, Quaternion.identity);
                            selectedAction = 0;
                            break;
                        }
                        case 6: {
                            GameManager.Instance.upgradeWeapon();
                            selectedAction = 0;
                            break;
                        }
                    }
                }
            }
            if (Input.GetMouseButtonDown(1)) {
                Vector2 mouseCoords = getCoordsFromMouse();
                if (mouseCoords.magnitude < GameManager.Instance.maxDistance) {
                    switch (selectedAction) {
                        case 0: {
                            GameManager.Instance.controlledUnit.GetComponent<ControlUnit>().performAction(1);
                            break;
                        }
                    }
                }
            }
        }
    }
    private Vector2 getCoordsFromMouse()
    {
        Vector2 mousePos = Input.mousePosition;
        Plane xy = new Plane(Vector3.zero, Vector3.up, Vector3.right);
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }
    void OnGUI() {
        if (selectedAction == 0) {
            // draw some sort of indicator for what ship is being controlled
        }
    }
}

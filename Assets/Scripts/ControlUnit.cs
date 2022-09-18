using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControlUnit : MonoBehaviour
{
    public bool isHumanUnit;
    // units are implemented as a linked list
    private GameObject nextUnit = null;
    // Update is called once per frame
    public Canvas controlReticle;
    protected void endTurn()
    {
        if (nextUnit) {
            // still more units to process controls for
            // delay calling setNextUnit, so that the same mouse
            // input isn't used for the next unit
            Invoke("setNextUnit",0.1f);
            nextUnit.GetComponent<ControlUnit>().getControlText();
        }
        else {
            // finished with our turn
            GameManager.Instance.controlledUnit = null;
            GameManager.Instance.updatePhysics = true;
        }
        return;
    }    
    private void setNextUnit() {
        GameManager.Instance.controlledUnit = nextUnit;
    }
    protected Vector2 getCoordsFromMouse()
    {
        Vector2 mousePos = Input.mousePosition;
        Plane xy = new Plane(Vector3.zero, Vector3.up, Vector3.right);
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }
    public abstract void onPortal();
    // ID 0 = fire, ID 1 = move, ID 2 = do nothing and end turn
    public abstract void performAction(int actionID);
    public abstract string getControlText();
    // recursively traverse the linked list, adding a unit to the end
    protected void addToUnitChain(GameObject givenNextUnit)
    {
        if (nextUnit)
        {
            ControlUnit nextControl = nextUnit.GetComponent<ControlUnit>();
            if (nextControl) {
                nextControl.addToUnitChain(givenNextUnit);
            }
            else {
                Debug.Log("Next unit is not a valid unit! It has no ControlUnit script!");
            }
        }
        else {
            nextUnit = givenNextUnit;
        }
    }
    void onDestroy() {
        // don't get rid of the proceeding units on destroy!
        if (isHumanUnit) {
            GameManager.Instance.humanMothership.GetComponent<ControlUnit>().addToUnitChain(nextUnit);
        } else {
            GameManager.Instance.alienMothership.GetComponent<ControlUnit>().addToUnitChain(nextUnit);
        }
    }

    void Update() {
        if (GameManager.Instance.controlledUnit == this.gameObject) {
            controlReticle.gameObject.SetActive(true);
        } else {
            controlReticle.gameObject.SetActive(false);
        }
    }
}

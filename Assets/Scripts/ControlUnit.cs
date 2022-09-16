using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControlUnit : MonoBehaviour
{
    public bool isHumanUnit;
    // units are implemented as a linked list
    private GameObject nextUnit = null;
    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.updatePhysics && GameManager.Instance.controlledUnit == this.gameObject)
        {
            if (Input.GetMouseButtonDown(0)) {
                reactToMouseEvent(0);
                endTurn();
            }
            if (Input.GetMouseButtonDown(1)) {
                reactToMouseEvent(1);
                endTurn();
            }
        }
    }
    private void endTurn()
    {
        if (nextUnit) {
            // still more units to process controls for
            // delay calling setNextUnit, so that the same mouse
            // input isn't used for the next unit
            Invoke("setNextUnit",0.01f);
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
    void OnGUI()
    {
        if (!GameManager.Instance.updatePhysics && GameManager.Instance.controlledUnit == this.gameObject)
        {
            // code for indicating we are controlling THIS unit right now
        }
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
    protected abstract void reactToMouseEvent(int input);
    // recursively traverse the linked list, adding a unit to the end
    public void setNextUnit(GameObject givenNextUnit)
    {
        if (nextUnit)
        {
            ControlUnit nextControl = nextUnit.GetComponent<ControlUnit>();
            if (nextControl) {
                nextControl.setNextUnit(givenNextUnit);
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
            GameManager.Instance.humanMothership.GetComponent<ControlUnit>().setNextUnit(nextUnit);
        } else {
            GameManager.Instance.alienMothership.GetComponent<ControlUnit>().setNextUnit(nextUnit);
        }
    }
}

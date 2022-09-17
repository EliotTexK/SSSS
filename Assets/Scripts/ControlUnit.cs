using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControlUnit : MonoBehaviour
{
    public bool isHumanUnit;
    // units are implemented as a linked list
    private GameObject nextUnit = null;
    // Update is called once per frame
    public Texture2D controlArrow;
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
            GUI.DrawTexture(new Rect(transform.position.x-32,transform.position.y-32,64,64), controlArrow);
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
    public abstract void onPortal();
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

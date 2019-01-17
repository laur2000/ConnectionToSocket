using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    string player_id;
    Vector3 mousePos = new Vector3();
    private void OnMouseDown()
    {
        
        
    }
    public Vector3 GetPos()
    {
        return mousePos;
    }
    private void Update()
    {

        if (Input.GetMouseButton(0))
        {
            mousePos = Input.mousePosition;
            this.gameObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 5));
        }
    }
    public Vector3 GetMousePos()
    {
        return mousePos;
    }
}

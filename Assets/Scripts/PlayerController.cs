using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    string player_id;
    Vector3 mousePos = new Vector3();
    private void OnMouseDown()
    {
        mousePos = Input.mousePosition;
        
    }
    public Vector3 GetPos()
    {
        return mousePos;
    }
    public void SetPlayerId(string id)
    {
        player_id = id;
    }
}

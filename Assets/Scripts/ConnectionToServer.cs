using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;


public class ConnectionToServer : MonoBehaviour
{
    WebSocket ws;
	// Use this for initialization
	void Start () {
         ws= new WebSocket("ws://app-mobile-api.herokuapp.com");
        
            ws.OnOpen += (sender, e) => {
                Debug.Log("Connection established!");
            };
            ws.OnMessage += (sender, e) => {
                Debug.Log(e.Data);
            };
            ws.Connect();

            
        

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

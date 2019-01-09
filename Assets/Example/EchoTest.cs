using UnityEngine;
using System.Collections;
using System;

public class EchoTest : MonoBehaviour {

    // Use this for initialization
    WebSocket w;
    IEnumerator Start() {
        w = new WebSocket(new Uri("ws://app-mobile-api.herokuapp.com"));
        yield return StartCoroutine(w.Connect());
        Debug.Log(w.RecvString());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            w.SendString("Hi there");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            w.Close();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log(w.RecvString());
        }
        




    }
}

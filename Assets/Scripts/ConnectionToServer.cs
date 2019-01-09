using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.Networking;

public class ConnectionToServer : MonoBehaviour {

    string url = "https://app-mobile-api.herokuapp.com/";
    
    private void Start()
    {
        string msg = "Hey claudiu";
        using (UnityWebRequest www = UnityWebRequest.Post(url,msg))
        {
            Debug.Log(www.error);
        }
        
    }
}

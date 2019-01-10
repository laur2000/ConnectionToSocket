using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using UnityEngine.Networking;

public class JsonData
{
    public string id;
    public string name;
    public string color;

   public JsonData(string ID)
    {
        id = ID;
    }
    //Get the parametres of the JSON
   public string GetId()
    {
        return id;
    }
    public string GetName()
    {
        return id;
    }
    public string GetColor()
    {
        return id;
    }

    //Set the parametres of the JSON
    
    public void SetName(string NAME)
    {
        name = NAME;
    }
    public void SetColor(string COLOR)
    {
        color = COLOR;
    }
}
public class ConnectionToServer : MonoBehaviour
{
    WebSocket ws;
    string web_uri = "ws://app-mobile-api.herokuapp.com";
    JsonData player;
	// Use this for initialization
	public void StartConnection () {

         ws= new WebSocket(web_uri);
         bool is_first = true;
            ws.OnOpen += (sender, e) => {
                Debug.Log("Connection established!");
            };
            ws.OnMessage += (sender, e) => {
                if(is_first)
                {
                    player = new JsonData(e.Data.Substring(4));

                    
                    is_first = false;
                }
                Debug.Log(e.Data);
            };
            ws.Connect();
        //string data =JsonConvert.SerializeObject(new JsonData());

    }

    public void SetDataJson(string name,string color)
    {
        player.SetName(name);
        player.SetColor(color);
    }
    public void SendJsonData()
    {
        
        StartCoroutine("Post");

    }
    
   IEnumerator Post()
    {
        string data = JsonUtility.ToJson(player);
        Debug.Log("Trying to upload: " + data);
        using (UnityWebRequest www = UnityWebRequest.Post(web_uri, data))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }
    



}

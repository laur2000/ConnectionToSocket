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
        return name;
    }
    public string GetColor()
    {
        return color;
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
   public WWWForm JsonDataToForm()
    {
        WWWForm data = new WWWForm();
        data.AddField("id", id);
        data.AddField("name", name);
        data.AddField("color", color);

        return data;
    }
}
public class ConnectionToServer : MonoBehaviour
{
    WebSocket ws;
    string web_uri;
    string web_http="https://app-mobile-api.herokuapp.com/player";
    JsonData player;
	// Use this for initialization
   
	public void StartConnection (string uri) {
        web_uri = uri;
         ws= new WebSocket(web_uri);
         bool is_first = true;
            ws.OnOpen += (sender, e) => {
                Debug.Log("Connection established!");
            };
        
            ws.OnMessage += (sender, e) => {
                if(is_first)
                {
                    
                    player = new JsonData("");
                    JsonUtility.FromJsonOverwrite(e.Data, player);
                    Debug.Log(player.GetId());
                    
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

        WWWForm data = player.JsonDataToForm();
        
        using (UnityWebRequest www = UnityWebRequest.Post(web_http,data))
        {
            
           
            yield return www.SendWebRequest();
            
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                Debug.Log(player.GetId());
                Debug.Log(www.downloadHandler.text);
               
            }
            else
            {
                Debug.Log("Form upload complete!");
                
            }
        }
    }

    
    



}

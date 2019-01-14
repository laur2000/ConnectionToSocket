using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using UnityEngine.Networking;
using SimpleJSON;

public class PlayerData
{
    string id;
    string name;
    string color;

   public PlayerData(string ID)
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
    
    PlayerData[] players_data=new PlayerData[6];

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
                    var data = JSON.Parse(e.Data);

                    players_data[0] = new PlayerData(data["id"]);
                    
                    Debug.Log("Client id: "+players_data[0].GetId());
                    
                    is_first = false;
                }
                else
                {
                    var data = JSON.Parse(e.Data);
                    for(int i=0;i<players_data.Length;i++)
                    {
                        if (data["playersList"][i] != null)
                        {
                            Debug.Log(data["playersList"][i]);
                        }
                    }

                }
                
            };
            ws.Connect();
        //string data =JsonConvert.SerializeObject(new JsonData());

    }
    public void CloseConnection()
    {

        ws.Close();
        Debug.Log("Connection closed");
    }
    public void SetDataJson(string name,string color)
    {
        while(players_data[0] ==null)
        { }
        Debug.Log("Data updated");
        players_data[0].SetName(name);
        players_data[0].SetColor(color);
    }
    public void SendJsonData()
    {
        
        StartCoroutine("Post");

    }
    
   IEnumerator Post()
    {

        WWWForm data = players_data[0].JsonDataToForm();
        
        using (UnityWebRequest www = UnityWebRequest.Post(web_http,data))
        {
            
           
            yield return www.SendWebRequest();
            
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                Debug.Log(players_data[0].GetId());
                Debug.Log(www.downloadHandler.text);
               
            }
            else
            {
                Debug.Log("Form upload complete!");
                
            }
        }
    }

    
    



}

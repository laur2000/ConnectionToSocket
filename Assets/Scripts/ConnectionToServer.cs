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
    public int pos_x, pos_y;
    public bool isInstantiated = false;
    public bool change = true;
    
    
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
    //GameObject methods
   
  
    //Set the parametres of the JSON
    
    public void SetName(string NAME)
    {
        name = NAME;
    }
    public void SetColor(string COLOR)
    {
        color = COLOR;
    }
    public void SetId(string ID)
    {
        id=ID;
    }
    
    public void SetData(JSONNode data,int index)
    {
        if (change)
        {
            
            id=data["playersList"][index]["id"];
            name=data["playersList"][index]["name"];
            color=data["playersList"][index]["color"];
            
            change = false;
        }
        pos_x = data["playersList"][index]["posX"].AsInt;
        pos_y = data["playersList"][index]["posY"].AsInt;
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
    GameObject player_prefab;
    List<PlayerData> players_data=new List<PlayerData>();
    string player_id;
    
    // Use this for initialization
    public void SetPlayerPrefab(GameObject go)
    {
        player_prefab = go;
        
    }
	public void StartConnection (string uri) {
        
        web_uri = uri;
        ws= new WebSocket(web_uri);
        bool is_first = true;
        ws.OnOpen += (sender, e) => 
        {
            Debug.Log("Connection established!");                
        };
        
        ws.OnMessage += (sender, e) => 
        {
            if (is_first)
            {
                var data = JSON.Parse(e.Data);
                player_id = data["id"];
                Debug.Log("player id: " + player_id);

                is_first = false;
            }
            else
            {
                var data = JSON.Parse(e.Data);
                Debug.Log("Data recieved---- " + data.ToString());

                int index = CreatePlayerData(data);
                for (int i = 0; i < index; i++)
                {
                    players_data[i].SetData(data,i); 
                    
                }
            }

            };
            ws.Connect();
        

    }
    int CreatePlayerData(JSONNode data)
    {
        int index = 0;
        while (data["playersList"][index] != null)
        {

            index++;
        }
        int gen = index - players_data.Count;
        for (int i = 0; i < gen; i++)
        {
            Debug.Log("Added new player");
            players_data.Add(new PlayerData(""));
        }
        return index;
    }
    public void CloseConnection()
    {

        ws.Close();
        Debug.Log("Connection closed");
    }
    
    public void SetDataJson(string name,string color)
    {
        int index = FindPlayerIndex(player_id);
        
        players_data[index].SetName(name);
        players_data[index].SetColor(color);
    }
    public void SendJsonData()
    {
        
        StartCoroutine("Post");

    }
   public void SetGameObject(GameObject go)
    {
        player_prefab = go;
    }
    int FindPlayerIndex(string id)
    {
        
        for (int i = 0; i < players_data.Count; i++)
        {
            if (players_data[i].GetId() == player_id)
            {
                Debug.Log("Index: " + i + " for id: " + id);
                return i;
            }
        }
        return 0;
    }

   IEnumerator Post()
    {
        int index = FindPlayerIndex(player_id);
        WWWForm data = players_data[index].JsonDataToForm();
        
        using (UnityWebRequest www = UnityWebRequest.Post(web_http,data))
        {

            
            yield return www.SendWebRequest();
            
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                Debug.Log(players_data[index].GetId());
                Debug.Log(www.downloadHandler.text);
               
            }
            else
            {
                Debug.Log("Post complete!");
                
                
            }
        }
    }

    
    



}

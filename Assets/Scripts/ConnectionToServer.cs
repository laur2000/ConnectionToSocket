using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using UnityEngine.Networking;
using SimpleJSON;

public class PlayerData: MonoBehaviour
{
    string id;
    string _name;
    string color;
    public int pos_x, pos_y;
    public bool isInstantiated = false;
    public bool change = true;
    GameObject container;
    
    public GameObject GetGameObject()
    {
        return container;
    }
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
        return _name;
    }
    public string GetColor()
    {
        return color;
    }
    //GameObject methods
   public void SetPosition(Vector3 pos)
    {
        pos_x = (int)pos.x;
        pos_y = (int)pos.y;
    }
  public Vector3 GetPosition()
    {
        return new Vector3(pos_x, pos_y, 5);
    }
    //Set the parametres of the JSON
    public void SetPrefab(GameObject go,string main_id )
    {
        container = go;
        container.name = _name;
        container.GetComponent<SpriteRenderer>().color = ColorChanger.ToColor(color);
        if(main_id!=id)
        {
            Destroy(container.GetComponent<PlayerController>());
        }
        
    }
    public void SetName(string NAME)
    {
        _name = NAME;
    }
    public void SetColor(string COLOR)
    {
        color = COLOR;
    }
    public void SetId(string ID)
    {
        id=ID;
    }
    
    public void UpdatePos()
    {
        container.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(pos_x, Screen.height-pos_y, 5));
    }
    public void SetData(JSONNode data,int index)
    {
        if (data["playersList"][index]["name"] != _name || data["playersList"][index]["color"] != color)
        {
            change = true;
            
        }
        if (change)
        {
            
            id=data["playersList"][index]["id"];
            _name=data["playersList"][index]["name"];
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
        data.AddField("name", _name);
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
    public string GetMainPlayerId()
    {
        return player_id;
    }
    // Use this for initialization
    public List<PlayerData> GetPlayersData()
    {
        return players_data;
    }
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
    public void SendPlayerPosition(Vector3 pos)
    {
        JSONObject json = new JSONObject();
        int posx = (int)pos.x;
        int posy = (int)pos.y;
        json.Add("id", player_id);
        json.Add("posX", posx);
        json.Add("posY", posy);
        Debug.Log(json);
        ws.Send(json);
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

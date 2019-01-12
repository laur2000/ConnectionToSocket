using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour

{
    [SerializeField]
    Text nickname_text;
    string nickname;
    string color;
    GameObject manager;
    ConnectionToServer connection;
    public void SetNickname()
    {
        nickname = nickname_text.text;
    }
    public void SetColorRed()
    {
        color = ColorUtility.ToHtmlStringRGB(Color.red);
        color = "#" + color.ToLower();
    }
    public void SetColorBlue()
    {
        color = ColorUtility.ToHtmlStringRGB(Color.blue);
        color = "#" + color.ToLower();
    }
    public void SetColorYellow()
    {
        color = ColorUtility.ToHtmlStringRGB(Color.yellow);
        color = "#" + color.ToLower();
    }
    public void SetColorGreen()
    {
        color = ColorUtility.ToHtmlStringRGB(Color.green);
        color = "#" + color.ToLower();
    }
    public void StartGame()
    {

        connection.SetDataJson(nickname, color);
        connection.SendJsonData();
        

    }
    
    private void Awake()
    {
        manager = new GameObject();
        manager.name = "networkManager";
        manager.AddComponent<ConnectionToServer>();
        connection = manager.GetComponent<ConnectionToServer>();
        connection.StartConnection("ws://app-mobile-api.herokuapp.com");
    }

}

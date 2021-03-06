﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour

{
    [SerializeField]
    GameObject player_prefab;
    [SerializeField]
    Text nickname_text;
    [SerializeField]
    Slider r_slider;
    [SerializeField]
    Slider g_slider;
    [SerializeField]
    Slider b_slider;
    ColorChanger color_changer;
    string nickname;
   
    GameObject manager;
    ConnectionToServer connection;
    [SerializeField]
    Image img;
    public void SetNickname()
    {
        nickname = nickname_text.text;
    }
    public void SetColorRed()
    {
        color_changer.r=r_slider.value;
        color_changer.UpdateRGB();
    }
    public void SetColorBlue()
    {
        color_changer.b = b_slider.value;
        color_changer.UpdateRGB();
    }
    public void SetColorGreen()            
    {
            color_changer.g = g_slider.value;
        color_changer.UpdateRGB();

    }
    public void StartGame()
    {
        manager.AddComponent<ConnectionToServer>();
        connection = manager.GetComponent<ConnectionToServer>();
        connection.SetPlayerPrefab(player_prefab);
        connection.StartConnection("ws://app-mobile-api.herokuapp.com");
        
        
        
        DontDestroyOnLoad(manager);
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene(1);

        
        SceneManager.sceneLoaded += (sender, e) =>
        {
            Debug.Log("Scene loaded");
            connection.SetDataJson(nickname, color_changer.ToHexadecimal());
            
            connection.SendJsonData();
            playerDatas = connection.GetPlayersData();
            instantiate = true;

        };
    }
    bool instantiate = false;
    private void Awake()
    {
        manager = new GameObject();
        manager.name = "networkManager";
        
    }
    List<PlayerData> playerDatas;
    private void Start()
    {
        color_changer= new ColorChanger(img);
        r_slider.value = color_changer.r;
        g_slider.value = color_changer.g;
        b_slider.value = color_changer.b;
        
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            connection.CloseConnection();
        }
        if(instantiate)
        {
            foreach(PlayerData pd in playerDatas)
            {
                Debug.Log(pd.GetId());
                if (pd.isInstantiated == false)
                {
                   pd.SetPrefab(Instantiate(player_prefab));
                    pd.isInstantiated = true;
                }
                else
                {
                    pd.UpdatePos();
                }


            }

        }
    }
}

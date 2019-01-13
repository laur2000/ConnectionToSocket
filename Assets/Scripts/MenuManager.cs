using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour

{
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
        
        connection.SetDataJson(nickname, color_changer.ToHexadecimal());
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
    private void Start()
    {
        color_changer= new ColorChanger(img);
        r_slider.value = color_changer.r;
        g_slider.value = color_changer.g;
        b_slider.value = color_changer.b;
    }
}

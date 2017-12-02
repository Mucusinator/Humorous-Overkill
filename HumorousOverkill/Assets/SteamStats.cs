using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facepunch.Steamworks;
using UnityEngine.UI;

public class SteamStats : MonoBehaviour {

    private bool show = false;
    public Button SteamButton;
    public Text statText;
    private string userName;
    private string id;


    void Start()
    {
        Button btn = SteamButton.GetComponent<Button>();
        
        btn.onClick.AddListener(ShowStats);
    }

    void ShowStats()
    {
        userName = Client.Instance.Username.ToString();
        id = Client.Instance.SteamId.ToString();
        if (!show)
        {
            show = true;
            
            Display();
        }
        else
        {
            show = false;

            Display();
        }
    }



    void Display()
    {
        if (show)
        {
            statText.text = "Steam Username is:" + userName + "\n";
            statText.text += "Steam ID:" + id + "\n";

            Debug.Log(Client.Instance.Username);
           
        }
        else
        {
            statText.text = " ";
        }
    }
}

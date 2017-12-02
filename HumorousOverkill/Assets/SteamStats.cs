using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facepunch.Steamworks;
using UnityEngine.UI;

public class SteamStats : MonoBehaviour {

    private bool show = false;
    public Button SteamButton;
    public Text statText;

    void Start()
    {
        Button btn = SteamButton.GetComponent<Button>();
        btn.onClick.AddListener(ShowStats);
    }

    void ShowStats()
    {
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
            statText.text = Client.Instance.Username;
            Debug.Log(Client.Instance.Username);
           
        }
        else
        {
            statText.text = " ";
        }
    }
}

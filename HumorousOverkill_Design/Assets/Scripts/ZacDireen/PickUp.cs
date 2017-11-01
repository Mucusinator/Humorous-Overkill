using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {
    public PickUpType pickUpSelection;
    private PickUpType pickUpSelected;
    public Health playerHealth;

    public enum PickUpType
    {
        HEALTH,
        RIFLEAMMO,
        SHOTGUNAMMO
    }
	// Use this for initialization
	void Start () {

        pickUpSelected = pickUpSelection;
	}

    //// Update is called once per frame
    //void Update () {


    //}
    void OnTriggerEnter(Collider other)
    {
        GameObject player = GameObject.Find("Player");
        CombinedScript playersWeaponHolder = GameObject.FindObjectOfType<CombinedScript>();
        Health playersHealth = player.GetComponent<Health>();
        CombinedScript weapons = playersWeaponHolder.GetComponent<CombinedScript>(); 
        
        Destroy(gameObject);
        switch (pickUpSelected) 
        {
            case PickUpType.HEALTH:
                playersHealth.HealDamage(25);
                break;
            case PickUpType.RIFLEAMMO:
                weapons.maxRifleAmmo += 15;
                break;
            case PickUpType.SHOTGUNAMMO:
                weapons.maxShotgunAmmo += 8;
                break;
            default:
                break;
        }
    }
}

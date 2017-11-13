using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killEnemies : MonoBehaviour
{
    [System.Serializable]
    // weapon information
    public struct weaponInfo
    {
        int maxAmmo; // maximum ammo that can be held
        int currentAmmo; // current amount of ammo
        int clipSize; // clip size of the weapon

        bool automaticFire; // can the button be held to fire


    }

    // we have two weapons confetti and laser
	public enum WEAPONTYPE { CONFETTI, LASER }

    // the currently equipped weapon
    public WEAPONTYPE equippedWeapon = WEAPONTYPE.CONFETTI;
    private bool laserUnlocked = false;

    private float currentHealth;
    private float currentConfettiAmmo;
    private float currentLaserAmmo;

    void Start()
    {

    }

	void Update ()
    {
        //RaycastHit hitInfo;
        //Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        //Debug.DrawRay(ray.origin, ray.direction * 1000, Color.yellow);
        //if (Input.GetMouseButton(0))
        //{
        //    if (Physics.Raycast(ray, out hitInfo))
        //    {
        //        if (hitInfo.collider.gameObject.GetComponent<CupcakeAI>() != null)
        //        {
        //            hitInfo.collider.gameObject.GetComponent<CupcakeAI>().HandleEvent(GameEvent.ENEMY_DAMAGED, 1);
        //        }
        //        if (hitInfo.collider.gameObject.GetComponentInParent<DonutAI>() != null)
        //        {
        //            hitInfo.collider.gameObject.GetComponentInParent<DonutAI>().HandleEvent(GameEvent.ENEMY_DAMAGED, 1);
        //        }
        //    }
        //}

        if (equippedWeapon == WEAPONTYPE.CONFETTI)
        {
            confettiLogic();
        }
        else if(equippedWeapon == WEAPONTYPE.LASER)
        {
            laserLogic();
        }

        // swap weapons using right mouse button
        if(Input.GetMouseButtonDown(1))
        {
            swapWeapon();
        }
	}

    // swaps weapons taking into account if the laser has been unlocked
    void swapWeapon()
    {
        switch (equippedWeapon)
        {
            case WEAPONTYPE.CONFETTI:
            if(laserUnlocked)
            {
                    equippedWeapon = WEAPONTYPE.LASER;
            }
            break;
            case WEAPONTYPE.LASER:
            equippedWeapon = WEAPONTYPE.CONFETTI;
            break;
        }
    }

    // do confetti stuff
    void confettiLogic()
    {

    }

    // do laser stuff
    void laserLogic()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        // deal with various types of pickup
        if(other.gameObject.GetComponent<Pickup>() != null)
        {
            switch (other.gameObject.GetComponent<Pickup>().type)
            {
                case GameEvent.PICKUP_HEALTH:
                    currentHealth += other.gameObject.GetComponent<Pickup>().amount;
                    break;
                case GameEvent.PICKUP_SHOTGUNAMMO:
                    currentConfettiAmmo += other.gameObject.GetComponent<Pickup>().amount;
                    break;
                case GameEvent.PICKUP_RIFLEAMMO:
                    currentLaserAmmo += other.gameObject.GetComponent<Pickup>().amount;
                    break;
            }
        }
    }
}

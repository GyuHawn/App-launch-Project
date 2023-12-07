using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class PlayerTail : MonoBehaviour
{
    private PlayerMove playerMove;
    private SettingScript settingScript;

    void Start()
    {
        playerMove = GameObject.Find("Player").GetComponent<PlayerMove>();
        settingScript = GameObject.Find("Manager").GetComponent<SettingScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enermy"))
        {
            if (settingScript.gaming)
            {
                playerMove.OnTailCollision(gameObject);
            }
        }
    }
}

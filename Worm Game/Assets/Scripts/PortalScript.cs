using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour
{
    public GameObject portal1;
    public GameObject portal1Point;
    public GameObject portal2;
    public GameObject portal2Point;

    public float portalTime;

    private void Update()
    {
        if(portalTime > 0)
        {
            portalTime -= Time.deltaTime;
        }
    }

    public void UsePortal()
    {
        portalTime = 5f;
    }  
}

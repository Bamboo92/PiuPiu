using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public bool inCollider = false;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Building")
        {
            inCollider = true;
        }
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Building")
        {
            inCollider = true;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Building")
        {
            inCollider = false;
            //playerLife2.TakeDamage(20f);
        }
    }
}

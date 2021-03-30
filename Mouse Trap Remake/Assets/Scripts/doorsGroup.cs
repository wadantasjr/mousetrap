using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorsGroup : MonoBehaviour
{
    public GameObject[] doors = null;
    public bool isStateOn = true;

    // Update is called once per frame
    void Update()
    {
        Door d = null;
        foreach (GameObject g in doors)
        {
            if (g != null)
            {
                d = g.GetComponent<Door>();
                if (d != null)
                {
                    d.isStateOn = isStateOn;
                }
            }
        }
    }
}

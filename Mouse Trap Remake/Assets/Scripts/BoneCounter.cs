using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneCounter : MonoBehaviour
{
    public GameObject[] Bones;
    private int _count = 1;
    public int Count
    {
        set
        {
            _count = (value < 0 ? 0 : (value >= 6 ? 6 : value)); //Min: 0 bones  Max: 6 bones
        }
        get
        {
            return _count;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Bones.Length; i++)
        {
            Bones[i].SetActive(i <= (_count - 1));
        }
    }
}

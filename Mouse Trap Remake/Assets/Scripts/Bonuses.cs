using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonuses : MonoBehaviour
{
    public GameObject[] bonuses = null;
    private int _count = 0;
    public int Count
    {
        set
        {
            _count = (value < 0 ? 0 : (value >= bonuses.Length ? 0 : value)); 
            RefreshBonuses();
        }
        get
        {
            return _count;
        }
    }

    private void Start()
    {
        RefreshBonuses();
    }

    private void RefreshBonuses()
    {
        for (int i = 0; i < bonuses.Length; i++)
        {
            bonuses[i].SetActive(i == _count);
        }
    }
}

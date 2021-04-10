using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonuses : MonoBehaviour
{
    public GameObject[] bonuses = null;
    private int _count = 0;
    private bool countChanged = false;
    public int Count
    {
        set
        {
            _count = (value < 0 ? 0 : (value >= bonuses.Length ? 0 : value));
            countChanged = true;
        }
        get
        {
            return _count;
        }
    }

    private void Update()
    {
        if (!GameController.instance.GameTimer.isPaused())
        {
            if (countChanged)
            {
                RefreshBonuses();
                countChanged = false;
            }
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

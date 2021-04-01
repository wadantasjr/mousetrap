using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{

    // Reference to an instance of this class.
    public static GameController instance = null;

    private Player Player = null;

    //public PathNode PlayerStart = null;
    public doorsGroup BlueDoor = null;
    public doorsGroup YellowDoor = null;
    public doorsGroup RedDoor = null;

    public GameObject Cheese = null;
    public GameObject PathNodes = null;

    private int Score = 0;

    public TMP_Text TxtScore = null;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one GameController in the scene.");
        }
        instance = this;
    }

    private void Start()
    {
        Player = Player.instance;
        if (Player == null)
        {
            Debug.LogError("There is no Player in the scene.");
        }
        //else if (PlayerStart != null)
        //{
        //    Player.transform.position = PlayerStart.transform.position;
        //    Player.currentNode = PlayerStart;
        //    Player.nextNode = PlayerStart;
        //}

        if (Cheese != null && PathNodes != null)
        {
            InitializeCheeses();
        }
    }

    private void InitializeCheeses()
    {
        foreach (PathNode pn  in PathNodes.GetComponentsInChildren<PathNode>())
        {
            if (pn.tag == "PathNode")
            {
                GameObject c = Instantiate(Cheese, this.transform);
                c.transform.position = pn.transform.position;
            }
        }
    }

    public void AddToScore(int value)
    {
        Score += value;
    }

    void Update()
    {
        //Game Loop

        CheckInput();
        RefreshScore();
    }

    private void RefreshScore()
    {
        TxtScore.text = Score.ToString();
    }

    private void CheckInput()
    {

        #region Doors
        if (BlueDoor != null)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                BlueDoor.isStateOn = !BlueDoor.GetComponent<doorsGroup>().isStateOn;
            }
        }
        if (YellowDoor != null)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                YellowDoor.isStateOn = !YellowDoor.GetComponent<doorsGroup>().isStateOn;
            }
        }
        if (RedDoor != null)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                RedDoor.isStateOn = !RedDoor.GetComponent<doorsGroup>().isStateOn;
            }
        }
        #endregion
    }

 }

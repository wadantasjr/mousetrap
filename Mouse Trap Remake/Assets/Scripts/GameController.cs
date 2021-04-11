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
    public GameObject BonusValue = null;

    public BoneCounter BoneCounter = null;

    public Bonuses Bonuses = null;

    private static readonly System.Random getrandom = new System.Random();

    [SerializeField]
    public GameTimer GameTimer = null;

    //Function to get random number
    public int GetRandomNumber(int min, int max)
    {
        lock (getrandom) // synchronize
        {
            return getrandom.Next(min, max);
        }
    }

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

        BonusValue.SetActive(false);
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

    public void AddToBoneCounter(int value)
    {
        BoneCounter.Count += value;
    }

    public void HitBonuses(Vector3 pos, int score)
    {
        StartCoroutine(ShowBonusValue(pos, score));
        Bonuses.Count++;
    }

    IEnumerator ShowBonusValue(Vector3 pos, int score)
    {
        BonusValue.GetComponentInChildren<TMP_Text>().text = score.ToString();
        BonusValue.transform.position = pos + new Vector3(0,4.6f,0);
        BonusValue.SetActive(true);
        GameTimer.PauseStart();
        yield return new WaitForSeconds(0.5f);
        BonusValue.SetActive(false);
        GameTimer.PauseStop();
    }

    void Update()
    {
        //Game Loop
        if (!GameTimer.isPaused())
        {
            CheckInput();
            RefreshScore();
        }
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

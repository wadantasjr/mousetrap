using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[Serializable]
public class PortalIn2Out
{
    public PathNode PortalIn = null;
    public PathNode PortalOut = null;
    public PathNode PathNodeTriggered = null;
}


public class Player : MonoBehaviour
{
    public static Player instance = null;
    public PathNode currentNode = null;
    public PathNode nextNode = null;
    private Vector2 direction = Vector2.zero;
    private Vector2 nextNodedirection = Vector2.right; //like the arcade
    private string pathNodeTriggeredName = "";
    public float speed = 4.0f;
    private Animator m_Animator;

    public PortalIn2Out[] portalIn2Out;
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
        m_Animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        CheckInput();
        MovePlayer();
        CheckCurrentNode();
    }

    private void CheckInput()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            MovePlayerToDirection(Vector2.right);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            MovePlayerToDirection(Vector2.left);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            MovePlayerToDirection(Vector2.up);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            MovePlayerToDirection(Vector2.down);
        }
        else
        {
            MovePlayerToDirection(Vector2.zero);
        }
    }
    


    private void CheckCurrentNode()
    {
        if (nextNode != null)
        {
            if (this.transform.position == nextNode.transform.position)
            {
                currentNode = nextNode;
            }
        }
    }


    public void MovePlayerToDirection(Vector2 d)
    {
        if (currentNode == nextNode)
        {
            if (CheckDirection(d) || d == Vector2.zero) //Verify the viability of the player´s direction want to move
            {
                direction = d;
                RefeshNextNode(d);
            }
            else if (CheckDirection(nextNodedirection) && d != (Vector2)(currentNode.transform.position - transform.position).normalized) //Verify the viability of the nextNodeDirection and if the player´s direction want to move is diferent from the direction to the currentNode
            {
                //Simulate the arcede movement, where the player continue 
                //to move if the previous movement still ok
                direction = nextNodedirection;
                RefeshNextNode(nextNodedirection);
            }
        }
        else
        {
            //verify the direction with player in the middle of the nodes
            if (d == direction )
            {
                direction = d;
            }
            else if (d == direction * -1)
            {
                direction = d;
            }
            else if (d == Vector2.zero)
            {
                direction = d;
            }
            else if (direction == Vector2.zero && CheckDirection(d))
            {
                direction = d;
            }
            else if (direction == Vector2.zero && d == nextNodedirection * -1)
            {
                direction = d;
            }
            else if (CheckDirection(nextNodedirection) )
            {
                //Simulate the arcade movement, where the player continue 
                //to move if the previous movement still ok
                direction = nextNodedirection;
                RefeshNextNode(nextNodedirection);
            }
        }

        if (direction != Vector2.zero)
        {
            m_Animator.SetTrigger("walking");            
        }
        else
        {
            m_Animator.ResetTrigger("walking");
        }
    }

    private void RefeshNextNode(Vector2 d)
    {
        bool flag = false;
        if (d != Vector2.zero)
        {
            for (int i = 0; i < currentNode.validDirections.Length; i++)
            {
                //if (verifyValidDiretionsEnabled(currentNode, i))
                //{
                    if (d == currentNode.validDirections[i])
                    {
                        nextNode = currentNode.neighbors[i];
                        nextNodedirection = currentNode.validDirections[i];
                        flag = true;
                    }
                //}
            }
        }
   }
    private bool verifyValidDiretionsEnabled(PathNode pn, int i)
    {
        bool r = true;
        try
        {
            if (GameController.instance.BlueDoor.isStateOn
                && pn.GetComponent<PathNode>().neighborsBlueDoorStateOn.Length > i)
            {
                r = pn.GetComponent<PathNode>().neighborsBlueDoorStateOn[i];
            }
            else if (!GameController.instance.BlueDoor.isStateOn
                && pn.GetComponent<PathNode>().neighborsBlueDoorStateOff.Length > i)
            {
                r = pn.GetComponent<PathNode>().neighborsBlueDoorStateOff[i];
            }

            if (GameController.instance.YellowDoor.isStateOn
                && pn.GetComponent<PathNode>().neighborsYellowDoorStateOn.Length > i)
            {
                r = pn.GetComponent<PathNode>().neighborsYellowDoorStateOn[i];
            }
            else if (!GameController.instance.YellowDoor.isStateOn
                && pn.GetComponent<PathNode>().neighborsYellowDoorStateOff.Length > i)
            {
                r = pn.GetComponent<PathNode>().neighborsYellowDoorStateOff[i];
            }

            if (GameController.instance.RedDoor.isStateOn
                && pn.GetComponent<PathNode>().neighborsRedDoorStateOn.Length > i)
            {
                r = pn.GetComponent<PathNode>().neighborsRedDoorStateOn[i];
            }
            else if (!GameController.instance.RedDoor.isStateOn
                && pn.GetComponent<PathNode>().neighborsRedDoorStateOff.Length > i)
            {
                r = pn.GetComponent<PathNode>().neighborsRedDoorStateOff[i];
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Player::verifyValidDiretionsEnabled: " + e);
        }

        return r;
    }

    private bool CheckDirection(Vector2 d)
    {
        bool r = false;

        for(int i=0;i<currentNode.validDirections.Length;i++)
        {
            if (verifyValidDiretionsEnabled(currentNode,i))
            {
                if (d == currentNode.validDirections[i])
                {
                    r = true;
                    break;
                }
            }
        }
        return r;
    }

    private bool CheckFutureDirection(Vector2 d)
    {
        bool r = false;
        if (d != nextNodedirection || (d * -1) != nextNodedirection)
        {
            for (int i = 0; i < nextNode.validDirections.Length; i++)
            {
                if (verifyValidDiretionsEnabled(nextNode, i))
                {
                    if (d == nextNode.validDirections[i])
                    {
                        r = true;
                        break;
                    }
                }
            }

            if (!r && currentNode == nextNode)
            {
                RefeshNextNode(d);
                if (currentNode != nextNode)
                {
                    for (int i = 0; i < nextNode.validDirections.Length; i++)
                    {
                        if (verifyValidDiretionsEnabled(nextNode, i))
                        {
                            if (d == nextNode.validDirections[i])
                            {
                                r = true;
                                break;
                            }
                        }
                    }
                }
            }
        }
        return r;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        try
        {
            switch (col.gameObject.tag)
            {
                case "Door":
                    direction = Vector2.zero;
                    nextNode = currentNode;
                    this.transform.position = currentNode.transform.position;
                    break;
                //Old portal mechanic----------------------------------------------------------
                //case "PathNode": //Only PathNodes with collision (trigger)
                //    pathNodeTriggeredName = col.gameObject.name; //Extract number in the name
                //    break;
                //-----------------------------------------------------------------------------
                case "Portal": //PathNode taged as Portal with collision (trigger)
                    //modifying the mechanics of the portals for the destination portal be random, just as it is in the original game
                    if (portalIn2Out.Length > 0)
                    {
                        int toPortalOut = GameController.instance.GetRandomNumber(0, portalIn2Out.Length);
                        direction = Vector2.zero;
                        currentNode = portalIn2Out[toPortalOut].PortalOut; ;
                        nextNode = currentNode;
                        this.transform.position = currentNode.transform.position;
                    }
                    break;
                //Old portal mechanic-------------------------------------------------------------------------------------
                //foreach (PortalIn2Out pio in portalIn2Out)
                //{
                //    if (pio.PortalIn.name == col.gameObject.name && pio.PathNodeTriggered.name == pathNodeTriggeredName)
                //    {
                //        direction = Vector2.zero;
                //        currentNode = pio.PortalOut;
                //        nextNode = currentNode;
                //        this.transform.position = currentNode.transform.position;
                //        break;
                //    }
                //}
                //--------------------------------------------------------------------------------------------------------
                case "Cheese": //Todo: Score
                    GameController.instance.AddToScore(col.gameObject.GetComponent<GameObjInfo>().ScorePoints);
                    Destroy(col.gameObject);
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Player::OnTriggerStay2D: " + e);
            Debug.Log(col.gameObject.name + "(" + col.gameObject.tag + ") : " + gameObject.name + " : " + Time.time);
            Debug.Log("pathNodeNumberTriggered : " + pathNodeTriggeredName);

        }
    }
    private void MovePlayer()
    {
        //Need to invert the direction?
        //Vector2 dirCurrentNode = (currentNode.transform.position - transform.position).normalized;
        int invertDirection = (nextNodedirection == (direction * -1)? -1 : 1);

        float step = speed * Time.deltaTime * (direction != Vector2.zero ? 1 : 0);
        if (invertDirection > 0 /*|| (direction == dirCurrentNode * -1)*/)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextNode.transform.position, step);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, currentNode.transform.position, step);

            //check if it is closer to curentNode than nextNode
            float d1 = (transform.position - nextNode.transform.position).sqrMagnitude;
            float d2 = (transform.position - currentNode.transform.position).sqrMagnitude;
            if (d1 > d2)
            {
                nextNode = currentNode;
            }
        }
    }
}

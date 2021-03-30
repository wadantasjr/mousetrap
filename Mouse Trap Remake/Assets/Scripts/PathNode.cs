using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    public PathNode[] neighbors;
    public Vector2[] validDirections;
    public bool[] neighborsBlueDoorStateOn;
    public bool[] neighborsBlueDoorStateOff;
    public bool[] neighborsYellowDoorStateOn;
    public bool[] neighborsYellowDoorStateOff;
    public bool[] neighborsRedDoorStateOn;
    public bool[] neighborsRedDoorStateOff;

    void Start()
    {
        SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.enabled = false;

        if (neighbors.Length > 0)
        {
            validDirections = new Vector2[neighbors.Length];
            for(int i=0; i< neighbors.Length; i++)
            {
                PathNode neighbor = neighbors[i];
                Vector2 tempVector = neighbor.transform.localPosition - transform.localPosition;

                validDirections[i] = tempVector.normalized;
            }
        }
    }


}

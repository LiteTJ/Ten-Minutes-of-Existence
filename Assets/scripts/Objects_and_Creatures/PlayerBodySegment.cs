using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodySegment : MonoBehaviour
{
    public void DestroyMe()
    {
        Debug.Log("Segment destroyed");
        Destroy(gameObject);
    }
}

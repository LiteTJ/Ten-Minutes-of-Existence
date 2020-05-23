using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCam : MonoBehaviour
{
    public GameObject player;

    //Between 0 and 1
    private float followSpeed = 0.16f;

    public void UpdatePosition()
    {
        Vector3 deltaPos = player.transform.position - transform.position;
        deltaPos.z = 0;

        transform.position += deltaPos * followSpeed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloaterEnemy : MonoBehaviour
{
    private Rigidbody2D rb;

    private float drag = 1f;
    private float maxPushForce = 10.8f;
    private float minAngularSpeed = 0.2f;
    private float maxAngularSpeed = 0.6f;
    private bool rotateClockwise;

    private void RotateFloater()
    {
        if(rotateClockwise)
        {
            transform.eulerAngles += Vector3.forward * Random.Range(minAngularSpeed, maxAngularSpeed);
        } else
        {
            transform.eulerAngles -= Vector3.forward * Random.Range(minAngularSpeed, maxAngularSpeed);
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = drag;

        rotateClockwise = Random.value > 0.5f;  
    }

    public void Move()
    {
        Vector2 force = new Vector2(
            Random.Range(-maxPushForce, maxPushForce),
            Random.Range(-maxPushForce, maxPushForce)
            );

        rb.AddForce(force);

        RotateFloater();
    }

}

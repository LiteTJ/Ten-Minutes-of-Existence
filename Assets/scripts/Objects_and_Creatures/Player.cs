using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Player : MonoBehaviour
{
    public GameObject bodyContainer;
    public GameObject bodySegmentPrefab;
    public GameObject playerLight;

    private Light2D light2D;

    private ArrayList bodySegments = new ArrayList();
    private Rigidbody2D rb;
    private GameObject newlyCollectedGemstone;
    private string state = "DEFAULT";
    private bool withinGateArea = false;
    private bool withinEndArea = false;

    private readonly int noSegments = 6;
    private float speedFactor = 5f;
    private float maxForce = 30f;
    private float drag = 4f;
    private float maxLightIntensity = 1f;
    private float minLightIntensity = 0.6f;
    private float flickerSpeed = 0.04f;

    public string GetState()
    {
        return state;
    }

    public void SetState(string _state)
    {
        state = _state;
    }

    public int GetBodyCount()
    {
        return bodySegments.Count;
    }

    public bool GetWithinGateArea()
    {
        return withinGateArea;
    }

    public bool GetWithinEndArea()
    {
        return withinEndArea;
    }

    public GameObject GetNewlyCollectedGemstone()
    {
        return newlyCollectedGemstone;
    }

    public void ResetNewlyCollectedGemstone()
    {
        newlyCollectedGemstone = null;
    }

    //Start didn't work as originally the GameController's start was called before this method
    //But we need the rigidbody reference
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = drag;

        light2D = playerLight.GetComponent<Light2D>();
    }

    public void CreateBodySegments()
    {
        for(int i = 0; i < noSegments; i++)
        {
            //Create body segment
            GameObject bodySegment = Instantiate(bodySegmentPrefab, transform.position, Quaternion.identity, bodyContainer.transform);

            //Set size
            float sizeFactor = (i + 1) * 0.04f;

            bodySegment.transform.localScale = new Vector3(
                1.0f - sizeFactor,
                1.0f - sizeFactor,
                1.0f
                );

            //Set opacity
            float maxAlpha = 0.5f;
            float minAlpha = 0.1f;

            Material material = bodySegment.GetComponent<Renderer>().material;

            var col = material.color;

            float alpha = maxAlpha - i * 0.08f;
            if (alpha < minAlpha) alpha = minAlpha;

            col.a = alpha;
            material.color = col;

            //Push body segment to list
            bodySegments.Add(bodySegment);
        }
    }

    public void MovePlayer(Vector2 mousePos)
    {
        Vector2 deltaPos = mousePos - rb.position;
        Vector2 force = deltaPos * speedFactor;

        if(force.magnitude > maxForce)
        {
            float factor = maxForce / force.magnitude;
            force *= factor;
        }

        rb.AddForce(force);
    }

    public void UpdateBodySegmentsPos()
    {
        for(int i = 0; i < bodySegments.Count; i++)
        {
            GameObject segment = (GameObject) bodySegments[i];

            float factor = 0.05f * (i + 1);

            Vector3 offset = new Vector3(
                    rb.velocity.x * factor,
                    rb.velocity.y * factor,
                    transform.position.z
                );

            segment.transform.position = transform.position - offset;
        }
    }

    public void RemoveBodySegment()
    {
        int lastIndex = bodySegments.Count - 1;

        GameObject segment = (GameObject) bodySegments[lastIndex];
        bodySegments.RemoveAt(lastIndex);

        Animator animator = segment.GetComponent<Animator>();
        animator.SetTrigger("FadeOut");
    }

    public void FlickerLight()
    {
        light2D.intensity += Random.Range(-flickerSpeed, flickerSpeed);
        light2D.intensity = Mathf.Clamp(light2D.intensity, minLightIntensity, maxLightIntensity);

        light2D.pointLightOuterRadius = light2D.intensity * 2.2f + 3.0f;
        light2D.pointLightInnerRadius = light2D.pointLightOuterRadius * 0.5f;
    }

    public void Respawn(GameObject start)
    {
        transform.position = start.transform.position;
        rb.velocity = Vector2.zero;

        state = "ALIVE";
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Gemstone"))
        {
            newlyCollectedGemstone = collision.gameObject;
        }

        if(collision.gameObject.tag.Equals("GateTrigger"))
        {
            withinGateArea = true;
        }

        if (collision.gameObject.tag.Equals("End"))
        {
            withinEndArea = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("GateTrigger"))
        {
            withinGateArea = false;
        }

        if (collision.gameObject.tag.Equals("End"))
        {
            withinEndArea = false;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag.Equals("Harmful"))
        {
            Debug.Log("Player dies");
            state = "DEAD";
        }
    }
}

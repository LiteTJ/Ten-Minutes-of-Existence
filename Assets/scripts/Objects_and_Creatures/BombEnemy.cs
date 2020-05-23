using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEnemy : MonoBehaviour
{
    private Level level;
    private GameObject floaterEnemyContainer;
    private GameObject floaterEnemyHarmfulPrefab;

    private Rigidbody2D rb;
    private float drag = 1f;
    private float maxPushForce = 10.8f;
    private float maxExplodeForce = 700f;
    private float explodeDistance = 3.8f;
    private int noChildren = 12;

    private void Explode()
    {
        //Create children
        for(int i = 0; i < noChildren; i++)
        {
            GameObject child = Instantiate(floaterEnemyHarmfulPrefab, transform.position, Quaternion.identity, floaterEnemyContainer.transform);
            level.floaterEnemies.Add(child);

            //Push children
            Vector2 force = new Vector2
                (
                Random.Range(-maxExplodeForce, maxExplodeForce),
                Random.Range(-maxExplodeForce, maxExplodeForce)
                );

            child.GetComponent<Rigidbody2D>().AddForce(force);
        }

        //Destroy me
        level.bombEnemies.Remove(gameObject);
        Destroy(gameObject);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = drag;

        level = GameObject.FindWithTag("LevelController").GetComponent<Level>();

        floaterEnemyContainer = level.floaterEnemyContainer;
        floaterEnemyHarmfulPrefab = level.floaterEnemyHarmfulPrefab;
    }

    public void Move()
    {
        Vector2 force = new Vector2(
            Random.Range(-maxPushForce, maxPushForce),
            Random.Range(-maxPushForce, maxPushForce)
            );

        rb.AddForce(force);
    }

    public void CheckExplode(GameObject player)
    {
        Vector2 deltaDist = transform.position - player.transform.position;

        if(deltaDist.magnitude < explodeDistance)
        {
            Explode();
        }
    }
}

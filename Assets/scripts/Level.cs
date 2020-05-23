using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public GameObject floaterEnemyContainer;
    public GameObject bombEnemyContainer;

    public GameObject floaterEnemyPrefab;
    public GameObject floaterEnemyHarmfulPrefab;
    public GameObject floaterStarPrefab;
    public GameObject bombEnemyPrefab;

    public GameObject[] gemstones;
    public GameObject gate;
    public bool[] gemstoneCollected;

    [HideInInspector] public ArrayList floaterEnemies = new ArrayList();
    [HideInInspector] public ArrayList bombEnemies = new ArrayList();

    public float maxFloaterSpawnX;
    public float maxFloaterSpawnY;
    public float minFloaterSpawnX;
    public float minFloaterSpawnY;

    private int noFloaterEnemies = 400;
    private int noBombEnemies = 40;
    private float floaterHarmfulChance = 0.2f;
    private float floaterStarChance = 0.1f;

    public void SetNoFloaterEnemies(int no)
    {
        noFloaterEnemies = no;
    }

    public void SetNoBombEnemies(int no)
    {
        noBombEnemies = no;
    }

    public void SetFloaterHarmfulChance(float chance)
    {
        floaterHarmfulChance = chance;
    }

    public void SetFloaterStarChance(float chance)
    {
        floaterStarChance = chance;
    }

    public void CreateFloaterEnemies()
    {
        for (int i = 0; i < noFloaterEnemies; i++)
        {
            //Get random position
            Vector3 randomPosition = new Vector3(
                Random.Range(minFloaterSpawnX, maxFloaterSpawnX),
                Random.Range(minFloaterSpawnY, maxFloaterSpawnY),
                transform.position.z
                );

            //Whether to spawn a normal floater, a harmful floater or a star
            GameObject floaterType;

            float rand = Random.Range(0f, 1f);
            if (rand < floaterHarmfulChance)
            {
                floaterType = floaterEnemyHarmfulPrefab;
            }
            else if (rand < floaterHarmfulChance + floaterStarChance)
            {
                floaterType = floaterStarPrefab;
            }
            else
            {
                floaterType = floaterEnemyPrefab;
            }

            //Create floater enemy
            GameObject floaterEnemy = Instantiate(floaterType, randomPosition, Quaternion.identity, floaterEnemyContainer.transform);

            floaterEnemies.Add(floaterEnemy);
        }
    }

    public void CreateBombEnemies()
    {
        for(int i = 0; i < noBombEnemies; i++)
        {
            //Get random position
            Vector3 randomPosition = new Vector3(
                Random.Range(minFloaterSpawnX, maxFloaterSpawnX),
                Random.Range(minFloaterSpawnY, maxFloaterSpawnY),
                transform.position.z
                );

            //Create bomb enemy
            GameObject bombEnemy = Instantiate(bombEnemyPrefab, randomPosition, Quaternion.identity, bombEnemyContainer.transform);

            bombEnemies.Add(bombEnemy);
        }
    }

    public bool AllGemstonesCollected()
    {
        bool _return = true;

        foreach(bool gemCollected in gemstoneCollected)
        {
            if (!gemCollected) _return = false;
        }

        return _return;
    }

    public void ResetGemstones()
    {
        gemstoneCollected = new bool[gemstones.Length];

        for(int i = 0; i < gemstoneCollected.Length; i++)
        {
            gemstoneCollected[i] = false;
        }
    }

    public void CreateEnemies()
    {
        CreateFloaterEnemies();
        CreateBombEnemies();
    }

    public void UpdateEnemies(GameObject player)
    {
        foreach(GameObject floaterEnemy in floaterEnemies)
        {
            floaterEnemy.GetComponent<FloaterEnemy>().Move();
        }

        for(int i = 0; i < bombEnemies.Count; i++)
        {
            GameObject bombEnemy = (GameObject) bombEnemies[i];
            BombEnemy bombScript = bombEnemy.GetComponent<BombEnemy>();

            bombScript.Move();
            bombScript.CheckExplode(player);
        }
    }

    public void DestroyAllEnemies()
    {
        for(int i = floaterEnemies.Count - 1; i >= 0; i--)
        {
            GameObject enemy = (GameObject) floaterEnemies[i];
            floaterEnemies.Remove(enemy);
            Destroy(enemy);
        }

        for (int i = bombEnemies.Count - 1; i >= 0; i --)
        {
            GameObject enemy = (GameObject) bombEnemies[i];
            bombEnemies.Remove(enemy);
            Destroy(enemy);
        }
    }
}

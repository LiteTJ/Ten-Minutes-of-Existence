using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public GameObject startIndicator;
    public GameObject transitionController;

    public Player player;
    public Level testLevel;

    private Vector2 GetMousePos()
    {
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos = new Vector2(mousePos3D.x, mousePos3D.y);

        return mousePos;
    }

    public void CreateFloaterEnemies()
    {
        testLevel.SetNoFloaterEnemies(10);
        testLevel.SetFloaterHarmfulChance(0);
        testLevel.SetFloaterStarChance(0);

        testLevel.CreateFloaterEnemies();
    }

    public void CreateFloaterEnemiesHarmful()
    {
        testLevel.SetNoFloaterEnemies(10);
        testLevel.SetFloaterHarmfulChance(1);
        testLevel.SetFloaterStarChance(0);

        testLevel.CreateFloaterEnemies();
    }

    public void CreateStars()
    {
        testLevel.SetNoFloaterEnemies(8);
        testLevel.SetFloaterHarmfulChance(0);
        testLevel.SetFloaterStarChance(1);

        testLevel.CreateFloaterEnemies();
    }

    public void CreateBombEnemies()
    {
        testLevel.SetNoBombEnemies(8);
        testLevel.CreateBombEnemies();
    }

    private void Start()
    {
        player.CreateBodySegments();
        player.Respawn(startIndicator);
    }

    private void FixedUpdate()
    {
        player.MovePlayer(GetMousePos());
        player.UpdateBodySegmentsPos();

        testLevel.UpdateEnemies(player.gameObject);
    }

    public void EndTutorial()
    {
        transitionController.GetComponent<Animator>().SetTrigger("EndTutorial");
    }
}
